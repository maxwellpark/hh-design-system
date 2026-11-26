# Acceptance: radio-group

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHRadioGroup` compiles for the editor and a player, with its tests | the compile gate | ok | verified |
| Every `.unity-*` class its rules name exists in Unity 6000.0 (`unity-radio-button-group__container`, `unity-radio-button`, `__text`, `__checkmark-background`) | the gate's Unity class check | ok | verified |
| It carries `.hh-radio-group` and stays a `RadioButtonGroup` | `RadioGroupTests.RadioGroupCarriesItsStyleClassAndStaysARadioButtonGroup` | | written, not run |
| Choices become radio buttons in order and `value` checks one | `RadioGroupTests.ChoicesBecomeButtonsAndValueChecksOne` | | written, not run |
| `label`, `choices` and `value` apply from UXML | `RadioGroupTests.UxmlChoicesAndValueApply` with the fixture | | written, not run |
| Every text and edge it draws meets AA (text) or 3:1 (non-text) in both themes | six pairs in `contrast.json`, below | pass | verified |
| It's in the gallery and the component reference, and has rules | `check.mjs` | pass | verified |
| It renders as a segmented row with the circles hidden, and arrow keys move the choice | | | unverified |

Pairs it adds:

| Use | Pair | Minimum |
|---|---|---|
| Radio choice text in its row | text-dim on bg | 4.5 |
| Checked radio choice text | bg on teal | 4.5 |
| Checked radio choice against its row | teal on bg | 3 |
| Radio group edge against a card | violet-light on surface | 3 |
| Focus ring on a radio choice | focus on bg | 3 |
| Focus ring on the checked radio choice | text on bg | 3 |

## Coverage and complexity

A constructor and a class name; the look is all USS. Keyboard behaviour is Unity's
`RadioButtonGroup`, not this package's.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (12 files)
ok   154 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 10 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
60 .meta files ok, 32 tokens match HHTheme, 100 contrast pairs checked across 2 theme(s), 16 controls referenced, 17 types in the component reference
74/74 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open Hooton Heroics > Design System > Gallery: under Form controls, a "Difficulty" row of Easy,
   Normal and Hard in a violet-edged pill, Normal filled teal with dark text.
2. Click Hard: it fills and Normal returns to dim text. Hover Easy: its text brightens.
3. Tab to the row: the focused choice shows a teal ring, or a white ring when it's the checked one.
4. Switch the gallery to high contrast: the same, in that theme's colours.

## Unverified

Rendering, keyboard behaviour in a live panel, and the three tests.
