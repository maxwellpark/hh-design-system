// Fails when Brand~/tokens.json or the Godot copy drifts from hh-tokens.uss, the source of the look.
import { readFileSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

const root = process.argv[2] ?? join(dirname(fileURLToPath(import.meta.url)), "..");
const read = (path) => readFileSync(join(root, path), "utf8");
const tokens = JSON.parse(read("Brand~/tokens.json"));
const errors = [];

// Colours compare as floats: Unity and Godot keep alpha unrounded, so 0.14 and 0.141 differ.
function rgba(value) {
  const hex = String(value).trim().match(/^#([0-9a-f]{6})([0-9a-f]{2})?$/i);
  if (hex) {
    const n = parseInt(hex[1], 16);
    return [n >> 16, (n >> 8) & 255, n & 255, hex[2] ? parseInt(hex[2], 16) / 255 : 1];
  }
  const fn = String(value).trim().match(/^rgba?\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*(?:,\s*([\d.]+)\s*)?\)$/);
  return fn ? [+fn[1], +fn[2], +fn[3], fn[4] === undefined ? 1 : +fn[4]] : null;
}
const same = (a, b) => Boolean(a && b) && a.every((v, i) => Math.abs(v - b[i]) < 1e-6);

const NUMERIC = ["space", "radius", "font-size"];
// Motion keeps the whole token name as its key, durations in seconds and easings as keywords
const MOTION = ["duration-", "easing-"];
const uss = read("Runtime/Styles/hh-tokens.uss").replace(/\/\*[\s\S]*?\*\//g, "");
const ussTokens = { color: {}, motion: {}, ...Object.fromEntries(NUMERIC.map((g) => [g, {}])) };
for (const [, name, raw] of uss.matchAll(/--hh-([\w-]+)\s*:\s*([^;]+);/g)) {
  const value = raw.trim();
  if (MOTION.some((prefix) => name.startsWith(prefix))) {
    const time = value.match(/^(\d+(?:\.\d+)?)(ms|s)$/);
    const parsed = name.startsWith("duration-") ? (time ? Number(time[1]) / (time[2] === "ms" ? 1000 : 1) : null) : /^[a-z-]+$/.test(value) ? value : null;
    if (parsed === null) {
      errors.push(`hh-tokens.uss --hh-${name} is ${value}, not a duration or easing keyword`);
    }
    ussTokens.motion[name] = parsed ?? value;
    continue;
  }
  const group = name.startsWith("color-") ? "color" : NUMERIC.find((g) => name.startsWith(`${g}-`));
  if (!group) {
    errors.push(`hh-tokens.uss --hh-${name} is in no group tokens.json mirrors; add its group to tokens.json and this check`);
    continue;
  }
  const key = name.slice(group.length + 1);
  if (group === "color") {
    ussTokens.color[key] = value;
  } else {
    const px = value.match(/^(\d+(?:\.\d+)?)px$/);
    if (!px) {
      errors.push(`hh-tokens.uss --hh-${name} is ${value}, not a px value`);
    }
    ussTokens[group][key] = px ? Number(px[1]) : value;
  }
}

for (const group of ["color", "motion", ...NUMERIC]) {
  const mirror = tokens[group] ?? {};
  for (const [key, value] of Object.entries(ussTokens[group])) {
    if (!(key in mirror)) {
      errors.push(`tokens.json has no ${group}.${key}, which hh-tokens.uss defines`);
    } else if (group === "color" ? !same(rgba(value), rgba(mirror[key])) : mirror[key] !== value) {
      errors.push(`tokens.json ${group}.${key} is ${mirror[key]}, hh-tokens.uss says ${value}`);
    }
  }
  for (const key of Object.keys(mirror)) {
    if (!(key in ussTokens[group])) {
      errors.push(`tokens.json ${group}.${key} is not in hh-tokens.uss`);
    }
  }
}

// Only whole-line constants count, so a commented-out or trailing-comment line reads as missing.
const gd = Object.fromEntries(
  read("Godot~/addons/hh_studio/hh_tokens.gd").split(/\r?\n/)
    .map((line) => line.match(/^const ([A-Z_]+) := (.+)$/))
    .filter(Boolean)
    .map((m) => [m[1], m[2].trim()]),
);
function godotColour(value) {
  const hex = value.match(/^Color\("(#[0-9A-Fa-f]{6}(?:[0-9A-Fa-f]{2})?)"\)$/);
  if (hex) {
    return rgba(hex[1]);
  }
  const parts = value.match(/^Color\(([^()]*)\)$/)?.[1].split(",").map((p) => p.trim());
  if (parts?.length !== 4) {
    return null;
  }
  const channel = (p) => {
    const over = p.match(/^(\d+(?:\.\d+)?) \/ 255\.0$/);
    return over ? Number(over[1]) : /^\d+(\.\d+)?$/.test(p) ? Number(p) * 255 : NaN;
  };
  return [channel(parts[0]), channel(parts[1]), channel(parts[2]), Number(parts[3])];
}
const constantFor = (key) => key.toUpperCase().replace(/-/g, "_");
for (const [key, value] of Object.entries(tokens.color)) {
  const constant = constantFor(key);
  if (!(constant in gd)) {
    errors.push(`hh_tokens.gd has no ${constant} for color.${key}`);
  } else if (!same(godotColour(gd[constant]), rgba(value))) {
    errors.push(`hh_tokens.gd ${constant} is ${gd[constant]}, tokens.json color.${key} is ${value}`);
  }
}
const colourConstants = new Set(Object.keys(tokens.color).map(constantFor));
for (const [constant, value] of Object.entries(gd)) {
  if (value.startsWith("Color") && !colourConstants.has(constant)) {
    errors.push(`hh_tokens.gd ${constant} is a colour tokens.json does not have`);
  }
}

for (const [key, value] of Object.entries(tokens.motion ?? {})) {
  const constant = constantFor(key);
  if (gd[constant] !== (typeof value === "string" ? JSON.stringify(value) : String(value))) {
    errors.push(`hh_tokens.gd ${constant} is ${gd[constant] ?? "missing"}, tokens.json motion.${key} is ${JSON.stringify(value)}`);
  }
}

const SPLASH = {
  "fade-in-seconds": "SPLASH_FADE_IN",
  "hold-seconds": "SPLASH_HOLD",
  "fade-out-seconds": "SPLASH_FADE_OUT",
  "logo-height-fraction": "SPLASH_LOGO_HEIGHT",
  "host-height-fraction": "SPLASH_HOST_HEIGHT",
  wordmark: "SPLASH_WORDMARK",
};
for (const [key, value] of Object.entries(tokens.splash)) {
  const constant = SPLASH[key];
  if (!constant) {
    errors.push(`tokens.json splash.${key} has no Godot constant; map it in check-tokens.mjs`);
  } else if (gd[constant] !== (typeof value === "string" ? JSON.stringify(value) : String(value))) {
    errors.push(`hh_tokens.gd ${constant} is ${gd[constant] ?? "missing"}, tokens.json splash.${key} is ${JSON.stringify(value)}`);
  }
}

if (!readFileSync(join(root, "Brand~/hh-logo.png")).equals(readFileSync(join(root, "Godot~/addons/hh_studio/hh-logo.png")))) {
  errors.push("Godot~/addons/hh_studio/hh-logo.png differs from Brand~/hh-logo.png");
}

for (const error of errors) {
  console.error(`check-tokens: ${error}`);
}
console.log(errors.length === 0 ? "check-tokens: tokens.json and Godot match hh-tokens.uss" : `check-tokens: ${errors.length} difference(s)`);
process.exitCode = errors.length === 0 ? 0 : 1;
