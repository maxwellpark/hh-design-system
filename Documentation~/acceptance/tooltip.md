# Acceptance: tooltip

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHTooltip`, the icon button's use of it and the tests compile, editor and player | the compile gate | 6 builds | verified |
| Tooltip text reads on its background; its class has a rule | contrast pair "Tooltip text"; the reference check | pass | verified |
| Showing adds a styled label with the text to the layer, and hiding removes it | `ShowAddsAStyledLabelToTheLayerAndHideRemovesIt` | compiled, not run | unverified |
| With no text it falls back to the target's `tooltip`, and with none it shows nothing | `FallsBackToTheTargetsTooltipAndShowsNothingWithoutText` | compiled, not run | unverified |
| Removing the manipulator hides the label | `RemovingTheManipulatorHidesTheLabel` | compiled, not run | unverified |
| An icon button's label shows as its tooltip | `IconButtonsCarryATooltipForTheirLabel` | compiled, not run | unverified |
| Hover or focus shows it after 500ms; leave, blur, Escape or detach hides it; the label sits below the target and never takes clicks | the callbacks in `HHTooltip` | needs a live panel | unverified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `Show` | text and layer / no text / no layer | the show tests (not run); no layer: none |
| `Text` | own text / target's tooltip / neither | the fallback test (not run) |
| events | enter / focus / leave / blur / Escape / detach | none (need a panel) |

## Coverage and complexity

Needs a Unity test run; not measured. Every member is under 15 lines with at most two branches.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
44 .meta files ok, 29 tokens match HHTheme, 32 contrast pairs checked, 12 controls referenced
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem [editor] (18 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (6 files)
ok   Sample.BasicUI [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (18 files)
ok   Sample.BasicUI [player] (1 files)
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, three minutes)

1. Hover an `HHIconButton` with `tooltip="Settings"` for half a second: a small dark label reading
   "Settings" appears just below it. Move away: it goes.
2. Tab to the button: the label appears; press Escape: it goes.
3. Run the EditMode tests: the four in `TooltipTests` pass.

## Unverified

The timing, placement and event routing in a live panel, and the four tests. Near the bottom or
right edge of the screen the label isn't flipped back on screen (a recorded decision).
