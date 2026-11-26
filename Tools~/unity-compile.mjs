// Compiles every committed asmdef with Unity's own compiler, source generators and references, following its references and constraints. Exit 0 clean, 1 failed, 2 setup.
import { spawnSync } from "node:child_process";
import { existsSync, mkdirSync, mkdtempSync, readdirSync, readFileSync, rmSync, statSync } from "node:fs";
import { tmpdir } from "node:os";
import { basename, dirname, join, relative } from "node:path";

const rev = process.argv[2] ?? "HEAD";
const setupFail = (message) => {
  console.log(`FAIL ${message}`);
  process.exit(2);
};

const root = spawnSync("git", ["rev-parse", "--show-toplevel"], { encoding: "utf8" });
if (root.status !== 0) {
  setupFail("run from inside the package repo");
}
const repo = root.stdout.trim();

// HH_UNITY_EDITOR wins; otherwise the newest 6000.0 under Unity Hub, then the cloud install
function findEditor() {
  if (process.env.HH_UNITY_EDITOR) {
    return process.env.HH_UNITY_EDITOR;
  }
  const hub = "C:/Program Files/Unity/Hub/Editor";
  if (existsSync(hub)) {
    const versions = readdirSync(hub).filter((v) => v.startsWith("6000.0.")).sort((a, b) => a.localeCompare(b, undefined, { numeric: true }));
    if (versions.length > 0) {
      return join(hub, versions.at(-1), "Editor");
    }
  }
  return "/opt/unity/Editor";
}
const editorDir = findEditor();
const data = join(editorDir, "Data");
const dotnet = [join(data, "NetCoreRuntime", "dotnet.exe"), join(data, "NetCoreRuntime", "dotnet")].find(existsSync);
const csc = join(data, "DotNetSdkRoslyn", "csc.dll");
const managed = join(data, "Managed", "UnityEngine");
if (!dotnet || !existsSync(csc) || !existsSync(join(managed, "UnityEngine.UIElementsModule.dll"))) {
  setupFail(`no Unity 6000.0 compiler under ${editorDir}; set HH_UNITY_EDITOR to an Editor folder`);
}
const builtIn = join(data, "Resources", "PackageManager", "BuiltInPackages");
const testFramework = join(builtIn, "com.unity.test-framework");
const cecil = join(data, "Resources", "PackageManager", "Editor", "com.unity.nuget.mono-cecil-1.11.6.tgz");
for (const path of [testFramework, cecil]) {
  if (!existsSync(path)) {
    setupFail(`no ${path}; the gate expects Unity 6000.0's bundled test framework and Mono.Cecil`);
  }
}
const unityExe = [join(editorDir, "Unity.exe"), join(editorDir, "Unity")].find(existsSync);
const versionName = basename(dirname(editorDir)).match(/^\d/) ? basename(dirname(editorDir)) : (unityExe ? spawnSync(unityExe, ["-version"], { encoding: "utf8" }).stdout ?? "" : "").trim();

// Each run gets its own folder unless HH_WORK names one, so parallel runs can't trample each other
const work = process.env.HH_WORK ?? mkdtempSync(join(tmpdir(), "hh-compile-"));
rmSync(work, { recursive: true, force: true });
mkdirSync(join(work, "src"), { recursive: true });
mkdirSync(join(work, "out"), { recursive: true });
// Only what is committed, as a git install sees it
// tar runs on relative paths, since Git Bash's GNU tar reads C:\... as a remote host
const archive = spawnSync("git", ["archive", "--format=tar", "-o", join(work, "src.tar"), rev], { cwd: repo });
if (archive.status !== 0 || spawnSync("tar", ["-xf", "src.tar", "-C", "src"], { cwd: work }).status !== 0) {
  setupFail(`could not archive ${rev}`);
}
const src = join(work, "src");

const readJson = (path) => JSON.parse(readFileSync(path, "utf8").replace(/^\uFEFF/, ""));
const listDlls = (dir, prefix) => readdirSync(dir).filter((f) => f.startsWith(prefix) && f.endsWith(".dll")).map((f) => join(dir, f));

// An assembly owns the .cs files under its folder, down to any folder with its own asmdef
function sourcesOf(dir) {
  const files = [];
  for (const name of readdirSync(dir).sort()) {
    const path = join(dir, name);
    if (statSync(path).isDirectory()) {
      if (!name.startsWith(".") && !readdirSync(path).some((f) => f.endsWith(".asmdef"))) {
        files.push(...sourcesOf(path));
      }
    } else if (name.endsWith(".cs")) {
      files.push(path);
    }
  }
  return files;
}

