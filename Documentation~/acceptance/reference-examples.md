# Acceptance: reference-examples

Node 22. No Unity code changed.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every `hh:` element in `components.md`'s examples is a documented type, and each attribute is in its Attributes list, its Unity base's table, or VisualElement's | the UXML check, now over `components.md` too, exit 0 | pass | verified |
| Examples no longer widen what guides may use | the allowlist is built from the lists and tables only | | verified by reading |
| A typo or an unknown element in a reference example fails | two planted cases | 2 of 2 | verified |
| The guide cases still hold, so nothing the guides use relied on the widening | the eight guide cases | pass | verified |

## Behaviour transcript

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (UXML cases and total):

```
68 .meta files ok, 32 tokens match HHTheme, 124 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
ok   a guide's UXML using documented attributes passes
ok   a guide's UXML naming an unknown hh element fails
ok   a guide's UXML with a misspelt attribute fails
ok   a guide's UXML in a loosely written fence is still checked
ok   a guide's UXML may use its Unity base's attributes
ok   a typo in a reference example fails
ok   a reference example using an unknown element fails
112/112 passed
```

## Unverified

Nothing: this is a Node check.
