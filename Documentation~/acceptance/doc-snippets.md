# Acceptance: doc-snippets

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Each fenced `csharp` block in `Documentation~/*.md` compiles as a sample-like `Docs.<guide>` assembly, editor and player | "a doc snippet that compiles passes" in `unity-compile.test.sh` | pass | verified |
| A snippet that doesn't compile fails the gate, reported at the guide's own line | "a doc snippet that doesn't compile fails at its line in the guide" (`Documentation~/index.md(line,col): error CS`) | pass | verified |
| The gate's other twelve cases still hold | `unity-compile.test.sh` | 14 of 14 | verified |
| Guide UXML may use only `hh:` elements in the component reference, with their listed or example attributes or VisualElement's own | three planted cases in `check.test.mjs` | 3 of 3 | verified |
| `components.md` itself and the acceptance packs aren't scanned | `check.mjs` skips `components.md`; only top-level `Documentation~/*.md` is read | | verified by reading |

## Coverage and complexity

About 15 lines in the gate and 35 in `check.mjs`. Only `csharp` fences compile; `cs` or
`c#` fences are left alone, so a guide opts in by the fence name.

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
ok   a doc snippet that compiles passes
ok   a doc snippet that doesn't compile fails at its line in the guide
ok   a missing editor exits 2
0 failed
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