function findAsmdefs(dir, skip) {
  const found = [];
  for (const name of readdirSync(dir).sort()) {
    const path = join(dir, name);
    if (statSync(path).isDirectory()) {
      if (!name.startsWith(".") && !skip(name)) {
        found.push(...findAsmdefs(path, skip));
      }
    } else if (name.endsWith(".asmdef")) {
      found.push(path);
    }
  }
  return found;
}

const assemblies = new Map();
const byGuid = new Map();
function addAssembly(path, origin) {
  const def = readJson(path);
  const meta = existsSync(`${path}.meta`) ? readFileSync(`${path}.meta`, "utf8").match(/^guid: (\w+)/m)?.[1] : undefined;
  const asm = { def, name: def.name, dir: dirname(path), origin, sources: sourcesOf(dirname(path)) };
  assemblies.set(def.name, asm);
  if (meta) {
    byGuid.set(meta, def.name);
  }
}
for (const path of findAsmdefs(src, (name) => name.endsWith("~"))) {
  addAssembly(path, "package");
}
for (const name of ["UnityEngine.TestRunner", "UnityEditor.TestRunner"]) {
  addAssembly(join(testFramework, name, `${name}.asmdef`), "unity");
}
// Each sample is compiled like a project's own scripts, which see every auto-referenced runtime assembly
const samples = existsSync(join(src, "Samples~")) ? readdirSync(join(src, "Samples~")).filter((n) => statSync(join(src, "Samples~", n)).isDirectory()) : [];
for (const sample of samples) {
  const dir = join(src, "Samples~", sample);
  const runtime = [...assemblies.values()].filter((a) => a.origin === "package" && a.def.autoReferenced !== false && !(a.def.includePlatforms ?? []).includes("Editor"));
  assemblies.set(`Sample.${sample}`, { def: { name: `Sample.${sample}`, references: runtime.map((a) => a.name) }, name: `Sample.${sample}`, dir, origin: "package", sources: sourcesOf(dir) });
}

const versionDefined = new Set(["Unity", "com.unity.test-framework", "com.unity.modules.uielements"]);
// Unity defines every UNITY_<year>_<n>_OR_NEWER up to the editor's own version
const baseDefines = ["UNITY_5_3_OR_NEWER", "UNITY_5_4_OR_NEWER", "UNITY_5_5_OR_NEWER", "UNITY_5_6_OR_NEWER", "UNITY_6000_0_OR_NEWER"];
for (let year = 2017; year <= 2023; year += 1) {
  for (let n = 1; n <= (year < 2023 ? 4 : 3); n += 1) {
    baseDefines.push(`UNITY_${year}_${n}_OR_NEWER`);
  }
}
const configs = {
  editor: { defines: [...baseDefines, "UNITY_EDITOR", "UNITY_INCLUDE_TESTS"], editorRefs: true },
  player: { defines: baseDefines, editorRefs: false },
};
const analyzers = ["Unity.UIToolkit.SourceGenerator.dll", "Unity.SourceGenerators.dll", "Unity.Properties.SourceGenerator.dll"].map((f) => join(data, "Tools", "Unity.SourceGenerators", f));
const precompiled = {
  "nunit.framework.dll": join(builtIn, "com.unity.ext.nunit", "net40", "unity-custom", "nunit.framework.dll"),
};
// The test runner needs Mono.Cecil, which Unity ships as a package tarball
{
  mkdirSync(join(work, "cecil"), { recursive: true });
  spawnSync("tar", ["-xzf", relative(join(work, "cecil"), cecil).split("\\").join("/"), "-C", "."], { cwd: join(work, "cecil") });
  for (const name of ["Mono.Cecil", "Mono.Cecil.Mdb", "Mono.Cecil.Pdb", "Mono.Cecil.Rocks"]) {
    precompiled[`${name}.dll`] = join(work, "cecil", "package", `${name}.dll`);
  }
}
// Precompiled .NET Framework assemblies resolve their framework types through these forwarders, as in Unity
const shimDir = join(data, "NetStandard", "compat", "2.1.0", "shims", "netfx");
const shims = readdirSync(shimDir).filter((f) => f.endsWith(".dll")).map((f) => join(shimDir, f));

const reference = (name) => (name.startsWith("GUID:") ? byGuid.get(name.slice(5)) ?? name : name);
let failed = 0;
const lines = [];

