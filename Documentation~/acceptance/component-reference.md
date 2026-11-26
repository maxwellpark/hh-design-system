# Acceptance: component-reference

Node 22. No Unity code changed, so the compile gate's result is unchanged from main.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `Documentation~/components.md` has a section for each of the 15 controls and `HHTooltip` | `check.mjs` summary: "16 types in the component reference" | 16 | verified |
| Each section's Classes, Attributes and Tokens lists equal the type's USS classes, its `[UxmlAttribute]` names in UXML spelling (or the name the mark gives), and the tokens its rules in `hh-components.uss` use: nothing missing, nothing stale | the reference check in `check.mjs`, exit 0 | pass | verified |
| The check fails on a missing or stale section, and on a missing or stale class, attribute or token, including one named only in prose or only as a longer class; it reads an attribute's own name and one behind a second mark | twelve planted cases in `check.test.mjs` | 12 of 12 | verified |
| The UXML examples parse in Unity and the attribute spellings match what UI Builder shows | the examples follow Unity's `[UxmlAttribute]` kebab-case naming | | unverified |
| The prose (states, defaults, behaviour) matches the controls | read against `Runtime/*.cs` and `hh-components.uss` by hand | | unverified beyond review |

## Coverage and complexity

The new check is about 60 lines in `check.mjs`, covered by twelve planted cases. It reads only the
three lists; prose (states, defaults, behaviour) isn't checked and can drift without it noticing.
The dropdown's open menu is styled in `hh-dropdown-menu.uss`, which the check doesn't read, so its
tokens aren't listed.

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
ok   a class named only in prose fails
ok   a modifier named only as a longer class fails
ok   a class the control doesn't have fails
ok   an attribute missing from the reference fails
ok   an attribute named only in prose fails
ok   an attribute the control doesn't have fails
ok   an attribute with its own name or a second mark is read
ok   a token a control's rules use, missing from its section, fails
ok   a token a control's rules don't use fails
73/73 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Paste the HHDialog and HHSegmentedBar examples from `components.md` into a UXML file with
   `xmlns:hh="HootonHeroics.DesignSystem"` and open it in UI Builder: no unknown-attribute warnings.
2. Select each element: the Inspector's attribute names match the reference.

## Unverified

The UXML examples in Unity, and the prose beyond a read against the code.
