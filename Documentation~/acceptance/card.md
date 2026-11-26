# Acceptance: card

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHCard`, its tests, the gallery and all three samples compile, editor and player | the compile gate | 10 of 10 | verified |
| It carries `.hh-card`; its title has `.hh-title` and hides when empty | `CardTests.CardCarriesItsClassAndHidesAnEmptyTitle` | | written, not run |
| Children go in the body, and `Clear()` keeps the title | `CardTests.ChildrenGoInTheBodyAndClearKeepsTheTitle` | | written, not run |
| `title` and children apply from UXML | `CardTests.UxmlTitleAndChildrenApply` with the fixture | | written, not run |
| The gallery, the Basic UI sample (C# and UXML) and the Settings sample build their cards with it | the diff; `check.mjs` gallery check | | verified |
| Its classes have rules and its reference section matches | `check.mjs` | pass | verified |
| Cards look as before, with the title now 12px above the body | `.hh-card__title` margin | | unverified |

## Coverage and complexity

A constructor, a title property and a content container. No new colours, so no new contrast pairs:
the title is text on `surface`, which "Body text on a card" holds.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (12 files)
ok   Sample.BasicUI [editor] (1 files)
ok   Sample.HUD [editor] (1 files)
ok   Sample.Settings [editor] (1 files)
ok   Sample.BasicUI [player] (1 files)
ok   Sample.HUD [player] (1 files)
ok   Sample.Settings [player] (1 files)
ok   HEAD compiles: 10 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs`:

```
60 .meta files ok, 32 tokens match HHTheme, 88 contrast pairs checked across 2 theme(s), 16 controls referenced, 17 types in the component reference
74/74 passed
```

## Hands-on checklist (Unity 6, five minutes)

1. Open Hooton Heroics > Design System > Gallery: each section is a card with its heading, as before.
2. Import Basic UI and press Play: the "Hooton Heroics" card, then its subtitle and controls.
3. Import Settings and press Play: the "Settings" card with its tabs.

## Unverified

Rendering and the three tests.
