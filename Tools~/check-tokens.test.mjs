// Each case is a drift the mirror check once missed or a valid form it once refused.
import { test } from "node:test";
import assert from "node:assert/strict";
import { spawnSync } from "node:child_process";
import { cpSync, mkdtempSync, readFileSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

const repo = join(dirname(fileURLToPath(import.meta.url)), "..");
const script = join(repo, "Tools~", "check-tokens.mjs");
const FILES = ["Brand~/tokens.json", "Brand~/hh-logo.png", "Runtime/Styles/hh-tokens.uss", "Godot~/addons/hh_studio/hh_tokens.gd", "Godot~/addons/hh_studio/hh-logo.png"];

function run(change) {
  const dir = mkdtempSync(join(tmpdir(), "check-tokens-"));
  try {
    for (const file of FILES) {
      cpSync(join(repo, file), join(dir, file), { recursive: true });
    }
    change?.((file, edit) => writeFileSync(join(dir, file), edit(readFileSync(join(dir, file), "utf8"))), dir);
    const result = spawnSync(process.execPath, [script, dir], { encoding: "utf8" });
    return { code: result.status, text: result.stdout + result.stderr };
  } finally {
    rmSync(dir, { recursive: true, force: true });
  }
}
const USS = "Runtime/Styles/hh-tokens.uss";
const GD = "Godot~/addons/hh_studio/hh_tokens.gd";
const JSON_FILE = "Brand~/tokens.json";
const json = (edit) => (text) => JSON.stringify(edit(JSON.parse(text)), null, 2);

test("the committed files pass", () => {
  assert.equal(run().code, 0);
});

const failing = {
  "a USS space token tokens.json lacks": (e) => e(USS, (t) => t.replace("--hh-space-6: 24px;", "--hh-space-6: 24px;\n    --hh-space-8: 32px;")),
  "a USS token in a group nobody mirrors": (e) => e(USS, (t) => t.replace("--hh-space-1: 4px;", "--hh-space-1: 4px;\n    --hh-border-width: 2px;")),
  "a drift hidden behind a commented-out old value": (e) => e(USS, (t) => t.replace("--hh-space-1: 4px;", "/* was --hh-space-1: 4px; */ --hh-space-1: 6px;")),
  "a translucent alpha changed in its third decimal": (e) => e(USS, (t) => t.replace("rgba(130, 87, 255, 0.14)", "rgba(130, 87, 255, 0.141)")),
  "a Godot alpha changed in its fourth decimal": (e) => e(GD, (t) => t.replace("251 / 255.0, 0.62)", "251 / 255.0, 0.6215)")),
  "a Godot colour behind a commented-out old line": (e) => e(GD, (t) => t.replace('const PINK := Color("#FF5C8A")', '# const PINK := Color("#FF5C8A")\nconst PINK := Color("#000000")')),
  "a Godot colour tokens.json lacks": (e) => e(GD, (t) => t + 'const SHADOW := Color("#000000")\n'),
  "a splash key with no Godot constant": (e) => e(JSON_FILE, json((d) => ({ ...d, splash: { ...d.splash, "sting-seconds": 0.5 } }))),
  "a splash constant written as an expression": (e) => e(GD, (t) => t.replace("const SPLASH_HOLD := 1.3", "const SPLASH_HOLD := 1.3 * 2")),
  "a Godot logo that differs from the brand logo": (e, dir) => writeFileSync(join(dir, "Godot~/addons/hh_studio/hh-logo.png"), "not the logo"),
};
for (const [name, change] of Object.entries(failing)) {
  test(`fails on ${name}`, () => {
    assert.equal(run(change).code, 1);
  });
}

test("accepts an eight-digit hex colour and a decimal px value when both sides agree", () => {
  const result = run((e) => {
    e(USS, (t) => t.replace("--hh-color-peach: #FFB25C;", "--hh-color-peach: #FFB25CFF;").replace("--hh-space-1: 4px;", "--hh-space-1: 4.5px;"));
    e(JSON_FILE, json((d) => ({ ...d, space: { ...d.space, 1: 4.5 } })));
  });
  assert.equal(result.code, 0, result.text);
});