function compileAll(configName) {
  const config = configs[configName];
  const built = new Map();
  const skipped = new Set();
  // Only the Editor platform is modelled: an assembly limited to it skips the player, one excluding it skips the editor
  const editorOnly = (a) => (a.def.includePlatforms ?? []).includes("Editor");
  const notInEditor = (a) => (a.def.excludePlatforms ?? []).includes("Editor") || ((a.def.includePlatforms ?? []).length > 0 && !editorOnly(a));
  const pending = [...assemblies.values()].filter((a) => (configName === "player" ? a.origin !== "unity" && !editorOnly(a) : !notInEditor(a)));
  const outDir = join(work, "out", configName);
  mkdirSync(outDir, { recursive: true });
  while (pending.length > 0) {
    const index = pending.findIndex((a) => (a.def.references ?? []).map(reference).every((r) => built.has(r) || skipped.has(r) || !assemblies.has(r)));
    if (index === -1) {
      for (const asm of pending) {
        lines.push(`FAIL ${asm.name} [${configName}]: reference cycle or unbuilt reference`);
        failed += 1;
      }
      return;
    }
    const asm = pending.splice(index, 1)[0];
    const defines = [...config.defines, ...(asm.def.versionDefines ?? []).filter((v) => versionDefined.has(v.name)).map((v) => v.define)];
    const unmet = (asm.def.defineConstraints ?? []).filter((c) => (c.startsWith("!") ? defines.includes(c.slice(1)) : !defines.includes(c)));
    if (unmet.length > 0) {
      skipped.add(asm.name);
      continue;
    }
    const refs = [];
    const problems = [];
    for (const raw of asm.def.references ?? []) {
      const name = reference(raw);
      if (built.has(name)) {
        refs.push(built.get(name));
      } else if (!skipped.has(name)) {
        problems.push(`unknown reference ${raw}`);
      }
    }
    for (const dll of asm.def.precompiledReferences ?? []) {
      if (precompiled[dll]) {
        refs.push(precompiled[dll]);
        if (!refs.includes(shims[0])) {
          refs.push(...shims);
        }
      } else {
        problems.push(`no precompiled ${dll}`);
      }
    }
    if (problems.length > 0) {
      lines.push(`FAIL ${asm.name} [${configName}]: ${problems.join(", ")}`);
      failed += 1;
      continue;
    }
    if (asm.sources.length === 0) {
      lines.push(`FAIL ${asm.name} [${configName}]: no sources`);
      failed += 1;
      continue;
    }
    const engine = [...listDlls(managed, "UnityEngine"), ...(config.editorRefs ? listDlls(managed, "UnityEditor") : [])];
    const out = join(outDir, `${asm.name}.dll`);
    const args = [
      csc, "-nologo", "-noconfig", "-nostdlib", "-target:library", "-langversion:9.0", "-nullable:disable", "-deterministic",
      ...(asm.origin === "package" ? ["-warnaserror"] : ["-nowarn:CS0618,CS0612,CS0414,CS0649,CS0169"]),
      `-r:${join(data, "NetStandard", "ref", "2.1.0", "netstandard.dll")}`,
      ...[...engine, ...refs].map((r) => `-r:${r}`),
      ...analyzers.map((a) => `-analyzer:${a}`),
      ...defines.map((d) => `-define:${d}`),
      `-out:${out}`,
      ...asm.sources,
    ];
    const run = spawnSync(dotnet, args, { encoding: "utf8", maxBuffer: 64 * 1024 * 1024 });
    if (run.status === 0) {
      built.set(asm.name, out);
      if (asm.origin === "package") {
        lines.push(`ok   ${asm.name} [${configName}] (${asm.sources.length} files)`);
      }
    } else {
      failed += 1;
      const where = (text) => text.split(src).join("").split(testFramework).join("test-framework");
      const errors = [...new Set(run.stdout.split(/\r?\n/).filter((l) => /error|warning/.test(l)).map(where))].slice(0, 20);
      lines.push(`FAIL ${asm.name} [${configName}]:`, ...errors.map((e) => `  ${e}`));
    }
  }
}

compileAll("editor");
compileAll("player");
for (const line of lines) {
  console.log(line);
}
if (failed === 0) {
  const count = lines.filter((l) => l.startsWith("ok")).length;
  console.log(`ok   ${rev} compiles: ${count} assembly builds with the compiler and generators of Unity ${versionName}`);
}
if (!process.env.HH_WORK) {
  rmSync(work, { recursive: true, force: true });
}
process.exit(failed > 0 ? 1 : 0);
