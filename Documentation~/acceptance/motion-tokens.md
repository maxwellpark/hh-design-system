# Acceptance: motion-tokens

Node 22 from the package root, running `Tools~/verify.sh`'s Node steps; its Godot step didn't run
(no Godot binary here). The transitions themselves need Unity to see.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Three motion tokens exist and `HHTheme` mirrors them (seconds as floats, easing as a string) | `node Tools~/check.mjs` | 28 tokens match | verified |
| A drifted duration or easing fails the check, and `200ms` equals `0.2s` | `check.test.mjs` "duration drifts", "easing drifts", "duration in ms matches seconds" | pass | verified |
| Every `var(--hh-*)` the transitions use is a token | the reference check in `node Tools~/check.mjs` | exit 0 | verified |
| `Brand~/tokens.json` and the Godot constants carry the motion group, drift in either fails with its message, `200ms` and Godot's `1.0` compare as numbers, and a non-time duration is refused | `node Tools~/check-tokens.mjs`; `node --test Tools~/check-tokens.test.mjs`, the seven motion cases | exit 0, 21/21 | verified |
| Buttons fade between rest, hover and pressed; inputs fade their border; the progress fill slides to a new value | `transition-*` rules in `hh-components.uss` | | unverified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `parseToken` | colour / duration in s / in ms / easing / px / anything else | clean run; the three motion cases; the earlier "size not in px" and colour cases |

## Coverage and complexity

Not measured (child processes). `parseToken` is 15 lines with four branches.

## Behaviour transcript

`node Tools~/check.mjs`, exit 0:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
29 .meta files ok, 28 tokens match HHTheme, 17 contrast pairs checked, 6 controls referenced
```

## Hands-on checklist (Unity 6, two minutes)

1. In the Basic UI sample, hover Play and Reset: the colour eases in over about a tenth of a second.
2. Click Play: the progress fill slides to its new width rather than jumping.

## Unverified

- The Godot add-on's tests, which `Tools~/verify.sh` runs with a Godot 4.7 binary; the new
  constants are plain values its tests don't read.

- How the transitions look, and whether Unity applies `var()` inside `transition-duration` and
  `transition-timing-function`; the compile gate flags an import warning if it doesn't parse.
