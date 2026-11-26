#!/usr/bin/env bash
# Runs the compile gate on a clean commit and on commits broken one way each; each must exit as expected.
set -u
ROOT="$(git rev-parse --show-toplevel)"
GATE="$(cd "$(dirname "$0")" && pwd)/unity-compile.sh"
SCRATCH="$(mktemp -d)"
trap 'rm -rf "$SCRATCH"' EXIT
failed=0

check() {
  local name="$1" want="$2" pattern="$3"
  shift 3
  local repo="$SCRATCH/$RANDOM$RANDOM"
  git clone -q "$ROOT" "$repo"
  ( cd "$repo" && "$@" ) >/dev/null 2>&1
  git -C "$repo" -c user.name=t -c user.email=t@t commit -qam "$name" --allow-empty
  local out code
  out="$(cd "$repo" && HH_WORK="$repo.work" bash "$GATE" HEAD 2>&1)"
  code=$?
  if [ "$code" -eq "$want" ] && printf '%s' "$out" | grep -qE "$pattern"; then
    echo "ok   $name"
  else
    echo "FAIL $name: exit $code"
    printf '%s\n' "$out" | head -5
    failed=$((failed + 1))
  fi
}

check "clean commit compiles" 0 "compiles with the compiler" true
check "a syntax error fails" 1 "FAIL HootonHeroics.DesignSystem:.*|error CS" \
  sed -i 's/AddToClassList(UssClassName);/AddToClassList(UssClassName)/' Runtime/HHProgressBar.cs
check "a UxmlElement class that isn't partial fails" 1 "error" \
  sed -i 's/public partial class HHProgressBar/public class HHProgressBar/' Runtime/HHProgressBar.cs
check "a broken test fails" 1 "FAIL HootonHeroics.DesignSystem.Tests.Editor" \
  sed -i 's/new HHProgressBar()/new HHProgressBarr()/' Tests/Editor/ControlTests.cs
check "a deprecated API fails as a warning" 1 "warning CS0618|error CS0618" \
  sed -i 's/public HHProgressBar()/[System.Obsolete("x")] public void Old() { } public void UsesOld() { Old(); }\n        public HHProgressBar()/' Runtime/HHProgressBar.cs
check "an uncommitted change is not compiled" 0 "compiles with the compiler" \
  bash -c 'echo "syntax error" > Runtime/Untracked.cs'

echo "$failed failed"
exit $((failed > 0))
