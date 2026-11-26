#!/usr/bin/env bash
# Runs the package checks, the token mirror check and the Godot add-on's tests headless. GODOT names the Godot 4.7 binary if it is not on PATH.
set -euo pipefail
cd "$(dirname "$0")/.."
node "Tools~/check.mjs"
node "Tools~/check-tokens.mjs"
godot="${GODOT:-godot}"
"$godot" --headless --path "Godot~" --import >/dev/null 2>&1
"$godot" --headless --path "Godot~" -s res://tests/run_tests.gd
