// Fails when an engine's copy of the brand tokens differs from Brand~/tokens.json. No dependencies.
import { readFileSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

const root = join(dirname(fileURLToPath(import.meta.url)), "..");
const read = (path) => readFileSync(join(root, path), "utf8");
const tokens = JSON.parse(read("Brand~/tokens.json"));
const errors = [];
const hex = (value) => value.toUpperCase();

const uss = read("Runtime/Styles/hh-tokens.uss");
for (const [name, value] of Object.entries(tokens.color)) {
  const found = uss.match(new RegExp(`--hh-color-${name}:\\s*(#[0-9A-Fa-f]{6});`));
  if (!found || hex(found[1]) !== hex(value)) {
    errors.push(`hh-tokens.uss --hh-color-${name} is ${found?.[1] ?? "missing"}, tokens.json says ${value}`);
  }
}
for (const group of ["space", "radius", "font-size"]) {
  for (const [name, value] of Object.entries(tokens[group])) {
    const found = uss.match(new RegExp(`--hh-${group}-${name}:\\s*(\\d+)px;`));
    if (!found || Number(found[1]) !== value) {
      errors.push(`hh-tokens.uss --hh-${group}-${name} is ${found?.[1] ?? "missing"}, tokens.json says ${value}`);
    }
  }
}

const theme = read("Runtime/HHTheme.cs");
const csNames = { bg: "background", surface: "surface", violet: "violet", "violet-light": "violetLight", teal: "teal", pink: "pink", peach: "peach", text: "text" };
for (const [name, field] of Object.entries(csNames)) {
  const found = theme.match(new RegExp(`public Color ${field} = new Color32\\(0x(..), 0x(..), 0x(..), 0xFF\\);`));
  const value = found ? `#${found.slice(1, 4).join("")}` : "missing";
  if (hex(value) !== hex(tokens.color[name])) {
    errors.push(`HHTheme.cs ${field} is ${value}, tokens.json says ${tokens.color[name]}`);
  }
}

const gd = read("Godot~/addons/hh_studio/hh_tokens.gd");
for (const [name, value] of Object.entries(tokens.color)) {
  const constant = name.toUpperCase().replace(/-/g, "_");
  const found = gd.match(new RegExp(`const ${constant} := Color\\("(#[0-9A-Fa-f]{6})"\\)`));
  if (!found || hex(found[1]) !== hex(value)) {
    errors.push(`hh_tokens.gd ${constant} is ${found?.[1] ?? "missing"}, tokens.json says ${value}`);
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
console.log(errors.length === 0 ? "check-tokens: every engine matches tokens.json" : `check-tokens: ${errors.length} difference(s)`);
process.exitCode = errors.length === 0 ? 0 : 1;
