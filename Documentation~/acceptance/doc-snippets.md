# Acceptance: doc-snippets

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Each fenced C# block (`csharp`, `cs`, `c#`, any case, indented or with trailing spaces) in `Documentation~/*.md` compiles as its own `Docs.<guide>.<n>` assembly, editor and player, so steps may redefine a type | "a doc snippet that compiles passes", "two steps of a guide may define the same type", "a cs fence is compiled too" | pass | verified |
| Two guides whose assembly names clash stop the gate rather than one hiding the other | "two guides whose snippet names clash stop the gate" | pass | verified |
| A snippet that doesn't compile fails the gate, reported at the guide's own line | "a doc snippet that doesn't compile fails at its line in the guide" (`Documentation~/index.md(line,col): error CS`) | pass | verified |
| The gate's other cases still hold | `unity-compile.test.sh` | 17 of 17 | verified |
| Guide UXML may use only `hh:` elements in the component reference, with their listed or example attributes, their Unity base's own (a table in `check.mjs`) or VisualElement's; fences are read like the gate's, a fence shown inside a longer one is skipped, values may hold `=` or `>`, and a bad attribute is reported at its own line | eight planted cases in `check.test.mjs` | 8 of 8 | verified |
| `components.md` itself and the acceptance packs aren't scanned | `check.mjs` skips `components.md`; only top-level `Documentation~/*.md` is read | | verified by reading |

## Coverage and complexity

A fence scanner in each of the gate and `check.mjs`. Snippets compile as runtime code only, with
warnings as errors; a guide about editor scripts would need an opt-out the gate doesn't have yet.
The base attribute table in `check.mjs` is kept by hand, so a missing entry fails safe.

The suite run below shows "a doc snippet that compiles passes" failing on exit 0: the gate passed,
but the case still expected the old assembly name. The pattern was corrected to `Docs.index.1`,
and that case was rerun on its own afterwards.

## Behaviour transcript

`bash Tools~/unity-compile.test.sh`:

```
ok   clean commit compiles
ok   a syntax error fails
ok   a UxmlElement class that isn't partial fails
ok   a broken test fails
ok   a deprecated API fails as a warning
ok   an unknown asmdef reference fails
ok   tests losing their runtime reference fail
ok   editor-only code in a runtime assembly fails the player build
ok   a misspelt Unity class in a stylesheet fails
ok   a real Unity class outside *UssClassName fields passes
ok   an uncommitted change is not compiled
FAIL a doc snippet that compiles passes: exit 0
ok   a doc snippet that doesn't compile fails at its line in the guide
ok   two steps of a guide may define the same type
ok   a cs fence is compiled too
ok   two guides whose snippet names clash stop the gate
ok   a missing editor exits 2
1 failed
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (guide cases and total):

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   a guide's UXML using documented attributes passes
ok   a guide's UXML naming an unknown hh element fails
ok   a guide's UXML with a misspelt attribute fails
83/83 passed
```

## Unverified

Nothing beyond the gate's own limits: it compiles, it doesn't run or import.
