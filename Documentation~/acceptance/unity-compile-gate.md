# Acceptance: unity-compile-gate

Run in a cloud session with the Unity 6000.0.85f2 Linux editor in `/opt/unity/Editor`.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The runtime, editor, test and Basic UI sample assemblies compile with Unity's own compiler and UI Toolkit source generators, warnings as errors | `bash Tools~/unity-compile.sh` | 4 of 4, exit 0 | verified |
| The UI Toolkit generator really runs (so `[UxmlElement]` and `[UxmlAttribute]` are exercised) | `strings` on the built runtime shows `UxmlSerializedData` and `Ghost_UxmlAttributeFlags` | present | verified |
| A syntax error, a non-partial `[UxmlElement]` class, a broken test and a deprecated API each fail the gate | `bash Tools~/unity-compile.test.sh` | 5 planted cases fail as expected | verified |
| Only committed files are compiled, as a git install sees them | the "uncommitted change is not compiled" case | pass | verified |
| `main` at 057e229 (v0.2 plus the contrast, test, form-control, reference and motion work) compiles | the gate on `origin/main` | exit 0 | verified |
| `Tools~/verify.sh` runs the gate | the added line in `verify.sh` | | verified (by reading; `verify.sh` itself stops at its Godot step here) |
| The gate finds Unity under Unity Hub on Windows and converts paths for its tools | `hub=` lookup and `win()` in the script | | unverified (no Windows machine here) |

## Paths

| Code | Path | Evidence |
|---|---|---|
| setup | editor found / missing | clean run / none (exit 2 by reading) |
| `compile` | compiles / error / warning / no sources | clean run, the planted cases / no sources: none |
| runtime gate | runtime fails, so dependants are skipped | the syntax-error case |

## Coverage and complexity

Shell, not measured. `compile` is 20 lines with two branches.

## Behaviour transcript

`bash Tools~/unity-compile.sh HEAD`, exit 0:

```
ok   HootonHeroics.DesignSystem (8 files)
ok   HootonHeroics.DesignSystem.Editor (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor (1 files)
ok   BasicUISample (1 files)
ok   HEAD compiles with the compiler and generators of Unity 6000.0.85f2
```

`bash Tools~/unity-compile.test.sh`, exit 0:

```
ok   clean commit compiles
ok   a syntax error fails
ok   a UxmlElement class that isn't partial fails
ok   a broken test fails
ok   a deprecated API fails as a warning
ok   an uncommitted change is not compiled
0 failed
```

## Hands-on checklist (Windows, two minutes)

1. With Unity 6000.0 installed through Unity Hub, run `bash Tools~/unity-compile.sh` from Git Bash
   in the repo. It prints four `ok` lines and the Unity version.

## Unverified

- USS, TSS and UXML import, and the EditMode tests: both need the editor, which exits 198 without
  a licence (under Blocked on owner).
- The Windows path handling.
