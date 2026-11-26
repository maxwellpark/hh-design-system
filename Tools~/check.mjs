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
const slashed = (path) => path.replaceAll("\\", "/");
const isTracked = (path) => [...tracked].some((file) => file === slashed(path) || file.startsWith(`${slashed(path)}/`));

function walk(path) {
  if (!existsSync(`${path}.meta`)) {
    problems.push(`${path}: no .meta`);
  } else if (tracked && isTracked(path) && !tracked.has(`${slashed(path)}.meta`)) {
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
  fields.set(m[1], { type: "number", value: +m[2] });
}
for (const m of theme.matchAll(/public string (\w+) = "([^"]*)";/g)) {
  fields.set(m[1], { type: "keyword", value: m[2] });
}

// Colours as RGBA, durations in seconds as HHTheme holds them, easings as keywords, the rest in px
function parseToken(name, raw) {
  if (name.startsWith("color-")) {
    return parseColour(raw);
  }
  if (name.startsWith("duration-")) {
    const time = raw.match(/^(\d+(?:\.\d+)?)(ms|s)$/);
    return time ? +time[1] / (time[2] === "ms" ? 1000 : 1) : null;
  }
  if (name.startsWith("easing-")) {
    return /^[a-z-]+$/.test(raw) ? raw : null;
  }
  const px = raw.match(/^(\d+(?:\.\d+)?)px$/);
  return px ? +px[1] : null;
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
  const want = parseToken(name, raw);
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

// WCAG contrast for each pair in contrast.json, translucent colours blended onto what they sit over
const colours = new Map(
  tokens.filter(([name]) => name.startsWith("color-")).map(([name, raw]) => [name.slice(6), parseColour(raw)]),
);
const linear = (c) => (c / 255 <= 0.03928 ? c / 255 / 12.92 : ((c / 255 + 0.055) / 1.055) ** 2.4);
const luminance = ([r, g, b]) => 0.2126 * linear(r) + 0.7152 * linear(g) + 0.0722 * linear(b);
const blend = (top, under) => top.slice(0, 3).map((c, i) => Math.round((c * top[3] + under[i] * (255 - top[3])) / 255));

function contrast(pair) {
  if (!pair.text || !pair.on) {
    return { problem: "needs both text and on" };
  }
  const [text, on, over] = [pair.text, pair.on, pair.over].map((name) => (name ? colours.get(name) : undefined));
  const missing = [pair.text, pair.on, pair.over].find((name, i) => name && ![text, on, over][i]);
  if (missing) {
    return { problem: `no token --hh-color-${missing}` };
  }
  if (over && over[3] < 255) {
    return { problem: `${pair.over} is translucent, so it can't be what ${pair.on} sits over` };
  }
  if (on[3] < 255 && !over) {
    return { problem: `${pair.on} is translucent, say what it sits over` };
  }
  const background = on[3] < 255 ? blend(on, over) : on;
  const [hi, lo] = [luminance(blend(text, background)), luminance(background)].sort((x, y) => y - x);
  return { ratio: (hi + 0.05) / (lo + 0.05) };
}

const accepted = [];
let pairs = 0;
for (const pair of JSON.parse(readFileSync("Tools~/contrast.json", "utf8")).pairs) {
  const { problem, ratio } = contrast(pair);
  if (problem) {
    problems.push(`${pair.use}: ${problem}`);
    continue;
  }
  pairs += 1;
  const min = pair.min ?? 4.5;
  const shown = ratio.toFixed(2);
  const line = `${pair.use}: ${pair.text} on ${pair.on} is ${shown}:1, needs ${min}`;
  // An accepted pair holds its recorded ratio, so it can't quietly get worse or outlive its reason
  if (pair.accepted && (typeof pair.accepted.ratio !== "number" || typeof pair.accepted.why !== "string")) {
    problems.push(`${pair.use}: accepted needs a numeric ratio and a why`);
  } else if (pair.accepted && ratio >= min) {
    problems.push(`${line}: passes now, drop its accepted entry`);
  } else if (pair.accepted && Number(shown) < pair.accepted.ratio) {
    problems.push(`${line}, and fell below its accepted ${pair.accepted.ratio}`);
  } else if (pair.accepted) {
    accepted.push(`accepted: ${line} (${pair.accepted.why})`);
  } else if (ratio < min) {
    problems.push(line);
  }
}

// Every token a stylesheet uses is defined, every control's class has rules, and every control has a test
const stripComments = (text) => text.replace(/\/\*[\s\S]*?\*\//g, "").replace(/\/\/.*$/gm, "");
const styles = readdirSync("Runtime/Styles").filter((name) => /\.(uss|tss)$/.test(name));
const defined = new Set(tokens.map(([name]) => name));
for (const name of styles) {
  const text = stripComments(readFileSync(join("Runtime/Styles", name), "utf8"));
  for (const [, token] of text.matchAll(/var\(\s*--hh-([\w-]+)\s*[,)]/g)) {
    if (!defined.has(token)) {
      problems.push(`${name}: var(--hh-${token}) is not a token`);
    }
  }
}

const componentRules = stripComments(readFileSync("Runtime/Styles/hh-components.uss", "utf8"));
const testSource = readdirSync("Tests/Editor")
  .filter((name) => name.endsWith(".cs"))
  .map((name) => stripComments(readFileSync(join("Tests/Editor", name), "utf8")))
  .join("\n");

// Resolves `A = "x"` and `B = A + "--y"` within one file, the forms the controls use
function classConstants(source) {
  const values = new Map();
  for (const [, constant, expression] of source.matchAll(/(?:const|static readonly) string (\w*UssClassName)\s*=\s*([^;]+);/g)) {
    const parts = expression.split("+").map((part) => part.trim());
    const resolved = parts.map((part) => (part.startsWith('"') ? part.slice(1, -1) : values.get(part)));
    values.set(constant, resolved.includes(undefined) ? undefined : resolved.join(""));
  }
  return values;
}

let controls = 0;
for (const name of readdirSync("Runtime").filter((file) => file.endsWith(".cs"))) {
  const source = stripComments(readFileSync(join("Runtime", name), "utf8"));
  for (const [constant, className] of classConstants(source)) {
    if (className === undefined) {
      problems.push(`${name}: can't resolve ${constant}`);
    } else if (!new RegExp(`\\.${className}(?![\\w-])`).test(componentRules)) {
      problems.push(`${name}: ${constant} "${className}" has no rule in hh-components.uss`);
    }
  }
  const marked = [...source.matchAll(/\[\s*UxmlElement\b/g)].length;
  const declared = [...source.matchAll(/\[\s*UxmlElement\b[^\]]*\][\s\S]*?\bclass\s+(\w+)/g)].map((m) => m[1]);
  if (declared.length !== marked) {
    problems.push(`${name}: ${marked} [UxmlElement] marks but ${declared.length} classes found after them`);
  }
  for (const control of declared) {
    controls += 1;
    if (!new RegExp(`\\b${control}\\b`).test(testSource)) {
      problems.push(`${name}: ${control} has no EditMode test in Tests/Editor`);
    }
  }
}

if (problems.length > 0) {
  console.error(problems.join("\n"));
  process.exit(1);
}
const summary = `${guids.size} .meta files ok, ${tokens.length} tokens match HHTheme, ${pairs} contrast pairs checked, ${controls} controls referenced`;
console.log([...accepted, summary].join("\n"));
