// Fails when Brand~/tokens.json or the Godot constants drift from hh-tokens.uss, the source of the look.
import { readFileSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

const root = join(dirname(fileURLToPath(import.meta.url)), "..");
const read = (path) => readFileSync(join(root, path), "utf8");
const tokens = JSON.parse(read("Brand~/tokens.json"));
const errors = [];

function rgba(value) {
  const hex = value.match(/^#([0-9a-f]{6})$/i);
  if (hex) {
    const n = parseInt(hex[1], 16);
    return [n >> 16, (n >> 8) & 255, n & 255, 255];
  }
  const fn = value.match(/^rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*([\d.]+)\s*)?\)$/);
  return fn ? [+fn[1], +fn[2], +fn[3], fn[4] === undefined ? 255 : Math.round(+fn[4] * 255)] : null;
}
const same = (a, b) => a && b && a.every((v, i) => v === b[i]);

const uss = read("Runtime/Styles/hh-tokens.uss");
const ussColours = Object.fromEntries([...uss.matchAll(/--hh-color-([\w-]+):\s*([^;]+);/g)].map((m) => [m[1], m[2].trim()]));
for (const [name, value] of Object.entries(ussColours)) {
  if (!(name in tokens.color)) {
    errors.push(`tokens.json has no color.${name}, which hh-tokens.uss defines`);
  } else if (!same(rgba(value), rgba(tokens.color[name]))) {
    errors.push(`tokens.json color.${name} is ${tokens.color[name]}, hh-tokens.uss says ${value}`);
  }
}
for (const name of Object.keys(tokens.color)) {
  if (!(name in ussColours)) {
    errors.push(`tokens.json color.${name} is not in hh-tokens.uss`);
  }
}
for (const group of ["space", "radius", "font-size"]) {
  for (const [name, value] of Object.entries(tokens[group])) {
    const found = uss.match(new RegExp(`--hh-${group}-${name}:\\s*(\\d+)px;`));
    if (!found || Number(found[1]) !== value) {
      errors.push(`tokens.json ${group}.${name} is ${value}, hh-tokens.uss says ${found?.[1] ?? "nothing"}`);
    }
  }
}

const gd = read("Godot~/addons/hh_studio/hh_tokens.gd");
function godotColour(constant) {
  const hex = gd.match(new RegExp(`const ${constant} := Color\\("(#[0-9A-Fa-f]{6})"\\)`));
  if (hex) {
    return rgba(hex[1]);
  }
  const parts = gd.match(new RegExp(`const ${constant} := Color\\(([^)]*)\\)`))?.[1].split(",").map((p) => p.trim());
  if (parts?.length !== 4) {
    return null;
  }
  const channel = (p) => Math.round(p.includes("/") ? Number(p.split("/")[0]) : Number(p) * 255);
  return [channel(parts[0]), channel(parts[1]), channel(parts[2]), Math.round(Number(parts[3]) * 255)];
}
for (const [name, value] of Object.entries(tokens.color)) {
  const constant = name.toUpperCase().replace(/-/g, "_");
  if (!same(godotColour(constant), rgba(value))) {
    errors.push(`hh_tokens.gd ${constant} differs from tokens.json color.${name} ${value}`);
  }
}
const splash = { "fade-in-seconds": "SPLASH_FADE_IN", "hold-seconds": "SPLASH_HOLD", "fade-out-seconds": "SPLASH_FADE_OUT", "logo-height-fraction": "SPLASH_LOGO_HEIGHT", "host-height-fraction": "SPLASH_HOST_HEIGHT" };
for (const [name, constant] of Object.entries(splash)) {
  const found = gd.match(new RegExp(`const ${constant} := ([0-9.]+)`));
  if (!found || Number(found[1]) !== tokens.splash[name]) {
    errors.push(`hh_tokens.gd ${constant} is ${found?.[1] ?? "missing"}, tokens.json says ${tokens.splash[name]}`);
  }
}
if (!gd.includes(`const SPLASH_WORDMARK := "${tokens.splash.wordmark}"`)) {
  errors.push(`hh_tokens.gd SPLASH_WORDMARK differs from tokens.json "${tokens.splash.wordmark}"`);
}

for (const error of errors) {
  console.error(`check-tokens: ${error}`);
}
console.log(errors.length === 0 ? "check-tokens: tokens.json and Godot match hh-tokens.uss" : `check-tokens: ${errors.length} difference(s)`);
process.exitCode = errors.length === 0 ? 0 : 1;
