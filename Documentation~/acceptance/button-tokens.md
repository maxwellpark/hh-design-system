# Acceptance: button-tokens

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The two new tokens exist in `hh-tokens.uss`, `HHTheme`, `Brand~/tokens.json` and Godot, at today's violet values | `node Tools~/check.mjs`, `node Tools~/check-tokens.mjs` | 31 tokens match | verified |
| Only the button's three backgrounds use them | `grep -c hh-color-button Runtime/Styles/hh-components.uss` | 3 | verified |
| The two accepted contrast exceptions now sit on the button tokens, at the same ratios | the check's accepted lines; the three accepted-pair tests repointed | 4.00 and 2.47, 51/51 | verified |
| Nothing changes on screen | `--hh-color-button` equals `--hh-color-violet`, and `-hover` equals `violet-light`, in `hh-tokens.uss` | same values | verified (values); unverified (render) |

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
50 .meta files ok, 31 tokens match HHTheme, 40 contrast pairs checked, 15 controls referenced
```

`node Tools~/check-tokens.mjs`:

```
check-tokens: tokens.json and Godot match hh-tokens.uss
```

## Unverified

Rendering, which should be identical.
