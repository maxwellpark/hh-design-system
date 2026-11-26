# Acceptance: slider-value

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHSlider` with its value label, the tests, the gallery and the Settings sample compile, editor and player | the compile gate | 12 of 12 | verified |
| The value is hidden until `show-value` is set | `SliderValueTests.TheValueIsHiddenUntilAsked` | | written, not run |
| Percent is where the value sits in the range, as a whole percentage, including a range not starting at 0 and a value set without notify | `SliderValueTests.PercentFollowsTheValueAcrossTheRange` | | written, not run |
| Number is the value to at most two places | `SliderValueTests.NumberShowsTheValueToTwoPlaces` | | written, not run |
| `show-value` and `value-format` apply from UXML | `SliderValueTests.UxmlAttributesApply` with the fixture | | written, not run |
| Percent text comes from a 101-entry table, so dragging allocates nothing; Number allocates a string per change | `HHSlider.Percents`, `UpdateValueText`; `decisions.md` | | verified by reading |
| The value text meets AA on a card in both themes | "Slider value text on a card" in `contrast.json` | pass | verified |
| The reference names `.hh-slider__value`, `show-value` and `value-format` | `check.mjs` | pass | verified |
| The label sits right of the track at 48px or wider, so the track doesn't shift | `.hh-slider__value` | | unverified |

## Coverage and complexity

`UpdateValueText` has both branches tested. The text refreshes when the value changes, not when
only the range does, as the reference says.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem [editor] (26 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (15 files)
ok   Sample.Settings [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (26 files)
ok   Sample.Settings [player] (1 files)
ok   HEAD compiles: 12 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
80/80 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open the gallery: the Volume slider reads 60% at its right.
2. Drag it: the percentage follows, and the track stays put.
3. In the Settings sample, the Audio tab's Volume reads 80%.

## Unverified

Rendering, and the four tests.
