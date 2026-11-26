// Runs check.mjs against a clean copy of the package and against copies broken one way each.
import assert from "node:assert/strict";
import { spawnSync } from "node:child_process";
import { cpSync, mkdtempSync, readFileSync, rmSync, unlinkSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { dirname, join, resolve } from "node:path";
import { fileURLToPath } from "node:url";

const root = resolve(dirname(fileURLToPath(import.meta.url)), "..");
const check = join(root, "Tools~", "check.mjs");

function run(breakIt) {
  const dir = mkdtempSync(join(tmpdir(), "hh-check-"));
  try {
    cpSync(root, dir, { recursive: true, filter: (src) => !src.includes(`${join(root, ".git")}`) });
    breakIt(dir);
    const result = spawnSync(process.execPath, [check], { cwd: dir, encoding: "utf8" });
    return { code: result.status, out: result.stdout + result.stderr };
  } finally {
    rmSync(dir, { recursive: true, force: true });
  }
}

const edit = (dir, file, from, to) => {
  const path = join(dir, file);
  const text = readFileSync(path, "utf8");
  assert.ok(text.includes(from), `${file} has no ${from}`);
  writeFileSync(path, text.replace(from, to));
};

const cases = [
  ["clean package passes", () => {}, 0, /tokens match HHTheme/],
  ["asset without a .meta", (d) => unlinkSync(join(d, "Runtime/HHButton.cs.meta")), 1, /HHButton\.cs: no \.meta/],
  ["new top-level folder without a .meta", (d) => { cpSync(join(d, "README.md"), join(d, "docs/x.md"), { recursive: true }); }, 1, /docs: no \.meta/],
  ["two .meta files share a guid", (d) => cpSync(join(d, "Editor.meta"), join(d, "Runtime.meta")), 1, /guid also used by/],
  [".meta without a guid", (d) => writeFileSync(join(d, "README.md.meta"), "fileFormatVersion: 2\n"), 1, /README\.md\.meta: no guid/],
  ["colour drifts", (d) => edit(d, "Runtime/HHTheme.cs", "0x82, 0x57, 0xFF, 0xFF", "0x80, 0x57, 0xFF, 0xFF"), 1, /--hh-color-violet is #8257FF/],
  ["alpha drifts", (d) => edit(d, "Runtime/HHTheme.cs", "0xF3, 0xF3, 0xFB, 0x9E", "0xF3, 0xF3, 0xFB, 0xA0"), 1, /--hh-color-text-dim/],
  ["size drifts", (d) => edit(d, "Runtime/HHTheme.cs", "radiusSm = 6f", "radiusSm = 7f"), 1, /--hh-radius-sm is 6px but HHTheme\.radiusSm is 7/],
  ["token with no field", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-space-1: 4px;", "--hh-space-1: 4px;\n    --hh-space-8: 32px;"), 1, /--hh-space-8: no HHTheme\.space8/],
  ["field with no token", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", "    --hh-space-1: 4px;\n", ""), 1, /HHTheme\.space1 has no matching token/],
  ["unparsable colour", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-color-teal: #45D6C4;", "--hh-color-teal: teal;"), 1, /--hh-color-teal: can't parse teal/],
  ["size not in px", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-space-2: 8px;", "--hh-space-2: 8em;"), 1, /--hh-space-2: can't parse 8em/],
  ["commented-out token is ignored", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", ":root {", ":root {\n    /* --hh-space-9: 36px; */"), 0, /tokens match HHTheme/],
  [".meta with Windows line endings", (d) => writeFileSync(join(d, "README.md.meta"), readFileSync(join(d, "README.md.meta"), "utf8").replace(/\n/g, "\r\n")), 0, /tokens match HHTheme/],
  [".meta on disk but not committed", (d) => {
    const git = (...args) => assert.equal(spawnSync("git", ["-c", "user.name=t", "-c", "user.email=t@t", ...args], { cwd: d }).status, 0);
    git("init", "-q");
    git("add", "-A");
    git("rm", "-q", "--cached", "Runtime/HHButton.cs.meta");
  }, 1, /Runtime\/HHButton\.cs\.meta: not committed/],
  ["contrast below AA fails", (d) => {
    edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-color-text-dim: rgba(243, 243, 251, 0.62);", "--hh-color-text-dim: rgba(243, 243, 251, 0.30);");
    edit(d, "Runtime/HHTheme.cs", "0xF3, 0xF3, 0xFB, 0x9E", "0xF3, 0xF3, 0xFB, 0x4C");
  }, 1, /Dim text on a card: text-dim on surface is [\d.]+:1, needs 4\.5/],
  ["accepted pair is reported, not failed", () => {}, 0, /accepted: Button text: text on violet is 4\.0\d:1/],
  ["pair naming an unknown token fails", (d) => edit(d, "Tools~/contrast.json", '"text": "focus"', '"text": "focs"'), 1, /Focus ring against a card: no token --hh-color-focs/],
  ["translucent background without over fails", (d) => edit(d, "Tools~/contrast.json", ', "over": "surface" }', " }"), 1, /Ghost button text: violet-ghost is translucent, say what it sits over/],
];

let failed = 0;
for (const [name, breakIt, code, pattern] of cases) {
  const got = run(breakIt);
  const ok = got.code === code && pattern.test(got.out);
  console.log(`${ok ? "ok  " : "FAIL"} ${name}${ok ? "" : `: exit ${got.code}, ${got.out.trim()}`}`);
  if (!ok) {
    failed += 1;
  }
}
console.log(`${cases.length - failed}/${cases.length} passed`);
process.exit(failed > 0 ? 1 : 0);
