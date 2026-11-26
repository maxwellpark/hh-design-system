# Acceptance: goal-numbers

Node 22. No Unity code changed.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| GOAL.md's Controls, Tokens and contrast rows match what `check.mjs` counts (its control classes, tokens, and accepted of checked pairs) | the goal check, exit 0 | 19, 32, 3 of 126 | verified |
| A stale count, a stale contrast row, or a missing row fails, naming the value expected | three planted cases | 3 of 3 | verified |
| A row is found by its label with its Latest as the last cell, past the escaped pipe in a command cell | the row pattern; the control-count case edits exactly that row | | verified |
| Gate builds, check cases and tests, and the Done-when prose, stay hand-kept | the check covers three rows only | | verified by reading |

## Behaviour transcript

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (goal cases and total):

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
ok   a stale goal control count fails
ok   a stale goal contrast count fails
ok   a missing goal row fails
115/115 passed
```

## Unverified

Nothing: this is a Node check.
