# Acceptance: unity-compile-gate

Run in a cloud session with the Unity 6000.0.85f2 Linux editor in `/opt/unity/Editor`.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every committed asmdef compiles with Unity's own compiler and UI Toolkit source generators, warnings as errors, following its references, platforms, define constraints and version defines | `bash Tools~/unity-compile.sh` | 6 builds, exit 0 | verified |
| Runtime assemblies and samples are also built for a player (no `UNITY_EDITOR`, no editor references) | `[player]` lines in the transcript; "editor-only code in a runtime assembly fails the player build" | pass | verified |
| Test assemblies compile against Unity's real test runner, built from the editor's copy | `UnityEngine.TestRunner` and `UnityEditor.TestRunner` compile before the tests | pass | verified |
| The UI Toolkit generator runs, so `[UxmlElement]` mistakes fail | "a UxmlElement class that isn't partial fails" with `UITKSG003` | pass | verified |
| A syntax error, a broken test, a deprecated API, an unknown asmdef reference and a lost runtime reference each fail | `bash Tools~/unity-compile.test.sh` | 10 of 10 cases | verified |
| Only committed files are compiled; a missing editor exits 2 | the last two cases | pass | verified |
| `main` at 057e229 compiles | the gate on `origin/main` | exit 0 | verified |
| `Tools~/verify.sh` runs the gate | the added line | | verified by reading; `verify.sh` stops at its Godot step here |
| The gate finds Unity under Unity Hub on Windows | `findEditor` | | unverified (no Windows machine) |

## Paths

| Code | Path | Evidence |
|---|---|---|
| setup | editor found / missing / not a repo / bad revision | clean run / missing-editor case / none / none (exit 2 by reading) |
| assembly order | references built first / unknown reference / constraint unmet (skipped) | clean run / unknown-reference case / the test assembly in the player build |
| compile | ok / error / warning / no sources | clean run, the planted cases / none |

## Coverage and complexity

Not measured. `compileAll` is about 75 lines with eight branches; it stays one function because
each branch reports on the same assembly. A backlog idea could split resolution from compiling.

## Behaviour transcript

`bash Tools~/unity-compile.sh HEAD`, exit 0:

```
ok   HootonHeroics.DesignSystem [editor] (8 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (1 files)
ok   Sample.BasicUI [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (8 files)
ok   Sample.BasicUI [player] (1 files)
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`bash Tools~/unity-compile.test.sh`, exit 0:

```
ok   clean commit compiles
ok   a syntax error fails
ok   a UxmlElement class that isn't partial fails
ok   a broken test fails
ok   a deprecated API fails as a warning
ok   an unknown asmdef reference fails
ok   tests losing their runtime reference fail
ok   editor-only code in a runtime assembly fails the player build
ok   an uncommitted change is not compiled
ok   a missing editor exits 2
0 failed
```

## Hands-on checklist (Windows, two minutes)

1. With Unity 6000.0 from Unity Hub, run `bash Tools~/unity-compile.sh` in Git Bash from the repo:
   six `ok` lines and the version.

## Unverified

- USS, TSS and UXML import, and running the EditMode tests: both need the editor, which exits 198
  without a licence (under Blocked on owner).
- Windows.
