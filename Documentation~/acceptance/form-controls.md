# Acceptance: form-controls

No Unity editor was available, so nothing here has been compiled or seen in Unity. The colour
claims are measured by the package check; how the controls look is not.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHToggle`, `HHSlider`, `HHDropdown` and `HHTextField` exist as UXML elements carrying their `.hh-*` class | `Runtime/HH*.cs`; `FormControlsCarryTheirStyleClasses` | test not run | unverified |
| They keep their base controls' UXML attributes (`value`, `label`, `low-value`, `high-value`, `choices`, `index`) | `FormControlsKeepTheirUxmlAttributes` with `Tests/Editor/Fixtures/controls.uxml` | test not run | unverified |
| Text in a field or dropdown, the focus ring around it, the field, dropdown and toggle borders against a card, a checked toggle and the slider handle on its track all meet WCAG AA (3:1 for non-text) | `node Tools~/check.mjs`, seven new pairs | 17 pairs, exit 0 | verified (ratios) |
| Slider and progress tracks show on a card (they were `surface` on `surface`) | `--hh-color-track` #2E3166 | | unverified (render) |
| Labels are white; hover whitens borders; focus turns them teal; checked toggles fill violet and lighten on hover; the slider handle stays teal when hovered or dragged; disabled controls dim | `hh-components.uss` form rules | | unverified |
| These rules outrank Unity's default theme for the same parts | `.unity-base-field` and `:enabled` added to each selector | | unverified (check in UI Builder) |
| Every new file imports from a git install | `node Tools~/check.mjs`: 29 `.meta` files, all committed | exit 0 | verified (files) |

## Paths

| Control | States styled | Evidence |
|---|---|---|
| Toggle | rest / hover / checked / focus / disabled | none (no Unity) |
| Slider | rest / focus / disabled | none |
| Dropdown | rest / hover / focus / disabled; open list unstyled | none |
| Text field | rest / hover / focus / disabled | none |

## Coverage and complexity

No C# logic beyond constructors; not measured.

## Behaviour transcript

`node Tools~/check.mjs`, exit 0:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
29 .meta files ok, 25 tokens match HHTheme, 17 contrast pairs checked
```

## Hands-on checklist (Unity 6, ten minutes)

1. With the package from this branch and `hh-theme.tss` as the panel theme, add to a UXML card:
   `<hh:HHToggle label="Music" value="true" />`, `<hh:HHSlider label="Volume" low-value="0" high-value="1" value="0.5" />`,
   `<hh:HHDropdown label="Quality" choices="Low,High" index="1" />`, `<hh:HHTextField label="Name" />`.
2. Each label is white; the toggle box is violet with a white tick; the slider track shows against
   the card with a teal handle; the field and dropdown are darker wells with a violet-light border.
3. Hover the toggle, field and dropdown: borders turn white (a checked toggle lightens). Hover and
   drag the slider: its handle stays teal. Tab through: each shows a teal (toggle, field,
   dropdown) or white (slider handle) focus edge.
4. Call `SetEnabled(false)` on one, or set `enabled="false"` in UXML: it dims to about 40%.
5. In UI Builder, select each control's part (checkmark, dragger, input) and confirm the `.hh-*`
   rule is the one applied, not Unity's default.

Anything invisible, unreadable, mis-sized or throwing counts as broken.

## Unverified

- Rendering of every rule, including sizes: the toggle box is set to 20px and the slider handle
  keeps Unity's default size.
- The dropdown's open list, which keeps Unity's default look (in Ideas).
