# Acceptance: switch-toggle

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHToggle` with its switch, the tests, the gallery and the Settings sample compile, editor and player | the compile gate | 12 of 12 | verified |
| A toggle is a box until `IsSwitch`, which adds and removes `.hh-toggle--switch` | `SwitchTests.AToggleIsABoxUntilItIsASwitch` | | written, not run |
| The knob is a child of the checkmark and ignores the pointer, so a click still toggles | `SwitchTests.TheKnobRidesInTheCheckmark` | | written, not run |
| `switch="true"` applies from UXML, despite `switch` being a C# keyword | `SwitchTests.UxmlSwitchApplies` with the fixture; `[UxmlAttribute("switch")]` | | written, not run |
| Track and knob meet 3:1 in both states and both themes | four pairs in `contrast.json`, below | pass | verified |
| The knob slides on `--hh-duration-fast`, so `.hh-reduced-motion` stops the slide | `.hh-toggle--switch .hh-toggle__knob` transition | | verified by reading |
| The reference names `switch`, `.hh-toggle--switch` and `.hh-toggle__knob` | `check.mjs` | pass | verified |
| Every `.unity-*` class it styles exists in Unity 6000.0 | the gate's class check | ok | verified |
| It looks and slides like a switch | | | unverified |

| Use | Pair | Minimum |
|---|---|---|
| Switch knob, off, against its track | violet-light on track | 3 |
| Switch knob, on, against its fill | text on violet | 3 |
| Switch edge, off, against a card | violet-light on surface | 3 |
| Switch fill, on, against a card | violet on surface | 3 |

## Coverage and complexity

A class toggle and a child element; the look is USS. Focus and hover borders come from the toggle's
existing rules on the checkmark, so the focus check still holds it.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (16 files)
ok   162 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 12 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
66 .meta files ok, 32 tokens match HHTheme, 110 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
94/94 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open the gallery: Vibration is a violet switch with a white knob at the right; Hints is a dark
   track with a lilac knob at the left.
2. Click Hints: the knob slides right and the track fills violet. Tick Reduce motion and click
   again: it jumps with no slide.
3. Tab to a switch: its edge turns the focus colour. Space toggles it.

## Unverified

Rendering, the slide, and the three tests.
