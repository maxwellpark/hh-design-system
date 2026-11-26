# Acceptance: unity-class-check

Run in a cloud session with Unity 6000.0.85f2 in `/opt/unity/Editor`.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The gate reads Unity's own class names from its runtime and editor UI Toolkit assemblies: every `unity-*` value a `*ClassName` field gets, resolving fields built from other types' fields | `Tools~/unity-classes.cs`, built and run by the gate | 319 names | verified |
| Every `.unity-*` class the stylesheets style exists | the gate on this branch | 99 uses, all known | verified |
| A real class held in a field not named `*UssClassName` passes | "a real Unity class outside *UssClassName fields passes" (`unity-tab-view__header-container`) | pass | verified |
| A misspelt Unity class fails the gate, naming the class | `unity-compile.test.sh` "a misspelt Unity class in a stylesheet fails" | pass | verified |
| The earlier controls' Unity classes were real all along | the same check over main's stylesheets | pass | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| reader | builds and runs / fails to build / fails to run | clean run / none / none |
| class uses | known / unknown | clean run / the planted case |

## Coverage and complexity

Not measured. The reader is a 45-line loop over IL handling four instructions, repeated until no new field resolves.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (last lines), exit 0:

```
ok   Sample.BasicUI [player] (1 files)
ok   99 .unity-* class uses checked against 275 Unity class names
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
ok   a misspelt Unity class in a stylesheet fails
ok   an uncommitted change is not compiled
ok   a missing editor exits 2
0 failed
```

## Unverified

- Names assigned outside static constructors (none seen in UI Toolkit) would be missed and
  reported as unknown, failing safe.
- Windows: the reader runs through the editor's `dotnet.exe`, untested there.
