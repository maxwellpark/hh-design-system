# Acceptance: dialog-focus

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHDialog` and its tests compile, editor and player | the compile gate | ok | verified |
| The dialog's tab stops follow Unity's focus ring: elements that can grab focus with a tab index of 0 or more, in order, including a focusable container's children, with a delegating field counted once | `DialogTests.FocusStopsFollowTheRingAndCountADelegatingFieldOnce` | | written, not run |
| While open, focus leaving the last stop goes to the first, leaving the first goes to the last, and cleared focus comes back; moving inside is left alone; a closed dialog never pulls | `DialogTests.FocusLeavingAnOpenDialogWrapsBackInside` | | written, not run |
| Of two open dialogs, the one holding focus keeps it and the other doesn't pull, until it closes | `DialogTests.TwoOpenDialogsDoNotFightOverFocus` | | written, not run |
| A dialog with no stops doesn't trap | `DialogTests.ADialogWithNoStopsLeavesFocusAlone` | | written, not run |
| The refocus is decided on the next tick from where focus landed, and skips a stop that can no longer grab focus, so it can't clear focus | `HHDialog.OnFocusOut` | | verified by reading |
| In a running panel, Tab and Shift+Tab wrap and focus moved outside comes back | the hands-on checklist | | unverified |

## Coverage and complexity

`RefocusTarget` and `FocusStops` carry the logic and have four tests; `OnFocusOut` is a few lines
that schedule `RefocusTarget` and check the target can still take focus. Hidden stops (`visibility`)
are left out through `canGrabFocus`, which reads resolved styles, so no test can show it without a
panel. For one scheduler tick, focus sits on the element behind
(`Documentation~/decisions.md`).

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (11 files)
ok   HEAD compiles: 8 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
58 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced, 16 types in the component reference
73/73 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Import the Settings sample and press Play. Change Music and press Back: the dialog opens with
   focus on "Keep editing".
2. Press Tab: focus goes to Discard. Tab again: back to Keep editing, not to the tabs behind.
3. Shift+Tab from Keep editing: focus goes to Discard.
4. Escape: the dialog closes and focus returns to Back.

## Unverified

The four tests haven't run, and the trap hasn't been driven by a keyboard in a running panel.
