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
  "an extra Godot colour written with Color8": (e) => e(GD, (t) => t + "const SHADOW := Color8(0, 0, 0)\n"),
  "a mirrored colour rewritten with Color.html": (e) => e(GD, (t) => t.replace('const PINK := Color("#FF5C8A")', 'const PINK := Color.html("#FF5C8A")')),
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

const motionFailures = {
  "a motion token tokens.json lacks": [(e) => e(JSON_FILE, json((t) => ({ ...t, motion: { ...t.motion, "duration-normal": undefined } }))), /tokens\.json has no motion\.duration-normal/],
  "a Godot duration that drifted": [(e) => e(GD, (t) => t.replace("const DURATION_FAST := 0.12", "const DURATION_FAST := 0.15")), /hh_tokens\.gd DURATION_FAST is 0\.15, tokens\.json motion\.duration-fast is 0\.12/],
  "an easing that drifted in tokens.json": [(e) => e(JSON_FILE, json((t) => ({ ...t, motion: { ...t.motion, "easing-standard": "linear" } }))), /tokens\.json motion\.easing-standard is linear, hh-tokens\.uss says ease-out/],
  "an easing that drifted in Godot": [(e) => e(GD, (t) => t.replace('const EASING_STANDARD := "ease-out"', 'const EASING_STANDARD := "linear"')), /hh_tokens\.gd EASING_STANDARD is "linear"/],
  "a duration that isn't a time": [(e) => e(USS, (t) => t.replace("--hh-duration-fast: 0.12s;", "--hh-duration-fast: fast;")), /--hh-duration-fast is fast, not a duration or easing keyword/],
};
for (const [name, [change, message]] of Object.entries(motionFailures)) {
  test(`fails on ${name}`, () => {
    const result = run(change);
    assert.equal(result.code, 1, result.text);
    assert.match(result.text, message);
  });
}

const motionPasses = {
  "a duration in ms": (e) => e(USS, (t) => t.replace("--hh-duration-normal: 0.2s;", "--hh-duration-normal: 200ms;")),
  "a whole-second duration written 1.0 in Godot": (e) => {
    e(USS, (t) => t.replace("--hh-duration-normal: 0.2s;", "--hh-duration-normal: 1s;"));
    e(JSON_FILE, json((t) => ({ ...t, motion: { ...t.motion, "duration-normal": 1 } })));
    e(GD, (t) => t.replace("const DURATION_NORMAL := 0.2", "const DURATION_NORMAL := 1.0"));
  },
};
for (const [name, change] of Object.entries(motionPasses)) {
  test(`passes ${name}`, () => {
    const result = run(change);
    assert.equal(result.code, 0, result.text);
  });
}
