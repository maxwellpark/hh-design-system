# Acceptance: settings-newer

Node 22 and Unity 6000.0.85f2's compiler, which builds the sample as a project's own scripts would.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The Settings sample, with a Resolution stepper and a Difficulty radio row, compiles, editor and player | the compile gate: `Sample.Settings [editor]` and `[player]` | ok | verified |
| Both are in the saved settings, so Back offers to discard a change to either, and Discard restores them | `Settings.Resolution`, `Difficulty`; `Current` and `Revert` | | verified by reading |
| The sample check requires both, and fails if either goes | two planted cases | 2 of 2 | verified |
| The README lists them | `Samples~/Settings/README.md` | | verified |

## Coverage and complexity

Sample code, not under test. Difficulty sits on the Player tab rather than Video, where it reads
naturally.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Sample.Settings [editor] (1 files)
ok   Sample.Settings [player] (1 files)
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (settings cases and total):

```
68 .meta files ok, 32 tokens match HHTheme, 120 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
ok   the settings sample dropping a control fails
ok   the settings sample dropping the radio group fails
ok   the settings sample dropping the stepper fails
99/99 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Import Settings and press Play. Video: Resolution reads 1920x1080 between arrows. Player:
   Difficulty is a row of Story, Normal and Hard with Normal filled.
2. Step Resolution right and press Back: the discard dialog. Discard: back to 1920x1080.
3. Pick Hard and Save: a toast; Back now does nothing.

## Unverified

The screen in Play mode.
