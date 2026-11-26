# Acceptance: component-reference

Node 22. No Unity code changed, so the compile gate's result is unchanged from main.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `Documentation~/components.md` has a section for each of the 15 controls and `HHTooltip` | `check.mjs` summary: "16 types in the component reference" | 16 | verified |
| Each section names the type's USS classes, its `[UxmlAttribute]` attributes in UXML spelling, and every token its rules in `hh-components.uss` use | the reference check in `check.mjs`, exit 0 | pass | verified |
| The check fails on a missing section, a stale section, a missing class (including one only named as a longer class), a missing attribute, a missing token and an undefined token | seven planted cases in `check.test.mjs` | 7 of 7 | verified |
| The UXML examples parse in Unity and the attribute spellings match what UI Builder shows | the examples follow Unity's `[UxmlAttribute]` kebab-case naming | | unverified |
| The prose (states, defaults, behaviour) matches the controls | read against `Runtime/*.cs` and `hh-components.uss` by hand | | unverified beyond review |

## Coverage and complexity

The new check is 50 lines in `check.mjs`, covered by seven planted cases. Prose isn't checked:
states, defaults and behaviour notes can drift without the check noticing.

## Behaviour transcript

`node Tools~/check.mjs` (last line):

```
58 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced, 16 types in the component reference
```

`node Tools~/check.test.mjs` (reference cases and total):

```
ok   a control with no reference section fails
ok   a reference section for no control fails
ok   a class missing from the reference fails
ok   an attribute missing from the reference fails
ok   a reference naming an unknown token fails
68/68 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Paste the HHDialog and HHSegmentedBar examples from `components.md` into a UXML file with
   `xmlns:hh="HootonHeroics.DesignSystem"` and open it in UI Builder: no unknown-attribute warnings.
2. Select each element: the Inspector's attribute names match the reference.

## Unverified

The UXML examples in Unity, and the prose beyond a read against the code.
