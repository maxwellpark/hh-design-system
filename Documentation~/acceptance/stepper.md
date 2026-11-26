# Acceptance: stepper

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHStepper`, its tests and the gallery compile, editor and player | the compile gate | 16 of 16 | verified |
| It carries `.hh-stepper` and shows the chosen value | `StepperTests.StepperCarriesItsClassAndShowsTheChosenValue` | | written, not run |
| Stepping stops at the ends and disables the arrow there | `StepperTests.SteppingStopsAtTheEndsAndDisablesThatButton` | | written, not run |
| The index stays within the choices, including when they change or are cleared | `StepperTests.TheIndexStaysWithinTheChoices` | | written, not run |
| An index set before its choices, as UXML may set them, lands when they arrive | `StepperTests.AnIndexSetBeforeItsChoicesLandsWhenTheyArrive` | | written, not run |
| The arrows don't take focus, so the field does | `StepperTests.ItsButtonsLeaveFocusToTheField` | | written, not run |
| `label`, `choices` and `index` apply from UXML | `StepperTests.UxmlChoicesAndIndexApply` with the fixture | | written, not run |
| Left and right step it when focused; a step against an end is clamped first, so it sends no change event | `HHStepper.OnNavigationMove`, `Step` | | verified by reading |
| Its focus ring, value and arrows meet AA (text) or 3:1 (non-text) in both themes | four pairs, below | pass | verified |
| The focus check holds it (`BaseField` added to the focusable bases) and the reference matches | `check.mjs` | pass | verified |
| It looks right and steps with a pad | | | unverified |

| Use | Pair | Minimum |
|---|---|---|
| Stepper value text | text on bg | 4.5 |
| Stepper value edge against a card | violet-light on surface | 3 |
| Stepper arrows | violet-light on violet-ghost over surface | 4.5 |
| Focus ring on a stepper's value | focus on bg | 3 |

## Coverage and complexity

Clamping lives in `SetValueWithoutNotify`, so every route (index, value, step, choices) passes
through it; five of the six tests drive it.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem [editor] (27 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (17 files)
ok   HootonHeroics.DesignSystem [player] (27 files)
ok   171 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
68 .meta files ok, 32 tokens match HHTheme, 120 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
97/97 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open the gallery: Resolution reads 1080p between two ghost arrows.
2. Click the right arrow twice: 1440p, then 4K, and the right arrow greys out.
3. Tab to it: the value box turns the focus colour. Left steps back; Tab moves on, skipping the arrows.

## Unverified

Rendering, pad input, and the six tests.
