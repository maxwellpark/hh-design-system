#!/usr/bin/env bash
# Runs the compile gate; unity-compile.mjs has the detail.
exec node "$(dirname "$0")/unity-compile.mjs" "$@"
