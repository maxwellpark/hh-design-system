# Acceptance: hud-card

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The HUD's health and lives sit in an `HHCard` titled `Player` (a const), and the sample compiles, editor and player | `HudOverlay.BuildTopBar`; the compile gate | ok | verified |
| The sample table requires `HHCard` for the HUD, and fails if it goes | "the HUD sample dropping its card fails" | pass | verified |
| The vitals read against any scene behind them | the card's `surface` fill | | unverified |

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Sample.HUD [editor] (1 files)
ok   Sample.HUD [player] (1 files)
ok   HEAD compiles: 20 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (HUD cases and total):

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
ok   the HUD sample dropping a control fails
ok   the HUD sample dropping its card fails
116/116 passed
```

## Hands-on checklist (Unity 6, one minute)

1. Import the HUD sample and press Play: top left, a card titled Player holding the health bar and
   the three life segments.

## Unverified

The look in Play mode.
