# Acceptance: pause-menu

Node 22 and Unity 6000.0.85f2's compiler, which builds each sample as a project's own scripts would,
in the editor and for a player.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The Pause Menu sample compiles against the package, editor and player | the compile gate: `Sample.PauseMenu [editor]` and `[player]` | ok | verified |
| It uses `HHCard`, `HHButton`, `HHHoldButton` and `HHToastStack`, and the check fails if it stops | the sample check; "the pause menu sample dropping a control fails" (now on `HHHoldButton`) | pass | verified |
| The Package Manager lists it | `package.json` `samples` entry; the check fails on a listed path that's missing | 4 samples | verified |
| It reads no input device | `PauseMenu.cs` uses only UI Toolkit buttons | | verified by reading |
| Pause shows the card with focus on Resume; Resume hides it and focuses Pause; Settings shows a toast; Hold to quit shows the quit toast only after a 1.5-second hold | `PauseMenu.Pause`, `Resume`, the `HHHoldButton`'s `Confirmed` | | unverified |

## Coverage and complexity

Sample code, not under test. Nothing runs per frame. Focus moves to Resume once the card is
laid out (`GeometryChangedEvent`), since an element that has just been displayed can't take focus
until then; it's skipped if the element was hidden again first. Resume focuses Pause the same way.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Sample.PauseMenu [editor] (1 files)
ok   Sample.PauseMenu [player] (1 files)
ok   HEAD compiles: 18 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (sample cases and total):

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
ok   the pause menu sample dropping a control fails
112/112 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Import the Pause Menu sample, put `PauseMenu` on a GameObject with a `UIDocument` using
   `hh-theme.tss`, and press Play: a Pause button top left.
2. Press Pause: a "Paused" card in the middle with Resume focused. Press Enter: the card goes and
   Pause has focus.
3. Pause, then Settings: a toast pointing to the Settings sample.
4. Pause, then press and hold Hold to quit: a bar fills along its foot, and after a second and a
   half a warning toast shows. Let go early: the bar empties and nothing happens.

## Unverified

The sample's behaviour and layout in Play mode, including Enter activating a focused button, which
needs the scene's UI input.
