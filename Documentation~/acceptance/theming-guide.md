# Acceptance: theming-guide

Node 22 and Unity 6000.0.85f2's compiler, through the doc snippet gate.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The guide's C# (a theme picker over `HHThemes.Next`) compiles, editor and player | the compile gate: `Docs.theming.1` | ok | verified |
| The token names, file paths and theme sheet imports it shows match the package | `Runtime/Styles/hh-tokens.uss`, `hh-theme-high-contrast.tss` | | verified by reading |
| A theme sheet in a game's project can import the package's sheets by their project-absolute path (`/Packages/...`) | Unity's USS `url()` resolution | | unverified |
| A package theme's contrast is checked by `check.mjs`; a game's is by hand | `check.mjs` reads only `Runtime/Styles` | | verified by reading |

## Coverage and complexity

One C# block, compiled; the USS blocks aren't checked by machine, since the gate doesn't import USS.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Docs.theming.1 [editor] (1 files)
ok   Docs.theming.1 [player] (1 files)
ok   HEAD compiles: 18 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
68 .meta files ok, 32 tokens match HHTheme, 124 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
```

## Hands-on checklist (Unity 6, ten minutes)

1. Follow steps 1 and 2 in a project with the package and set a Panel Settings' theme to the new
   sheet: the gallery's cards turn the new colours.
2. Add `ThemePicker` with the panel and two themes, and call `NextTheme`: the panel switches.

## Unverified

The project-path imports and the steps end to end in Unity.
