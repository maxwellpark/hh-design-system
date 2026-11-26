# Acceptance: tab-badges

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHTabs.SetBadge`, its tests and the gallery compile, editor and player | the compile gate | ok | verified |
| A badge shows on the tab's header with the count, and a second call updates the same badge | `TabsTests.ABadgeShowsOnATabHeaderAndUpdatesInPlace` | | written, not run |
| 0 removes it, and a null tab is ignored | `TabsTests.ZeroRemovesTheBadge` | | written, not run |
| The badge is found by class on each call, so it holds however Unity rebuilds the header | `SetBadge` queries `.hh-tabs__badge` on `tab.tabHeader` | | verified by reading |
| Its count meets AA on its fill, and the fill 3:1 on the card, for a selected or unselected header (the header's background doesn't change) | two "Tab badge" pairs | pass | verified |
| The reference names `.hh-tabs__badge` and `SetBadge` | `check.mjs` | pass | verified |
| It sits beside the label in the header | `.hh-tabs__badge` margin | | unverified |

## Coverage and complexity

One method, both its branches tested. The badge ignores the pointer, so a click still selects the
tab.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem [editor] (27 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (17 files)
ok   HootonHeroics.DesignSystem [player] (27 files)
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
68 .meta files ok, 32 tokens match HHTheme, 128 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery's Tabs card: Audio, Video and Items, with a 3 badge beside Items.
2. Click Items: it selects, and the badge stays.

## Unverified

Rendering and the two tests.
