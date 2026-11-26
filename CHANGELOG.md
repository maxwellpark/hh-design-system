# Changelog

## [0.2.0] - 2026-11-26
- Requires Unity 6 (6000.0). `HHButton` uses `[UxmlElement]` in place of the deprecated
  `UxmlFactory`, and gains a `Ghost` property (`ghost="true"` in UXML).
- New `HHProgressBar` control and `.hh-progress-bar` styles.
- New `hh-theme.tss`: pick it as the PanelSettings theme instead of adding stylesheets per document.
- Fix: hovering a ghost button turned its background the same colour as its text.
- Buttons show a focus ring (`--hh-color-focus`) for keyboard and gamepad navigation, and dim
  when disabled.
- `HHTheme` now mirrors every token (adds `violetGhost`, `violetGhostHover`, `textDim`, `focus`,
  `space1`, `space3`, `radiusSm` and the font sizes), checked by `Tools~/check.mjs` in CI.
- Commit `.meta` files, without which Unity skipped the package's assets when installed from git.

## [0.1.0] - 2026-07-31
- Initial package: design tokens (USS), `HHTheme` ScriptableObject, `.hh-*` component styles,
  `HHButton` control, `HHStyles.Apply` helper, editor docs menu, and a Basic UI sample.
