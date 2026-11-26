# Acceptance: reference-check

Node 22, from the package root. Pure tooling, so everything here is checked by machine.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| A stylesheet using a token that isn't defined fails the check | `check.test.mjs` "stylesheet using an undefined token fails" | pass | verified |
| A control's `UssClassName` with no rule in `hh-components.uss` fails | "class with no rule fails" | pass | verified |
| A `[UxmlElement]` control with no EditMode test naming it fails | "control with no test fails" | pass | verified |
| `main`'s six controls all have rules and tests, and every `var(--hh-*)` is defined | `node Tools~/check.mjs` | exit 0, 6 controls | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| token references | defined / undefined / inside a comment (ignored) | clean run / case above / none |
| class rules | rule present / missing / a longer class sharing the prefix (`hh-button--ghost` doesn't satisfy `hh-button`'s check by accident, by the `(?![\w-])` guard) | clean run / case above / none |
| control tests | named in a test / not named | clean run / case above |

## Coverage and complexity

Not measured (child processes). The new block is about 35 lines in three loops, each with one
branch.

## Behaviour transcript

`node Tools~/check.mjs`, exit 0:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
29 .meta files ok, 25 tokens match HHTheme, 17 contrast pairs checked, 6 controls referenced
```

`node Tools~/check.test.mjs`, exit 0:

```
ok   clean package passes
ok   asset without a .meta
ok   new top-level folder without a .meta
ok   two .meta files share a guid
ok   .meta without a guid
ok   colour drifts
ok   alpha drifts
ok   size drifts
ok   token with no field
ok   field with no token
ok   unparsable colour
ok   size not in px
ok   commented-out token is ignored
ok   .meta with Windows line endings
ok   .meta on disk but not committed
ok   contrast below AA fails
ok   accepted pair is reported, not failed
ok   pair naming an unknown token fails
ok   blended ratio is measured
ok   3:1 minimum applies to the focus ring
ok   accepted pair getting worse fails
ok   accepted pair that now passes fails
ok   accepted without a ratio fails
ok   pair missing a key fails readably
ok   translucent background without over fails
ok   stylesheet using an undefined token fails
ok   class with no rule fails
ok   control with no test fails
28/28 passed
```

## Unverified

Nothing in this change needs Unity.
