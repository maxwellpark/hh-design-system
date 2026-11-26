# Acceptance: hud-sample

Node 22 and Unity 6000.0.85f2's compiler, which builds each sample as a project's own scripts would,
in the editor and for a player.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The HUD sample compiles against the package, editor and player | the compile gate: `Sample.HUD [editor]` and `[player]` | ok | verified |
| It uses the progress bar, segmented bar, badge, button and toast stack, and the check fails if it stops | the sample check in `check.mjs`; "the HUD sample dropping a control fails" | pass | verified |
| The Package Manager lists it | `package.json` `samples` entry; the check fails on a listed path that's missing | 3 samples | verified |
| It reads no input device, so it runs under either input system | `HudOverlay.cs` uses only UI Toolkit buttons; no `Input` or `InputSystem` reference | | verified by reading |
| Take a hit drains health and costs a life at zero; Heal restores; Pick up counts on the badge until the bag opens | `HudOverlay.TakeHit`, `Heal`, `PickUp`, `OpenBag` | | unverified |

## Coverage and complexity

Sample code, not under test. No method is over 25 lines, and nothing runs per frame.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (sample and summary lines):

```
ok   Sample.HUD [editor] (1 files)
ok   Sample.HUD [player] (1 files)
ok   HEAD compiles: 10 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (sample cases and total):

```
58 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced, 16 types in the component reference
ok   the HUD sample dropping a control fails
ok   a listed sample that's been deleted fails
74/74 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Import the HUD sample, put `HudOverlay` on a GameObject with a `UIDocument` using
   `hh-theme.tss`, and press Play: a full pink health bar and three peach life segments top left,
   a Bag button top right, and three buttons along the bottom.
2. Take a hit twice: health drops to 65, then 30. A third hit costs a life: health refills, a
   "Life lost" toast shows and a segment goes dark.
3. Pick up twice: two toasts and a badge reading 2 on Bag. Press Bag: the badge goes.
4. Take six more hits: the second life goes at the sixth hit of the run, leaving one segment; the
   third life goes at the ninth, with a red "Game over" toast and all three segments back at once.

## Unverified

The sample's behaviour and layout in Play mode.
