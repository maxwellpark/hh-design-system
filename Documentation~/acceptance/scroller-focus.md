# Acceptance: scroller-focus

Node 22, Unity 6000.0.85f2's compiler, and a hand read of its UI Toolkit assembly's IL.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| A scroll view's scrollers take focus in Unity 6000.0 | IL of `UnityEngine.UIElementsModule.dll`: `Scroller` builds `ScrollerSlider`, a plain `Slider`; `BaseField` sets its input focusable (default tab index 0) and makes itself a composite root, so `:focus` reaches the slider; nothing in `Scroller`, `ScrollerSlider`, `ScrollView`, `BaseSlider` or `Slider` turns it off. Recorded in `decisions.md` | yes | verified by reading |
| A focused scroller's handle turns the focus colour, in a scroll view and in a list | `.hh-scroll-view` and `.hh-list-view` `.unity-slider:focus .unity-base-slider__dragger` | | unverified in Unity |
| That handle meets 3:1 against its track in both themes | "Focused scroller handle against its track" | pass | verified |
| The focus check holds `ScrollView` now, accepting `:focus` deeper only for it and only on a `.unity-*` part; the list still needs its own ring | the focus check; four planted cases | pass | verified |
| Every `.unity-*` class the rule names exists | the gate's class check | ok | verified |

## Coverage and complexity

The IL was read once by hand rather than by the gate, as the backlog item first planned: what lasts is
the focus check, which now fails if the rule goes. The "focusable control with no focus rule"
planted case moved to the icon button, since the list now has two focus rules.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   177 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (focus cases and total):

```
68 .meta files ok, 32 tokens match HHTheme, 122 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
ok   a focusable control with no focus rule fails
ok   a scroll view without a focused scroller rule fails
100/100 passed
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery and press Tab until focus reaches the page's scroller: its handle turns teal.

## Unverified

Whether Tab reaches the scrollers in a running panel, and the handle's colour there.
