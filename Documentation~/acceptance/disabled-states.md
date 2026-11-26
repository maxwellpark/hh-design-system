# Acceptance: disabled-states

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every control on a focusable base has a `:disabled` rule on its own class | the disabled check in `check.mjs`, exit 0 | pass | verified |
| The gallery builds each of them disabled, in its Disabled section or, for the button and toggle, where they already were | the same check: a variable declared once (`var` or its type), added to the page, with `SetEnabled(false)` outside strings | pass | verified |
| The check fails on a missing rule, a rule on a child only, a control never disabled, a disable on a same-named variable elsewhere, one written in a string, and one never added; it accepts any variable name and a typed declaration | eight planted cases | 8 of 8 | verified |
| The gallery compiles | the compile gate, `HootonHeroics.DesignSystem.Editor` | ok | verified |
| Each disabled control looks faded | the `:disabled` rules | | unverified |

## Coverage and complexity

About 40 lines in `check.mjs`, reusing the focus check's base table. A control inside a disabled
container fades twice, as `decisions.md` records. A disabled control's colours
aren't contrast-checked: WCAG exempts inactive components.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem.Editor [editor] (2 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (17 files)
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (disabled cases and total):

```
68 .meta files ok, 32 tokens match HHTheme, 124 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
ok   control declared with extra attributes is still found
ok   a focusable control with no disabled rule fails
ok   a focusable control the gallery never disables fails
ok   a disabled control is matched by type, not variable name
ok   a disable on a same-named variable elsewhere doesn't count
ok   a disable written in a string doesn't count
ok   a disabled control never added to the page doesn't count
ok   a control declared with its type rather than var counts
ok   a disabled rule on a child only doesn't count
110/110 passed
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery's Disabled section: an icon button, slider, dropdown, text field, radio row,
   stepper, tabs, list and scroll view, each faded and unresponsive to clicks and Tab.

## Unverified

The faded look in Unity.
