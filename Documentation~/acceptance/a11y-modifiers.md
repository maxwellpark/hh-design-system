# Acceptance: a11y-modifiers

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner). The USS isn't imported anywhere here.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `.hh-reduced-motion` sets every `--hh-duration-*` token to 0s, and nothing else | the modifier check in `check.mjs` | pass | verified |
| `.hh-large-text` sets every `--hh-font-size-*` token to 1.25 times its `hh-tokens.uss` value, rounded to a whole pixel (15, 18, 23, 35), and nothing else | the modifier check | pass | verified |
| The check fails on a missing token, an off-scale size, a token outside the modifier's kind, and a name that isn't a token | four planted cases | 4 of 4 | verified |
| `HHAccessibility` adds and removes both classes, and ignores a null root | `AccessibilityTests` | | written, not run |
| The Settings sample (Access tab, applied on Save and Revert) and the gallery (two toggles) compile, editor and player | the compile gate | ok | verified |
| The Settings sample keeps using `HHAccessibility` | the sample check | pass | verified |
| Under a class on the document root, controls lose their transitions and grow their text | the theme sheets import `hh-components.uss` after the tokens, so the class rule wins over `:root` at equal specificity | | unverified |

## Coverage and complexity

Two one-line methods and two USS rules; the check holds the rules to the tokens. The 1.25 scale
and the class approach are in `decisions.md` as needs-review defaults.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem.Editor [editor] (2 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (14 files)
ok   Sample.Settings [editor] (1 files)
ok   Sample.Settings [player] (1 files)
ok   HEAD compiles: 10 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (modifier cases and total):

```
64 .meta files ok, 32 tokens match HHTheme, 100 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   a modifier named only as a longer class fails
ok   a modifier missing a token of its kind fails
ok   a large-text size off the 1.25 scale fails
ok   a modifier setting a token outside its kind fails
ok   a modifier setting something that isn't a token fails
78/78 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open the gallery and tick Large text: every label and control grows; untick it and they return.
2. Tick Reduced motion and hover a button: its colour changes at once, with no fade.
3. Import Settings, press Play, open the Access tab, tick Large text and press Save: the whole
   screen grows. Tick Reduce motion, press Back and Discard: it unticks and nothing changes.
4. In the gallery, switch to high contrast with Large text ticked: still large.

## Unverified

The USS in Unity, the two tests, and a dropdown's open menu (drawn outside the document, so it
keeps the theme's sizes, as `decisions.md` records).
