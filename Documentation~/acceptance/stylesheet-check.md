# Acceptance: stylesheet-check

Node 22, from the package root. Pure tooling.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| A rule missing its closing brace fails, naming the line where the next rule opens inside it | `check.test.mjs` "a rule missing its closing brace fails"; also the check run on `72eef7d`, the dialog merge that dropped a brace | case passes; `72eef7d` fails at line 331 | verified |
| A misspelt property, a declaration without a colon, a missing last semicolon, a stray brace, a rule with no selector, a rule never closed, a stray `;` between rules and text after the last rule each fail | eight planted cases | pass | verified |
| A bad declaration is reported at its own line | "a bad declaration is reported at its own line" (line 5) | pass | verified |
| A `;` or brace inside a quoted `url()`, custom properties and an empty rule pass | two planted cases | pass | verified |
| `main`'s three stylesheets, including the theme sheet's `@import` lines, pass | `node Tools~/check.mjs` | exit 0 | verified |
| The property list is Unity 6000.0's | `Tools~/uss-properties.txt`: the editor's `UnityEngine.UIElementsModule.dll` strings, filtered, plus `top` and `all` by hand | 95 properties | verified (by the command in decisions.md) |

## Paths

| Code | Path | Evidence |
|---|---|---|
| braces | balanced / nested open / stray close / never closed / inside quotes | clean run / the brace cases / `72eef7d` / the url case |
| before a selector | nothing / `@import` / other text / empty selector | clean run / theme sheet / stray `;` case / no-selector case |
| declarations | known / unknown / custom `--` / no colon / no final semicolon / `;` in a url / empty rule | clean run and the cases |
| after the last rule | nothing / `@import` lines / other text | clean run / theme sheet / the text case |

## Coverage and complexity

Not measured. `checkStylesheet` is about 75 lines and `splitTop` 30; each is one pass over the text,
so they stay single loops.

## Behaviour transcript

`node Tools~/check.mjs`, exit 0:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
36 .meta files ok, 28 tokens match HHTheme, 27 contrast pairs checked, 9 controls referenced
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
ok   duration drifts
ok   duration in ms matches seconds
ok   easing drifts
ok   // inside a stylesheet url() doesn't hide a bad token
ok   a rule missing its closing brace fails
ok   a misspelt property fails
ok   a declaration without a colon fails
ok   a missing last semicolon fails
ok   a stray closing brace fails
ok   a semicolon or brace inside a quoted url passes
ok   a bad declaration is reported at its own line
ok   a rule with no selector fails
ok   a rule never closed fails
ok   text after the last rule fails
ok   a stray semicolon between rules fails
ok   custom properties and an empty rule pass
49/49 passed
```

## Unverified

- Value syntax: a known property with a bad value (say `color: 12px`) passes; only Unity's importer
  judges values.
- Selectors beyond "not empty".
