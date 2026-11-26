#!/usr/bin/env bash
# Compiles each committed assembly with Unity's own compiler, source generators and reference assemblies. Exit 0 clean, 1 failed, 2 setup.
# The editor itself won't open a project in batch mode without a licence, but its compiler needs none.
set -u
# HH_UNITY_EDITOR wins; otherwise the newest 6000.0 under Unity Hub, then the cloud install
hub="$(ls -d "/c/Program Files/Unity/Hub/Editor"/6000.0.*/Editor 2>/dev/null | sort -V | tail -1)"
EDITOR_DIR="${HH_UNITY_EDITOR:-${hub:-/opt/unity/Editor}}"
REV="${1:-HEAD}"
ROOT="$(git rev-parse --show-toplevel 2>/dev/null)" || { echo "FAIL run from inside the package repo"; exit 2; }
WORK="${HH_WORK:-${TMPDIR:-/tmp}/hh-design-system-compile}"
DATA="$EDITOR_DIR/Data"
DOTNET="$DATA/NetCoreRuntime/dotnet"
if [ -e "$DOTNET.exe" ]; then
  DOTNET="$DOTNET.exe"
fi
# Windows tools need Windows paths; elsewhere this is a no-op
win() { cygpath -m "$1" 2>/dev/null || printf '%s' "$1"; }
CSC="$DATA/DotNetSdkRoslyn/csc.dll"
MANAGED="$DATA/Managed/UnityEngine"

for path in "$DOTNET" "$CSC" "$MANAGED/UnityEngine.UIElementsModule.dll"; do
  if [ ! -e "$path" ]; then
    echo "FAIL no $path; set HH_UNITY_EDITOR to a Unity 6000.0 Editor folder"
    exit 2
  fi
done

rm -rf "$WORK"
mkdir -p "$WORK/src" "$WORK/out"
# Only what is committed, as a git install sees it
if ! git -C "$ROOT" archive --format=tar "$REV" | tar -x -C "$WORK/src"; then
  echo "FAIL could not archive $REV"
  exit 2
fi

engine=()
editor=()
for dll in "$MANAGED"/UnityEngine*.dll; do
  engine+=("-r:$(win "$dll")")
done
for dll in "$MANAGED"/UnityEditor*.dll; do
  editor+=("-r:$(win "$dll")")
done
common=(
  -nologo -noconfig -nostdlib -target:library -langversion:9.0 -nullable:disable -deterministic -warnaserror
  "-r:$(win "$DATA/NetStandard/ref/2.1.0/netstandard.dll")"
  "-analyzer:$(win "$DATA/Tools/Unity.SourceGenerators/Unity.UIToolkit.SourceGenerator.dll")"
  "-analyzer:$(win "$DATA/Tools/Unity.SourceGenerators/Unity.SourceGenerators.dll")"
  "-analyzer:$(win "$DATA/Tools/Unity.SourceGenerators/Unity.Properties.SourceGenerator.dll")"
  -define:UNITY_6000_0_OR_NEWER -define:UNITY_2023_1_OR_NEWER -define:UNITY_2022_3_OR_NEWER -define:UNITY_EDITOR
)

status=0
compile() {
  local name="$1" dir="$2"
  shift 2
  local sources=()
  while IFS= read -r -d '' file; do
    sources+=("$(win "$file")")
  done < <(find "$WORK/src/$dir" -name '*.cs' -print0 | sort -z)
  if [ "${#sources[@]}" -eq 0 ]; then
    echo "FAIL $name: no sources in $dir"
    status=1
    return
  fi
  if "$DOTNET" "$(win "$CSC")" "${common[@]}" "$@" "-out:$(win "$WORK/out/$name.dll")" "${sources[@]}" > "$WORK/out/$name.log" 2>&1; then
    echo "ok   $name (${#sources[@]} files)"
  else
    echo "FAIL $name:"
    grep -E 'error|warning' "$WORK/out/$name.log" | sed "s#$WORK/src/##" | sort -u | head -20
    status=1
  fi
}

runtime="$WORK/out/HootonHeroics.DesignSystem.dll"
compile HootonHeroics.DesignSystem Runtime "${engine[@]}"
if [ -f "$runtime" ]; then
  compile HootonHeroics.DesignSystem.Editor Editor "${engine[@]}" "${editor[@]}" "-r:$(win "$runtime")"
  compile HootonHeroics.DesignSystem.Tests.Editor Tests/Editor "${engine[@]}" "${editor[@]}" "-r:$(win "$runtime")" \
    "-r:$(win "$DATA/Resources/PackageManager/BuiltInPackages/com.unity.ext.nunit/net40/unity-custom/nunit.framework.dll")" \
    "-r:$(win "$DATA/NetStandard/compat/2.1.0/shims/netfx/mscorlib.dll")" -define:UNITY_INCLUDE_TESTS
  compile BasicUISample "Samples~/BasicUI" "${engine[@]}" "-r:$(win "$runtime")"
fi

if [ "$status" -eq 0 ]; then
  unity="$(ls "$EDITOR_DIR/Unity.exe" "$EDITOR_DIR/Unity" 2>/dev/null | head -1)"
  echo "ok   $REV compiles with the compiler and generators of Unity $("$unity" -version 2>/dev/null | tail -1)"
fi
exit "$status"
