# Acceptance: high-contrast

Node 22 and Unity 6000.0.85f2's compiler. The button tokens, already on main, made a passing palette
possible.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every contrast pair passes AA in the high-contrast theme, with no accepted exceptions | `node Tools~/check.mjs` | 80 pairs across 2 themes, exit 0 | verified |
| A theme must define exactly the default's tokens, once each, parse, and have a theme sheet importing it | the five theme cases in `check.test.mjs` | pass | verified |
| Cards and dialogs show an edge in high contrast (`card-edge` #9A9AC8 is 7.3:1 against the page) and none in the default theme (`card-edge` equals `surface`) | the tokens and the `.hh-card` and dialog panel rules | | unverified (render) |
| An exception accepted in the default theme doesn't excuse another theme | "a pair accepted in the default theme must pass in another" | pass | verified |
| The theme sheet imports Unity's default theme, the high-contrast tokens, the components and the dropdown list | `grep -c import Runtime/Styles/hh-theme-high-contrast.tss` | 4 | verified |
| With the theme picked, every control shows black surfaces, white text, a yellow focus ring and darker buttons | the tokens | | unverified |

## Coverage and complexity

Not measured. The theme loop adds one pass per theme file over the same pairs.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
54 .meta files ok, 31 tokens match HHTheme, 80 contrast pairs checked across 2 theme(s), 15 controls referenced
```

`node Tools~/check.test.mjs` (last lines):

```
ok   a theme missing a token fails
ok   a theme with an extra token fails
ok   a pair accepted in the default theme must pass in another
54/54 passed
```

## Hands-on checklist (Unity 6, three minutes)

1. Set a panel's Theme Style Sheet to `hh-theme-high-contrast.tss` and open the Basic UI sample:
   black card, white text, bright violet accents, a yellow focus ring on Tab.
2. Swap back to `hh-theme.tss`: the usual look returns.

## Unverified

Rendering.
