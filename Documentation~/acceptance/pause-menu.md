# Acceptance: pause-menu

Node 22 and Unity 6000.0.85f2's compiler, which builds each sample as a project's own scripts would,
in the editor and for a player.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The Pause Menu sample compiles against the package, editor and player | the compile gate: `Sample.PauseMenu [editor]` and `[player]` | ok | verified |
| It uses `HHCard`, `HHButton`, `HHDialog` and `HHToastStack`, and the check fails if it stops | the sample check; "the pause menu sample dropping a control fails" | pass | verified |
| The Package Manager lists it | `package.json` `samples` entry; the check fails on a listed path that's missing | 4 samples | verified |
| It reads no input device | `PauseMenu.cs` uses only UI Toolkit buttons | | verified by reading |
| Pause shows the card with focus on Resume; Resume hides it; Settings shows a toast; Quit opens a dialog that Escape and the scrim can't close | `PauseMenu.Pause`, `Resume`, `BuildQuitDialog` (`RequireChoice`) | | unverified |

## Coverage and complexity

Sample code, not under test. Nothing runs per frame. Focus moves to Resume a tick after the card is
shown, since an element that has just been displayed can't take focus until styles resolve.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Sample.PauseMenu [editor] (1 files)
ok   Sample.PauseMenu [player] (1 files)
ok   HEAD compiles: 12 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (sample cases and total):

```
64 .meta files ok, 32 tokens match HHTheme, 100 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   the settings sample dropping a control fails
ok   the HUD sample dropping a control fails
ok   the pause menu sample dropping a control fails
ok   a listed sample that's been deleted fails
80/80 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Import the Pause Menu sample, put `PauseMenu` on a GameObject with a `UIDocument` using
   `hh-theme.tss`, and press Play: a Pause button top left.
2. Press Pause: a "Paused" card in the middle with Resume focused. Press Enter: the card goes.
3. Pause, then Settings: a toast pointing to the Settings sample.
4. Pause, then Quit: a dialog. Escape and clicks outside do nothing; Stay closes it.

## Unverified

The sample's behaviour and layout in Play mode.
