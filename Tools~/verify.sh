#!/usr/bin/env bash
# Runs the package checks, the token mirror check and its tests, and the Godot add-on's tests headless.
# GODOT names the Godot 4.7 binary if it is not on PATH as godot.
set -euo pipefail
cd "$(dirname "$0")/.."
node "Tools~/check.mjs"
node --test "Tools~/check.test.mjs" "Tools~/check-tokens.test.mjs" >/dev/null
node "Tools~/check-tokens.mjs"
godot="${GODOT:-godot}"
if ! command -v "$godot" >/dev/null 2>&1; then
  echo "verify: no Godot binary at '$godot'; set GODOT to a Godot 4.7 binary" >&2
  exit 127
fi
log="$(mktemp)"
if ! "$godot" --headless --path "Godot~" --import >"$log" 2>&1; then
  cat "$log" >&2
  rm -f "$log"
  exit 1
fi
rm -f "$log"
"$godot" --headless --path "Godot~" -s res://tests/run_tests.gd
