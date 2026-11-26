// Package checks. Run from the package root: node Tools~/check.mjs  (exits 1 on any problem)
//  1. Every asset Unity imports has a committed .meta. Unity skips meta-less assets in a
//     package installed from git, because it can't write the .meta into the read-only cache.
//  2. HHTheme.cs defaults match hh-tokens.uss, so the two can't drift.
// Unity ignores folders ending in ~ and hidden folders, so this never ships into a project.
import { existsSync, readdirSync, readFileSync, statSync } from "node:fs";
import { join } from "node:path";

const uss = readFileSync("Runtime/Styles/hh-tokens.uss", "utf8");
const cs = readFileSync("Runtime/HHTheme.cs", "utf8");

// C# field names that don't follow the token name
const aliases = { "color-bg": "background" };

const fieldFor = (token) =>
  aliases[token] ??
  token.replace(/^color-/, "").replace(/-(\w)/g, (_, c) => c.toUpperCase());

const parseColour = (v) => {
  let m = v.match(/^#([0-9a-f]{6})([0-9a-f]{2})?$/i);
  if (m) {
    const n = parseInt(m[1], 16);
    return [n >> 16, (n >> 8) & 255, n & 255, m[2] ? parseInt(m[2], 16) : 255];
  }
  m = v.match(/^rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*([\d.]+)\s*)?\)$/);
  if (m) return [+m[1], +m[2], +m[3], m[4] === undefined ? 255 : Math.round(+m[4] * 255)];
  return null;
};

const tokens = [...uss.matchAll(/--hh-([\w-]+)\s*:\s*([^;]+);/g)].map(([, n, v]) => [n, v.trim()]);
const fields = new Map(
  [...cs.matchAll(/public\s+(Color|float)\s+(\w+)\s*=\s*new Color32\(([^)]*)\)|public\s+(float)\s+(\w+)\s*=\s*([\d.]+)f/g)]
    .map((m) => m[1]
      ? [m[2], { type: "Color", value: m[3].split(",").map((s) => parseInt(s.trim(), s.trim().startsWith("0x") ? 16 : 10)) }]
      : [m[5], { type: "float", value: +m[6] }]),
);

const problems = [];

// 1. .meta files
const imported = (name) => !name.startsWith(".") && !name.endsWith("~") && !name.endsWith(".meta");
const walk = (dir) => {
  for (const name of readdirSync(dir).filter(imported)) {
    const path = dir === "." ? name : join(dir, name);
    if (dir === "." && !["Runtime", "Editor", "package.json", "README.md", "CHANGELOG.md", "LICENSE.md"].includes(name)) continue;
    if (!existsSync(path + ".meta")) problems.push(`${path}: no .meta`);
    if (statSync(path).isDirectory()) walk(path);
  }
};
walk(".");
const guids = new Map();
const collectGuids = (dir) => {
  for (const name of readdirSync(dir)) {
    if (name.startsWith(".") || name.endsWith("~")) continue;
    const path = dir === "." ? name : join(dir, name);
    if (statSync(path).isDirectory()) collectGuids(path);
    else if (name.endsWith(".meta")) {
      const guid = readFileSync(path, "utf8").match(/^guid: ([0-9a-f]{32})$/m)?.[1];
      if (!guid) problems.push(`${path}: no guid`);
      else if (guids.has(guid)) problems.push(`${path}: guid also used by ${guids.get(guid)}`);
      else guids.set(guid, path);
    }
  }
};
collectGuids(".");

// 2. tokens
const used = new Set();
for (const [name, raw] of tokens) {
  const field = fieldFor(name);
  const f = fields.get(field);
  used.add(field);
  if (!f) { problems.push(`--hh-${name}: no HHTheme.${field}`); continue; }
  if (name.startsWith("color-")) {
    const want = parseColour(raw);
    if (!want) { problems.push(`--hh-${name}: can't parse colour ${raw}`); continue; }
    if (f.type !== "Color" || want.some((c, i) => c !== f.value[i]))
      problems.push(`--hh-${name} is ${raw} (${want}) but HHTheme.${field} is ${f.value}`);
  } else {
    const want = parseFloat(raw);
    if (f.type !== "float" || f.value !== want)
      problems.push(`--hh-${name} is ${raw} but HHTheme.${field} is ${f.value}`);
  }
}
for (const field of fields.keys())
  if (!used.has(field)) problems.push(`HHTheme.${field} has no matching token`);

if (problems.length) {
  console.error(problems.join("\n"));
  process.exit(1);
}
console.log(`${guids.size} .meta files ok, ${tokens.length} tokens match HHTheme`);
