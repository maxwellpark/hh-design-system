# Acceptance: focus-rules

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every control on a focusable Unity base (Button, Toggle, Slider, DropdownField, TextField, RadioButtonGroup, ListView, TabView for its headers) has a `:focus` rule whose selector starts with its own class, with `:focus` on it or the part right after | the focus check in `check.mjs`, exit 0 | pass | verified |
| The check fails when a focusable control loses its focus rule, or has one only inside another control, on a variant or as `:not(:focus)`, and on a base it doesn't list; it accepts a rule on a part | six planted cases | 6 of 6 | verified |
| `HHListView` rings in the focus colour when focus is anywhere inside it | Unity's `FocusController` sets `:focus` on each composite-root ancestor of the focused element, and the ListView is one (read in IL by review); "Focus ring against a card" holds the colour at 3:1 | | verified by reading, not in Unity |
| The reference lists the new state and token | `check.mjs` reference check | pass | verified |

## Coverage and complexity

The base tables are kept by hand, and a control on a base in neither fails, so a new base gets
decided. The list's 2px edge takes 4px from its viewport. `HHDialog` is
focusable outside the tab order and isn't on a focusable base, so it isn't held.

## Behaviour transcript

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (focus cases and total):

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   a focusable control with no focus rule fails
ok   a focus rule on a part of the control passes
ok   a focus rule written inside another control doesn't count
ok   a focus rule on a variant only doesn't count
ok   a :not(:focus) rule doesn't count
ok   a control on an unlisted base fails
ok   a guide's UXML may use its Unity base's attributes
94/94 passed
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
