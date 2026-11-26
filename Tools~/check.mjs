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

// WCAG contrast for each pair in contrast.json, in every theme; translucent colours blend onto what they sit over
const readColours = (list) => new Map(list.filter(([name]) => name.startsWith("color-")).map(([name, raw]) => [name.slice(6), parseColour(raw)]));
const linear = (c) => (c / 255 <= 0.03928 ? c / 255 / 12.92 : ((c / 255 + 0.055) / 1.055) ** 2.4);
const luminance = ([r, g, b]) => 0.2126 * linear(r) + 0.7152 * linear(g) + 0.0722 * linear(b);
const blend = (top, under) => top.slice(0, 3).map((c, i) => Math.round((c * top[3] + under[i] * (255 - top[3])) / 255));

function contrast(pair, colours) {
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

// The default theme may hold accepted exceptions; every other theme defines the same tokens and passes outright
const themeFiles = readdirSync("Runtime/Styles").filter((f) => /^hh-tokens-[\w-]+\.uss$/.test(f)).sort();
const themes = [{ name: "default", tokens }];
for (const file of themeFiles) {
  const text = readFileSync(join("Runtime/Styles", file), "utf8").replace(/\/\*[\s\S]*?\*\//g, "");
  const list = [...text.matchAll(/--hh-([\w-]+)\s*:\s*([^;]+);/g)].map(([, name, value]) => [name, value.trim()]);
  const names = new Set(list.map(([name]) => name));
  if (names.size !== list.length) {
    problems.push(`${file}: a token is defined twice`);
  }
  const sheet = `hh-theme-${file.slice("hh-tokens-".length, -".uss".length)}.tss`;
  if (!existsSync(join("Runtime/Styles", sheet)) || !readFileSync(join("Runtime/Styles", sheet), "utf8").includes(`url("${file}")`)) {
    problems.push(`${file}: no ${sheet} importing it`);
  }
  for (const [name] of tokens) {
    if (!names.has(name)) {
      problems.push(`${file}: no --hh-${name}, which hh-tokens.uss defines`);
    }
  }
  for (const name of names) {
    if (!tokens.some(([n]) => n === name)) {
      problems.push(`${file}: --hh-${name} isn't a token in hh-tokens.uss`);
    }
  }
  for (const [name, raw] of list) {
    if (parseToken(name, raw) === null) {
      problems.push(`${file}: --hh-${name}: can't parse ${raw}`);
    }
  }
  themes.push({ name: file.slice("hh-tokens-".length, -".uss".length), tokens: list });
}

const accepted = [];
let pairs = 0;
const pairList = JSON.parse(readFileSync("Tools~/contrast.json", "utf8")).pairs;
for (const theme of themes) {
  const colours = readColours(theme.tokens);
  const isDefault = theme.name === "default";
  const where = isDefault ? "" : ` [${theme.name}]`;
  for (const pair of pairList) {
    const { problem, ratio } = contrast(pair, colours);
    if (problem) {
      problems.push(`${pair.use}${where}: ${problem}`);
      continue;
    }
    pairs += 1;
    const min = pair.min ?? 4.5;
    const shown = ratio.toFixed(2);
    const line = `${pair.use}${where}: ${pair.text} on ${pair.on} is ${shown}:1, needs ${min}`;
    // An accepted pair holds its recorded ratio, so it can't quietly get worse or outlive its reason
    if (isDefault && pair.accepted && (typeof pair.accepted.ratio !== "number" || typeof pair.accepted.why !== "string")) {
      problems.push(`${pair.use}: accepted needs a numeric ratio and a why`);
    } else if (isDefault && pair.accepted && ratio >= min) {
      problems.push(`${line}: passes now, drop its accepted entry`);
    } else if (isDefault && pair.accepted && Number(shown) < pair.accepted.ratio) {
      problems.push(`${line}, and fell below its accepted ${pair.accepted.ratio}`);
    } else if (isDefault && pair.accepted) {
      accepted.push(`accepted: ${line} (${pair.accepted.why})`);
    } else if (ratio < min) {
      problems.push(line);
    }
  }
}

// Every token a stylesheet uses is defined, every control's class has rules, and every control has a test
// USS has only block comments, where `//` can sit inside a url(); C# also has line comments
const stripComments = (text, lineComments = false) => {
  const blockless = text.replace(/\/\*[\s\S]*?\*\//g, "");
  return lineComments ? blockless.replace(/^\s*\/\/.*$/gm, "").replace(/\s\/\/[^"\n]*$/gm, "") : blockless;
};
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
// The gallery's test names every control by design, so it can't stand in for a control's own test
const testSource = readdirSync("Tests/Editor")
  .filter((name) => name.endsWith(".cs") && name !== "GalleryTests.cs")
  .map((name) => stripComments(readFileSync(join("Tests/Editor", name), "utf8"), true))
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

// Every sample package.json lists must exist, and the settings, HUD and pause menu samples must use the controls those screens need
for (const sample of JSON.parse(readFileSync("package.json", "utf8")).samples ?? []) {
  if (!existsSync(sample.path)) {
    problems.push(`package.json: sample "${sample.displayName}" path ${sample.path} is missing`);
  }
}
const sampleControls = {
  "Samples~/Settings/SettingsMenu.cs": ["HHTabs", "HHToggle", "HHSlider", "HHDropdown", "HHTextField", "HHDialog", "HHToastStack", "HHAccessibility"],
  "Samples~/HUD/HudOverlay.cs": ["HHProgressBar", "HHSegmentedBar", "HHBadge", "HHButton", "HHToastStack"],
  "Samples~/PauseMenu/PauseMenu.cs": ["HHCard", "HHButton", "HHDialog", "HHToastStack"],
};
for (const [path, needed] of Object.entries(sampleControls)) {
  const source = existsSync(path) ? stripComments(readFileSync(path, "utf8"), true) : null;
  for (const control of needed) {
    if (source !== null && !new RegExp(`\\b${control}\\b`).test(source)) {
      problems.push(`${path}: doesn't use ${control}`);
    }
  }
}

// The gallery must show every control
const gallery = existsSync("Editor/HHGallery.cs") ? stripComments(readFileSync("Editor/HHGallery.cs", "utf8"), true) : null;
let controls = 0;
for (const name of readdirSync("Runtime").filter((file) => file.endsWith(".cs"))) {
  const source = stripComments(readFileSync(join("Runtime", name), "utf8"), true);
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
    if (gallery !== null && !new RegExp(`\\b${control}\\b`).test(gallery)) {
      problems.push(`${name}: ${control} isn't in the gallery, Editor/HHGallery.cs`);
    }
  }
}

// Accessibility modifiers override every token of their kind, and only tokens: motion to 0s, type to 1.25x rounded
const modifiers = { "hh-reduced-motion": ["duration-", () => "0s"], "hh-large-text": ["font-size-", (raw) => `${Math.round(parseFloat(raw) * 1.25)}px`] };
for (const [className, [kind, expect]] of Object.entries(modifiers)) {
  const bodies = [...componentRules.matchAll(new RegExp(`(?:^|\\})\\s*\\.${className}\\s*\\{([^}]*)\\}`, "g"))].map((m) => m[1]);
  if (bodies.length !== 1) {
    problems.push(`hh-components.uss: ${bodies.length} standalone .${className} rules, needs exactly 1`);
    continue;
  }
  const body = bodies[0];
  const set = new Map([...body.matchAll(/--hh-([\w-]+)\s*:\s*([^;]+);/g)].map(([, name, value]) => [name, value.trim()]));
  for (const [name, raw] of tokens.filter(([n]) => n.startsWith(kind))) {
    if (!set.has(name)) {
      problems.push(`hh-components.uss: .${className} doesn't set --hh-${name}`);
    } else if (set.get(name) !== expect(raw)) {
      problems.push(`hh-components.uss: .${className} sets --hh-${name} to ${set.get(name)}, expected ${expect(raw)}`);
    }
  }
  for (const name of set.keys()) {
    if (!defined.has(name)) {
      problems.push(`hh-components.uss: .${className} sets --hh-${name}, which isn't a token`);
    } else if (!name.startsWith(kind)) {
      problems.push(`hh-components.uss: .${className} sets --hh-${name}, outside its kind (${kind}*)`);
    }
  }
}

// The component reference has a section per class-bearing type, naming its classes, UXML attributes and every token its rules use
const referencePath = "Documentation~/components.md";
const documented = new Set();
if (!existsSync(referencePath)) {
  problems.push(`${referencePath} is missing`);
} else {
  const sections = new Map(readFileSync(referencePath, "utf8").split(/^## /m).slice(1).map((s) => [s.split(/\s/)[0], s]));
  const rules = [...componentRules.matchAll(/([^{}]+)\{([^}]*)\}/g)].map((m) => ({ selectors: m[1].split(","), body: m[2] }));
  for (const name of readdirSync("Runtime").filter((file) => file.endsWith(".cs"))) {
    const source = stripComments(readFileSync(join("Runtime", name), "utf8"), true);
    const classes = [...classConstants(source).values()].filter((value) => value !== undefined);
    if (classes.length === 0) {
      continue;
    }
    const type = name.slice(0, -3);
    const section = sections.get(type);
    documented.add(type);
    if (section === undefined) {
      problems.push(`${name}: ${type} has no section in ${referencePath}`);
      continue;
    }
    // Only the three lists count, so a mention in prose or an example can't stand in for an entry
    const listed = (label) => [...(section.match(new RegExp(`^- \\*\\*${label}:\\*\\*(.*)$`, "m"))?.[1] ?? "").matchAll(/`([^`]+)`/g)].map((m) => m[1]);
    const attributeBlock = section.match(/^- \*\*Attributes:\*\*\n((?: {2}- .*\n?)*)/m)?.[1] ?? "";
    const listedAttributes = [...attributeBlock.matchAll(/^ {2}- `([^`]+)`/gm)].map((m) => m[1]);
    const attributes = [...source.matchAll(/\[\s*UxmlAttribute\b\s*(?:\(\s*"([^"]+)"\s*\))?\s*\]\s*(?:\[[^\]]*\]\s*)*public\s+[\w.<>?\[\],]+\s+(\w+)/g)].map(
      ([, explicit, property]) => explicit ?? property.replace(/([a-z0-9])([A-Z])/g, "$1-$2").toLowerCase(),
    );
    const owns = (selector) => classes.some((className) => new RegExp(`\\.${className}(?![\\w-])`).test(selector));
    const used = new Set();
    for (const rule of rules.filter((r) => r.selectors.some(owns))) {
      for (const [, token] of rule.body.matchAll(/var\(\s*--hh-([\w-]+)/g)) {
        used.add(`--hh-${token}`);
      }
    }
    const compare = (what, expected, found) => {
      for (const item of expected.filter((x) => !found.includes(x))) {
        problems.push(`${referencePath}: ${type}'s ${what} list is missing ${item}`);
      }
      for (const item of found.filter((x) => !expected.includes(x))) {
        problems.push(`${referencePath}: ${type}'s ${what} list names ${item}, which it doesn't have`);
      }
    };
    compare("Classes", classes.map((c) => `.${c}`), listed("Classes"));
    compare("Attributes", attributes, listedAttributes);
    compare("Tokens", [...used], listed("Tokens"));
  }
  for (const type of sections.keys()) {
    if (!documented.has(type)) {
      problems.push(`${referencePath}: section ${type} has no matching control`);
    }
  }
}

// UXML in the guides may only use documented hh: elements, with attributes from their reference section (its Attributes
// list, or one its own example already uses on that element) or VisualElement's own
if (existsSync(referencePath)) {
  const common = new Set(["name", "class", "style", "tooltip", "picking-mode", "focusable", "tabindex", "view-data-key", "usage-hints", "enabled", "data-source", "data-source-path", "data-source-type"]);
  const xmlBlocks = (text) => [...text.matchAll(/^```xml\r?\n([\s\S]*?)^```/gm)].map((m) => ({ code: m[1], line: text.slice(0, m.index).split("\n").length + 1 }));
  const elements = (code) => [...code.matchAll(/<hh:(\w+)\b([^>]*)>/g)].map((m) => ({ type: m[1], attributes: [...m[2].matchAll(/([\w-]+)\s*=/g)].map((a) => a[1]), at: code.slice(0, m.index).split("\n").length - 1 }));
  const allowed = new Map();
  for (const section of readFileSync(referencePath, "utf8").split(/^## /m).slice(1)) {
    const type = section.split(/\s/)[0];
    const names = new Set([...(section.match(/^- \*\*Attributes:\*\*\n((?: {2}- .*\n?)*)/m)?.[1] ?? "").matchAll(/^ {2}- `([^`]+)`/gm)].map((m) => m[1]));
    for (const { code } of xmlBlocks(section)) {
      for (const element of elements(code).filter((e) => e.type === type)) {
        element.attributes.forEach((a) => names.add(a));
      }
    }
    allowed.set(type, names);
  }
  for (const doc of readdirSync("Documentation~").filter((n) => n.endsWith(".md") && n !== "components.md")) {
    for (const { code, line } of xmlBlocks(readFileSync(join("Documentation~", doc), "utf8"))) {
      for (const { type, attributes, at } of elements(code)) {
        const where = `Documentation~/${doc}:${line + at}`;
        if (!allowed.has(type)) {
          problems.push(`${where}: hh:${type} isn't in the component reference`);
          continue;
        }
        for (const attribute of attributes.filter((a) => !common.has(a) && !allowed.get(type).has(a))) {
          problems.push(`${where}: hh:${type} has no ${attribute} attribute in the component reference`);
        }
      }
    }
  }
}

// Each stylesheet parses as flat rules: selectors, then one brace block of known `property: value;` lines
const knownProperties = new Set(readFileSync("Tools~/uss-properties.txt", "utf8").split(/\r?\n/).filter((l) => l && !l.startsWith("#")));

// Splits on a character only outside quotes and parentheses, keeping each piece's starting line
function splitTop(text, startLine, separator) {
  const pieces = [];
  let quote = null;
  let parens = 0;
  let line = startLine;
  let piece = "";
  let pieceLine = startLine;
  for (const ch of text) {
    if (!piece.trim() && ch.trim()) {
      pieceLine = line;
    }
    if (quote) {
      quote = ch === quote ? null : quote;
    } else if (ch === '"' || ch === "'") {
      quote = ch;
    } else if (ch === "(") {
      parens += 1;
    } else if (ch === ")") {
      parens -= 1;
    } else if (ch === separator && parens === 0) {
      pieces.push({ text: piece, line: pieceLine, closed: true });
      piece = "";
      continue;
    }
    piece += ch;
    if (ch === "\n") {
      line += 1;
    }
  }
  pieces.push({ text: piece, line: pieceLine, closed: false });
  return pieces;
}

function checkStylesheet(name, source) {
  const text = stripComments(source);
  let depth = 0;
  let line = 1;
  let chunk = "";
  let chunkLine = 1;
  let quote = null;
  let parens = 0;
  for (const ch of text) {
    if (quote || parens > 0 || (ch !== "{" && ch !== "}")) {
      if (quote) {
        quote = ch === quote ? null : quote;
      } else if (ch === '"' || ch === "'") {
        quote = ch;
      } else if (ch === "(") {
        parens += 1;
      } else if (ch === ")") {
        parens -= 1;
      }
      chunk += ch;
      if (ch === "\n") {
        line += 1;
      }
      continue;
    }
    if (ch === "{") {
      if (depth > 0) {
        problems.push(`${name}:${line}: a rule opens inside another rule; a closing brace is missing above`);
      } else {
        // Before a selector only whole `@import url(...);` statements may sit
        const parts = splitTop(chunk, chunkLine, ";");
        const selector = parts.at(-1).text.trim();
        for (const part of parts.slice(0, -1)) {
          const piece = part.text.trim();
          if (!piece) {
            problems.push(`${name}:${part.line}: a stray ; before a rule`);
          } else if (!/^@import\s+url\([^)]*\)$/.test(piece)) {
            problems.push(`${name}:${part.line}: "${piece.replace(/\s+/g, " ")}" before a rule isn't an @import`);
          }
        }
        if (selector.startsWith("@import")) {
          problems.push(`${name}:${line}: an @import runs into a rule; it needs a semicolon`);
        } else if (!selector) {
          problems.push(`${name}:${line}: a rule has no selector`);
        }
      }
      depth += 1;
      chunk = "";
      chunkLine = line;
    } else {
      if (depth === 0) {
        problems.push(`${name}:${line}: a closing brace with no rule open`);
      } else {
        const declarations = splitTop(chunk, chunkLine, ";");
        for (const { text: declaration, line: at, closed } of declarations) {
          const trimmed = declaration.trim();
          if (!trimmed) {
            continue;
          }
          const match = trimmed.match(/^(-{0,2}[a-z][a-z0-9-]*)\s*:\s*(.+)$/s);
          if (!match) {
            problems.push(`${name}:${at}: "${trimmed.replace(/\s+/g, " ")}" isn't a property: value pair`);
          } else if (!match[1].startsWith("--") && !knownProperties.has(match[1])) {
            problems.push(`${name}:${at}: unknown property ${match[1]}`);
          } else if (!closed) {
            problems.push(`${name}:${at}: the last declaration has no semicolon`);
          }
        }
      }
      depth = Math.max(0, depth - 1);
      chunk = "";
      chunkLine = line;
    }
  }
  if (depth !== 0) {
    problems.push(`${name}: ${depth} rule(s) never closed`);
  }
  for (const part of splitTop(chunk, chunkLine, ";")) {
    const trimmed = part.text.trim();
    if (trimmed && !(part.closed && /^@import\s+url\([^)]*\)$/.test(trimmed))) {
      problems.push(`${name}:${part.line}: "${trimmed.slice(0, 40)}" after the last rule isn't an @import`);
    }
  }
}

for (const name of styles) {
  checkStylesheet(name, readFileSync(join("Runtime/Styles", name), "utf8"));
}

if (problems.length > 0) {
  console.error(problems.join("\n"));
  process.exit(1);
}
const summary = `${guids.size} .meta files ok, ${tokens.length} tokens match HHTheme, ${pairs} contrast pairs checked across ${themes.length} theme(s), ${controls} controls referenced, ${documented.size} types in the component reference`;
console.log([...accepted, summary].join("\n"));
