# Acceptance: focus-rules

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every control on a focusable Unity base (Button, Toggle, Slider, DropdownField, TextField, RadioButtonGroup, ListView, TabView) has a `:focus` rule naming one of its classes, on it or a part | the focus check in `check.mjs`, exit 0 | pass | verified |
| The check fails when a focusable control loses its focus rule, and accepts a rule on a part | two planted cases | 2 of 2 | verified |
| `HHListView` rings in the focus colour when focused | `.hh-list-view:focus`; "Focus ring against a card" holds the colour at 3:1 | | unverified in Unity |
| The reference lists the new state and token | `check.mjs` reference check | pass | verified |

## Coverage and complexity

The base table is kept by hand: a control on a new focusable base needs adding. `HHDialog` is
focusable outside the tab order and isn't on a focusable base, so it isn't held.

## Behaviour transcript

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (focus cases and total):

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   3:1 minimum applies to the focus ring
ok   a focusable control with no focus rule fails
ok   a focus rule on a part of the control passes
82/82 passed
```

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   156 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 12 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery and Tab to the list: a teal ring round it; arrows move the selection.

## Unverified

The ring in Unity.
