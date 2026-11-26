# Acceptance: dropdown-menu

Node 22 and Unity 6000.0.85f2's compiler, with the Unity class-name check from the class-check
branch run against this one.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every class styled is one Unity 6000.0's `GenericDropdownMenu` defines | read with Mono.Cecil; the class-name check over this branch | 106 uses, all known | verified |
| The new sheet parses and uses only known properties and tokens | `node Tools~/check.mjs` (stylesheet and reference checks) | exit 0 | verified |
| The theme sheet imports it | `hh-theme.tss` | | verified (by reading) |
| List text and the checkmark read on the card | two new contrast pairs (15.2 and 9.3:1) | 40 pairs pass | verified |
| An open list shows a rounded card, white items, a track hover and a teal checkmark | the rules | | unverified |

## Coverage and complexity

Stylesheet only.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
50 .meta files ok, 29 tokens match HHTheme, 40 contrast pairs checked, 15 controls referenced
```

## Hands-on checklist (Unity 6, two minutes)

1. With `hh-theme.tss` as the panel theme, open an `HHDropdown`: the list is a dark rounded card;
   hovering an item shows the track colour; the chosen item has a teal tick.
2. With only the two stylesheets on a document (no theme), the list keeps Unity's look, as
   documented.

## Unverified

Rendering and precedence over Unity's default list rules.
