# Acceptance: getting-started

Node 22 and Unity 6000.0.85f2's compiler, through the doc snippet gate.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Both C# blocks in the guide compile against the package, editor and player | the compile gate: `Docs.getting_started [editor]` and `[player]` | ok | verified |
| Its UXML uses only `hh:` elements and attributes the component reference has | `check.mjs` guide UXML check, exit 0 | pass | verified |
| The steps produce the screen described: a dark card titled Northwind, a Music toggle and a violet Play button that logs | | | unverified |
| The menu paths (Panel Settings asset, Package Manager) match Unity 6000.0 | | | unverified |

## Coverage and complexity

Two C# blocks and one UXML block, all held by machine; the prose and menu paths aren't.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Docs.getting_started [editor] (2 files)
ok   Docs.getting_started [player] (2 files)
ok   HEAD compiles: 14 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
```

## Hands-on checklist (Unity 6, ten minutes)

1. In a new Unity 6 project, follow the guide from step 1 to step 4 and press Play: the card,
   toggle and button show as described, and Play logs "Playing with music".
2. Untick Music and press Play: it logs "Playing in silence".
3. Add `MenuOptions`, assign the document and `hh-theme-high-contrast.tss`, and call
   `UseHighContrast` and `SetAccess(true, true)` from a button or the Inspector: the card restyles.

## Unverified

Following the guide in Unity, end to end.
