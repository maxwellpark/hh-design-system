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
  ["accepted pair is reported, not failed", () => {}, 0, /accepted: Button text: text on violet is 4\.00:1, needs 4\.5 \(palette from 0\.1/],
  ["pair naming an unknown token fails", (d) => edit(d, "Tools~/contrast.json", '"text": "focus"', '"text": "focs"'), 1, /Focus ring against a card: no token --hh-color-focs/],
  ["blended ratio is measured", (d) => {
    edit(d, "Runtime/Styles/hh-tokens.uss", "rgba(130, 87, 255, 0.22)", "rgba(130, 87, 255, 0.28)");
    edit(d, "Runtime/HHTheme.cs", "0x82, 0x57, 0xFF, 0x38", "0x82, 0x57, 0xFF, 0x47");
  }, 1, /Ghost button text, hovered: violet-light on violet-ghost-hover is 4\.4\d:1, needs 4\.5/],
  ["3:1 minimum applies to the focus ring", (d) => {
    edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-color-focus: #45D6C4;", "--hh-color-focus: #3A3D70;");
    edit(d, "Runtime/HHTheme.cs", "focus = new Color32(0x45, 0xD6, 0xC4, 0xFF)", "focus = new Color32(0x3A, 0x3D, 0x70, 0xFF)");
  }, 1, /Focus ring against a card: focus on surface is [\d.]+:1, needs 3$/m],
  ["accepted pair getting worse fails", (d) => {
    edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-color-violet-light: #A78BFA;", "--hh-color-violet-light: #E0DAFF;");
    edit(d, "Runtime/HHTheme.cs", "0xA7, 0x8B, 0xFA, 0xFF", "0xE0, 0xDA, 0xFF, 0xFF");
  }, 1, /Button text, hovered: .*fell below its accepted 2\.47/],
  ["accepted pair that now passes fails", (d) => {
    edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-color-violet: #8257FF;", "--hh-color-violet: #6E4AD9;");
    edit(d, "Runtime/HHTheme.cs", "0x82, 0x57, 0xFF, 0xFF", "0x6E, 0x4A, 0xD9, 0xFF");
  }, 1, /Button text: .*passes now, drop its accepted entry/],
  ["accepted without a ratio fails", (d) => edit(d, "Tools~/contrast.json", '"accepted": { "ratio": 2.47, "why": "palette from 0.1, waits on the owner\'s choice in BACKLOG.md" }', '"accepted": "palette from 0.1"'), 1, /Button text, hovered: accepted needs a numeric ratio and a why/],
  ["pair missing a key fails readably", (d) => edit(d, "Tools~/contrast.json", '{ "use": "Body text on the page", "text": "text", "on": "bg" }', '{ "use": "Body text on the page", "text": "text" }'), 1, /Body text on the page: needs both text and on/],
  ["translucent background without over fails", (d) => edit(d, "Tools~/contrast.json", ', "over": "surface" }', " }"), 1, /Ghost button text: violet-ghost is translucent, say what it sits over/],
  ["stylesheet using an undefined token fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "background-color: var(--hh-color-surface);", "background-color: var(--hh-color-surfce);"), 1, /hh-components\.uss: var\(--hh-color-surfce\) is not a token/],
  ["class with no rule fails", (d) => edit(d, "Runtime/HHProgressBar.cs", '"hh-progress-bar"', '"hh-progress-meter"'), 1, /HHProgressBar\.cs: UssClassName "hh-progress-meter" has no rule/],
  ["control with no test fails", (d) => {
    cpSync(join(d, "Runtime/HHToggle.cs"), join(d, "Runtime/HHSwitch.cs"));
    cpSync(join(d, "Runtime/HHToggle.cs.meta"), join(d, "Runtime/HHSwitch.cs.meta"));
    edit(d, "Runtime/HHSwitch.cs.meta", /guid: \w+/.exec(readFileSync(join(d, "Runtime/HHSwitch.cs.meta"), "utf8"))[0], "guid: 0123456789abcdef0123456789abcdef");
    edit(d, "Runtime/HHSwitch.cs", "public partial class HHToggle", "public partial class HHSwitch");
  }, 1, /HHSwitch\.cs: HHSwitch has no EditMode test/],
  ["ghost rule removed fails, though .hh-button remains", (d) => {
    edit(d, "Runtime/Styles/hh-components.uss", ".hh-button.hh-button--ghost:hover:active {", ".hh-button.hh-button-ghostly:hover:active {");
    edit(d, "Runtime/Styles/hh-components.uss", ".hh-button.hh-button--ghost:hover {", ".hh-button.hh-button-ghostly:hover {");
    edit(d, "Runtime/Styles/hh-components.uss", ".hh-button.hh-button--ghost {", ".hh-button.hh-button-ghostly {");
  }, 1, /HHButton\.cs: GhostUssClassName "hh-button--ghost" has no rule/],
  ["undefined token inside a comment is ignored", (d) => edit(d, "Runtime/Styles/hh-components.uss", ".hh-card {", "/* var(--hh-nothing) */\n.hh-card {"), 0, /controls referenced/],
  ["undefined token with a fallback fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "background-color: var(--hh-color-surface);", "background-color: var(--hh-color-surfce, #000);"), 1, /var\(--hh-color-surfce\) is not a token/],
  ["control named only in a test comment fails", (d) => {
    edit(d, "Tests/Editor/ControlTests.cs", "new HHTextField().ClassListContains(HHTextField.UssClassName)", 'new TextField().ClassListContains("hh-text-field")');
    edit(d, "Tests/Editor/ControlTests.cs", 'root.Q<HHTextField>("text-field")', '/* HHTextField */ root.Q<TextField>("text-field")');
  }, 1, /HHTextField\.cs: HHTextField has no EditMode test/],
  ["control declared with extra attributes is still found", (d) => {
    cpSync(join(d, "Runtime/HHToggle.cs"), join(d, "Runtime/HHSwitch.cs"));
    cpSync(join(d, "Runtime/HHToggle.cs.meta"), join(d, "Runtime/HHSwitch.cs.meta"));
    edit(d, "Runtime/HHSwitch.cs.meta", /guid: \w+/.exec(readFileSync(join(d, "Runtime/HHSwitch.cs.meta"), "utf8"))[0], "guid: fedcba9876543210fedcba9876543210");
    edit(d, "Runtime/HHSwitch.cs", "[UxmlElement]\n    public partial class HHToggle", "[UxmlElement(\"switch\")]\n    [System.Serializable]\n    public sealed partial class HHSwitch");
  }, 1, /HHSwitch\.cs: HHSwitch has no EditMode test/],
  ["duration drifts", (d) => edit(d, "Runtime/HHTheme.cs", "durationFast = 0.12f", "durationFast = 0.15f"), 1, /--hh-duration-fast is 0\.12s but HHTheme\.durationFast is 0\.15/],
  ["duration in ms matches seconds", (d) => edit(d, "Runtime/Styles/hh-tokens.uss", "--hh-duration-normal: 0.2s;", "--hh-duration-normal: 200ms;"), 0, /tokens match HHTheme/],
  ["easing drifts", (d) => edit(d, "Runtime/HHTheme.cs", '"ease-out"', '"linear"'), 1, /--hh-easing-standard is ease-out but HHTheme\.easingStandard is linear/],
  ["// inside a stylesheet url() doesn't hide a bad token", (d) => edit(d, "Runtime/Styles/hh-components.uss", ".hh-card {\n", '.hh-card {\n    background-image: url("project://database/x.png"); color: var(--hh-color-nope);\n'), 1, /var\(--hh-color-nope\) is not a token/],
  ["a rule missing its closing brace fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "    border-radius: var(--hh-radius-lg);\n    padding: var(--hh-space-6);\n}\n", "    border-radius: var(--hh-radius-lg);\n    padding: var(--hh-space-6);\n"), 1, /a rule opens inside another rule; a closing brace is missing above/],
  ["a misspelt property fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "    background-color: var(--hh-color-surface);\n    border-radius: var(--hh-radius-lg);", "    background-colour: var(--hh-color-surface);\n    border-radius: var(--hh-radius-lg);"), 1, /unknown property background-colour/],
  ["a declaration without a colon fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "    padding: var(--hh-space-6);\n}", "    padding var(--hh-space-6);\n}"), 1, /"padding var\(--hh-space-6\)" isn't a property: value pair/],
  ["a missing last semicolon fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", "    padding: var(--hh-space-6);\n}", "    padding: var(--hh-space-6)\n}"), 1, /the last declaration has no semicolon/],
  ["a stray closing brace fails", (d) => edit(d, "Runtime/Styles/hh-components.uss", ".hh-card {", "}\n.hh-card {"), 1, /a closing brace with no rule open/],
  ["the theme sheet's imports pass", () => {}, 0, /controls referenced/],
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
