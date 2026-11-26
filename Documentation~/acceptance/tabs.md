# Acceptance: tabs

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHTabs` and its tests compile, editor and player | the compile gate | 6 builds | verified |
| The class names styled are Unity 6000.0's own (`unity-tab__header`, `__header-label`, `__header-underline`, `unity-tab-view__content-container`) | read from `Tab` and `TabView` static constructors in the editor's `UnityEngine.UIElementsModule.dll` with Mono.Cecil | match | verified |
| The selected tab is styled through `:checked` | `Tab.SetActive` sets pseudo-state 8 (`Checked`) on the tab and its header, read from IL | match | verified |
| Unselected labels and the selected underline read on a card | two new contrast pairs (6.5:1 and 9.3:1) | pass | verified |
| `HHTabs` keeps the `TabView` class and adds `hh-tabs` | `TabsCarryTheirStyleClassAndStayATabView` | compiled, not run | unverified |
| `ui:Tab` children from UXML land in the view | `UxmlTabsLandInTheView` | compiled, not run | unverified |
| The rules outrank Unity's defaults for these parts | `.unity-tab-view` added to each selector | | unverified (check in UI Builder) |

## Coverage and complexity

One constructor; nothing to measure.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
46 .meta files ok, 29 tokens match HHTheme, 34 contrast pairs checked, 13 controls referenced
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem [editor] (19 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (7 files)
ok   Sample.BasicUI [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (19 files)
ok   Sample.BasicUI [player] (1 files)
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, three minutes)

1. Add `<hh:HHTabs><ui:Tab label="Audio" /><ui:Tab label="Video" /></hh:HHTabs>` to a card with the
   theme sheet: two bold labels, the first white with a teal underline, the second dim.
2. Hover the second: it brightens. Click it: the underline moves. Tab to a header: a teal edge.
3. In UI Builder, select a header and confirm the `.hh-tabs` rules apply over Unity's.
4. Run the EditMode tests: the two in `TabsTests` pass.

## Unverified

Rendering, the precedence over Unity's default theme, and the two tests.
