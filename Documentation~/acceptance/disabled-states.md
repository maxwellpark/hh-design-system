# Acceptance: disabled-states

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every control on a focusable base has a `:disabled` rule on its own class | the disabled check in `check.mjs`, exit 0 | pass | verified |
| The gallery builds each of them disabled, in its Disabled section or, for the button and toggle, where they already were | the same check, matching `var x = new HHType` with `x.SetEnabled(false)` by type | pass | verified |
| The check fails on a missing rule and on a control never disabled, and doesn't care what the variable is called | three planted cases | 3 of 3 | verified |
| The gallery compiles | the compile gate, `HootonHeroics.DesignSystem.Editor` | ok | verified |
| Each disabled control looks faded | the `:disabled` rules | | unverified |

## Coverage and complexity

About 30 lines in `check.mjs`, reusing the focus check's base table. A disabled control's colours
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
ok   a focusable control with no disabled rule fails
ok   a focusable control the gallery never disables fails
ok   a disabled control is matched by type, not variable name
105/105 passed
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery's Disabled section: an icon button, slider, dropdown, text field, radio row,
   stepper, tabs, list and scroll view, each faded and unresponsive to clicks and Tab.

## Unverified

The faded look in Unity.
