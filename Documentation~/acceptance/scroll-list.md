# Acceptance: scroll-list

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHScrollView`, `HHListView` and their tests compile, editor and player | the compile gate | 6 builds | verified |
| Every Unity class styled exists in 6000.0 (`unity-scroller--vertical`, `__low-button`, `__high-button`, `unity-base-slider__tracker`, `__dragger`, `unity-collection-view__item`, `--selected`, `unity-list-view__empty-label`) | read from the static constructors of `Scroller`, `BaseSlider`, `BaseVerticalCollectionView` and `BaseListView` with Mono.Cecil | all present | verified |
| Row text reads when hovered and selected, the selected edge and the scroller handle stand out | four new contrast pairs (10.9, 11.8, 6.2 and 4.4:1) | pass | verified |
| Both keep Unity's own class and add theirs | `ScrollAndListCarryTheirClassesOverUnitys` | compiled, not run | unverified |
| The list binds items as a `ListView`; UXML content lands in the scroll view's content; `fixed-item-height` applies from UXML | `ListBindsItemsAsAListView`, `UxmlContentAndAttributesApply` | compiled, not run | unverified |
| Scrollers are slim with no arrows, rows highlight on hover and selection | the `.hh-scroll-view` and `.hh-list-view` rules | | unverified |

## Coverage and complexity

Constructors only; nothing to measure.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
47 .meta files ok, 29 tokens match HHTheme, 36 contrast pairs checked, 14 controls referenced
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem [editor] (20 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (7 files)
ok   Sample.BasicUI [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (20 files)
ok   Sample.BasicUI [player] (1 files)
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. Put an `HHScrollView` with tall content on a card: an 8px violet-light handle on a dark track,
   no arrow buttons.
2. Bind an `HHListView` to ten strings with `fixedItemHeight = 36`: hover a row, it shows the
   track colour; click one, it gets a violet tint and a light edge on the left.
3. Run the EditMode tests: the three in `ScrollAndListTests` pass.

## Unverified

Rendering, precedence over Unity's default theme (including the dragger's own border child), and
the three tests.
