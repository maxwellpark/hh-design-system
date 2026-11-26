# Acceptance: hold-button

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHHoldButton`, its tests and the gallery compile, editor and player | the compile gate | ok | verified |
| It carries `.hh-button`, so it looks like one, and holds for at least 0.1s | `HoldButtonTests.ItLooksLikeAButtonAndHoldsForAtLeastATenthOfASecond` | | written, not run |
| A hold fills, then confirms once at `hold-seconds` and resets; a finished hold doesn't confirm again | `HoldButtonTests.HoldingFillsThenConfirmsOnceAndResets`, driving `Advance` with explicit times | | written, not run |
| Letting go early cancels, and the next hold starts from nothing | `HoldButtonTests.LettingGoEarlyCancels` | | written, not run |
| A disabled button doesn't start a hold, and disabling mid-hold stops it | `HoldButtonTests.ADisabledButtonDoesNotStartAHold`, `DisablingMidHoldStopsIt` | | written, not run |
| A confirmed hold waits for the key or pointer to be released before another, so a held key can't confirm twice | `HoldButtonTests.AConfirmedHoldWaitsForReleaseBeforeAnother` | | written, not run |
| It has no Clickable, so `clicked` never fires and Space, Enter or a pad's submit can't confirm at once | `HoldButtonTests.ItHasNoClickableSoNothingButAHoldConfirms`; Button's submit handler does nothing without one (IL, read by review) | | written, not run |
| Only the left pointer that started a hold, and only the key that started it, can end it | `OnPointerDown`, `ReleasePointer`, the `KeyUp` handler | | verified by reading |
| `hold-seconds` applies from UXML | `HoldButtonTests.UxmlHoldSecondsApplies` with the fixture | | written, not run |
| Holding allocates nothing per frame: one scheduled item, paused between holds, and a struct width | `Begin`, `SetFill` | | verified by reading |
| Reduced motion never shortens the hold, which is an attribute, not a duration token | `HoldSeconds` | | verified by reading |
| The fill meets 3:1 on the button in both themes, hovered or not, since a hold button keeps the button fill on hover; focus and disabled rules exist, and the gallery builds one disabled | "Hold button fill against the button, hovered or not"; `check.mjs` focus and disabled checks | pass | verified |
| A pad's submit does nothing, since Unity sends it with no release | `decisions.md` | | recorded |
| Pointer, Space and Enter hold it in a running panel | | | unverified |

## Coverage and complexity

The timing is all in `Advance`, `Begin` and `Cancel`, which the tests drive without a panel; the
scheduler only calls `Advance` with each tick's elapsed time.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem [editor] (28 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (2 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (18 files)
ok   HootonHeroics.DesignSystem [player] (28 files)
ok   185 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 18 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
110/110 passed
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery's Buttons: "Hold to quit". Press and hold: a text-coloured bar grows along its foot and
   reaches the end in a second and a half. Let go early: the bar empties.
2. Tab to it and hold Space: the same. Release early: it empties.
3. The Disabled section's "Hold to skip" is faded and does nothing.

## Unverified

Input in a running panel, and the eight tests.
