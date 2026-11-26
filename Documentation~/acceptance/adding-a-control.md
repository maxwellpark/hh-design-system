# Acceptance: adding-a-control

Node 22 and Unity 6000.0.85f2's compiler, through the doc snippet gate.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The guide's example control compiles, editor and player, with UI Toolkit's source generators | the compile gate: `Docs.adding_a_control.1` | ok | verified |
| Its list matches what `check.mjs` and the gate enforce today: metas, rules per class constant, known properties and Unity classes, tokens, a test outside the gallery test, the gallery, the reference's exact lists and examples, focus and disabled rules and a disabled gallery instance for focusable controls, the base tables, contrast pairs by convention, and new tokens in every mirror | read against `check.mjs`, `unity-compile.mjs` and `check-tokens.mjs`, and by the backlog review | | verified by reading |
| It names `check.mjs` as the authority, so it can lag without misleading | the opening paragraph | | verified |

## Coverage and complexity

One compiled C# block. The list itself isn't machine-checked against `check.mjs`.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Docs.adding_a_control.1 [editor] (1 files)
ok   Docs.adding_a_control.1 [player] (1 files)
ok   HEAD compiles: 20 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
```

## Unverified

That following the guide alone gets a new control through every check first time.
