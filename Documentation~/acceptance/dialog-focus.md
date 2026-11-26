# Acceptance: dialog-focus

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHDialog` and its tests compile, editor and player | the compile gate | ok | verified |
| The dialog's tab stops are its enabled, displayed, focusable elements with a tab index of 0 or more, in order, without the parts of a field that is itself a stop | `DialogTests.FocusStopsAreEnabledFocusablesInOrderWithoutTheirParts` | | written, not run |
| While open, focus leaving the last stop for outside goes to the first; leaving the first goes to the last; moving inside, or the panel losing focus, is left alone; a closed dialog never pulls | `DialogTests.FocusLeavingAnOpenDialogWrapsBackInside` | | written, not run |
| In a running panel, Tab and Shift+Tab wrap and a pointer focus outside comes back | `HHDialog.OnFocusOut` schedules the refocus | | unverified |

## Coverage and complexity

`RefocusTarget` and `FocusStops` carry the logic and both have a test; `OnFocusOut` is a few lines
that schedule the result. For one scheduler tick, focus sits on the element behind
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

The two tests haven't run, and the trap hasn't been driven by a keyboard in a running panel.
