# Acceptance: reference-check

Node 22, from the package root. Pure tooling, so everything here is checked by machine.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| A stylesheet using a token that isn't defined fails the check | `check.test.mjs` "stylesheet using an undefined token fails" | pass | verified |
| A control's class constant with no rule in `hh-components.uss` fails, including one built by concatenation (`UssClassName + "--ghost"`) | "class with no rule fails", "ghost rule removed fails, though .hh-button remains" | pass | verified |
| A `[UxmlElement]` control with no EditMode test naming it fails, however it's declared (arguments, more attributes, `sealed`), and a mention only in a comment doesn't count | "control with no test fails", "control declared with extra attributes is still found", "control named only in a test comment fails" | pass | verified |
| A token used with a fallback, `var(--hh-x, ...)`, is checked too; one inside a comment is ignored | "undefined token with a fallback fails", "undefined token inside a comment is ignored" | pass | verified |
| `main`'s six controls all have rules and tests, and every `var(--hh-*)` is defined | `node Tools~/check.mjs` | exit 0, 6 controls | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| token references | defined / undefined / with fallback / inside a comment | clean run and the cases above |
| class rules | literal / concatenated / unresolvable / missing rule / a rule for a longer class only | clean run, the cases above; unresolvable: none; longer class: the ghost case (`.hh-button-ghostly` doesn't satisfy `hh-button--ghost`) |
| control tests | named in a test / only in a comment / not named / unusual declaration / marks and classes disagree | clean run and the cases above; disagreement: none |

## Coverage and complexity

Not measured (child processes). The new block is about 60 lines: `classConstants` (8 lines) and three loops, none
over four branches.

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
ok   ghost rule removed fails, though .hh-button remains
ok   undefined token inside a comment is ignored
ok   undefined token with a fallback fails
ok   control named only in a test comment fails
ok   control declared with extra attributes is still found
33/33 passed
```

## Unverified

Nothing in this change needs Unity.
