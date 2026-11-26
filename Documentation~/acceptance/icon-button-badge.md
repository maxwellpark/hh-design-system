# Acceptance: icon-button-badge

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh` from the compile-gate branch).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHIconButton`, `HHBadge` and `HHTone` compile with Unity's compiler and UI Toolkit generator, as do their tests | the compile gate | 4 of 4 assemblies | verified |
| The icon reads against its tint (3:1), and badge text reads on every tone (4.5:1) | `node Tools~/check.mjs`, seven new pairs | 24 pairs pass | verified |
| An icon button's label becomes its tooltip and its icon its background, removable with `null` | `IconButtonUsesItsLabelAsTooltipAndShowsTheIcon` | compiled, not run | unverified |
| A badge shows nothing until given a count, then the count capped at `99+`, never negative; a dot hides the count and restores it | `BadgeShowsItsCountCappedAt99`, `DotBadgeHidesItsCountAndShowsItAgain` | compiled, not run | unverified |
| Exactly one tone class is set at a time, none for violet | `ToneSetsExactlyOneToneClass` | compiled, not run | unverified |
| `icon`, `tooltip`, `count`, `tone` and `dot` work from UXML | `UxmlAttributesReachTheBadgeAndIconButton` with the fixture | compiled, not run | unverified |
| Both controls have rules and tests | the reference check | 8 controls | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `HHBadge.Count` | normal / over 99 / negative / while a dot | the badge tests (not run) |
| `HHBadge.Tone` | each tone / back to violet | `ToneSetsExactlyOneToneClass` (not run) |
| `HHIconButton.Icon` | texture / null | the icon test (not run) |

## Coverage and complexity

Needs a Unity test run; not measured. Every member is under 10 lines; `Count`'s setter has three conditions.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
33 .meta files ok, 28 tokens match HHTheme, 24 contrast pairs checked, 8 controls referenced
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem (11 files)
ok   HootonHeroics.DesignSystem.Editor (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor (2 files)
ok   BasicUISample (1 files)
ok   HEAD compiles with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. Add `<hh:HHIconButton icon="..." tooltip="Settings" />` with any small white icon: a 40px
   rounded square, faint violet, the icon in light violet; hovering strengthens the tint.
2. Add `<hh:HHBadge count="3" />`, `count="120" tone="Pink"` and `dot="true"`: a violet pill
   reading 3, a pink pill reading 99+, and an 8px violet dot.
3. Run the EditMode tests: the five in `BadgeAndIconButtonTests` pass.

## Unverified

The rendering of both controls and the five tests, which need a licensed editor.
