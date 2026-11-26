// Fails on an asset without a .meta (Unity skips it in a git install) or HHTheme drifting from the tokens.
import { spawnSync } from "node:child_process";
import { existsSync, readdirSync, readFileSync, statSync } from "node:fs";
import { join } from "node:path";

// Unity ignores hidden names and names ending in ~, and never imports a .meta itself
const imported = (name) => !name.startsWith(".") && !name.endsWith("~") && !name.endsWith(".meta");
const roots = readdirSync(".").filter(imported);
const problems = [];

// A .meta that exists on disk but was never committed still leaves the asset out of a git install
const listed = spawnSync("git", ["ls-files", "-z"], { encoding: "utf8" });
const tracked = listed.status === 0 ? new Set(listed.stdout.split("\0").filter(Boolean)) : null;
const isTracked = (path) => [...tracked].some((file) => file === path || file.startsWith(`${path}/`));

function walk(path) {
  if (!existsSync(`${path}.meta`)) {
    problems.push(`${path}: no .meta`);
  } else if (tracked && isTracked(path) && !tracked.has(`${path}.meta`)) {
    problems.push(`${path}.meta: not committed`);
  }
  if (statSync(path).isDirectory()) {
    for (const name of readdirSync(path).filter(imported)) {
      walk(join(path, name));
    }
  }
}

const guids = new Map();
function collectGuids(path) {
  if (!existsSync(path)) {
    return;
  }
  if (statSync(path).isDirectory()) {
    for (const name of readdirSync(path)) {
      if (!name.startsWith(".") && !name.endsWith("~")) {
        collectGuids(join(path, name));
      }
    }
    return;
  }
  if (!path.endsWith(".meta")) {
    return;
  }
  const guid = readFileSync(path, "utf8").match(/^guid: ([0-9a-f]{32})\r?$/m)?.[1];
  if (!guid) {
    problems.push(`${path}: no guid`);
  } else if (guids.has(guid)) {
    problems.push(`${path}: guid also used by ${guids.get(guid)}`);
  } else {
    guids.set(guid, path);
  }
}

for (const root of roots) {
  walk(root);
  collectGuids(`${root}.meta`);
  if (statSync(root).isDirectory()) {
    collectGuids(root);
  }
}

// Unity serializes fields by name, so 0.1's `background` keeps its name to keep saved themes
const aliases = { "color-bg": "background" };
const fieldFor = (token) =>
  aliases[token] ?? token.replace(/^color-/, "").replace(/-(\w)/g, (_, c) => c.toUpperCase());

function parseColour(value) {
  const hex = value.match(/^#([0-9a-f]{6})([0-9a-f]{2})?$/i);
  if (hex) {
    const n = parseInt(hex[1], 16);
    return [n >> 16, (n >> 8) & 255, n & 255, hex[2] ? parseInt(hex[2], 16) : 255];
  }
  const rgb = value.match(/^rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*([\d.]+)\s*)?\)$/);
  if (rgb) {
    return [+rgb[1], +rgb[2], +rgb[3], rgb[4] === undefined ? 255 : Math.round(+rgb[4] * 255)];
  }
  return null;
}

const byte = (s) => parseInt(s.trim(), s.trim().startsWith("0x") ? 16 : 10);
const theme = readFileSync("Runtime/HHTheme.cs", "utf8");
const fields = new Map();
for (const m of theme.matchAll(/public Color (\w+) = new Color32\(([^)]*)\);/g)) {
  fields.set(m[1], { type: "colour", value: m[2].split(",").map(byte) });
}
for (const m of theme.matchAll(/public float (\w+) = ([\d.]+)f;/g)) {
  fields.set(m[1], { type: "px", value: +m[2] });
}

const uss = readFileSync("Runtime/Styles/hh-tokens.uss", "utf8").replace(/\/\*[\s\S]*?\*\//g, "");
const tokens = [...uss.matchAll(/--hh-([\w-]+)\s*:\s*([^;]+);/g)].map(([, name, value]) => [name, value.trim()]);
const mirrored = new Set();
for (const [name, raw] of tokens) {
  const field = fieldFor(name);
  const f = fields.get(field);
  mirrored.add(field);
  if (!f) {
    problems.push(`--hh-${name}: no HHTheme.${field}`);
    continue;
  }
  const px = raw.match(/^(\d+(?:\.\d+)?)px$/);
  const want = name.startsWith("color-") ? parseColour(raw) : px ? +px[1] : null;
  if (want === null) {
    problems.push(`--hh-${name}: can't parse ${raw}`);
  } else if (f.type === "colour" ? !Array.isArray(want) || want.some((c, i) => c !== f.value[i]) : f.value !== want) {
    problems.push(`--hh-${name} is ${raw} but HHTheme.${field} is ${f.value}`);
  }
}
for (const field of fields.keys()) {
  if (!mirrored.has(field)) {
    problems.push(`HHTheme.${field} has no matching token`);
  }
}

if (problems.length > 0) {
  console.error(problems.join("\n"));
  process.exit(1);
}
console.log(`${guids.size} .meta files ok, ${tokens.length} tokens match HHTheme`);
