# Changelog

## [Unreleased]
- EditMode tests in `Tests/Editor`. Run them by adding the package to a project's `testables`.
- `Tools~/check.mjs` checks WCAG AA contrast for every text and focus pair listed in
  `Tools~/contrast.json`, blending translucent colours onto what they sit over.
- The ghost button's hover tint drops from 28% to 22%, lifting its text from 4.4:1 to 4.8:1.
- The progress bar's title sits above the bar instead of on it: no colour reads on both the teal
  fill and the track (white on teal is 1.6:1).

## [0.2.0] - 2026-11-26
- Requires Unity 6 (6000.0). `HHButton` uses `[UxmlElement]` in place of the deprecated
  `UxmlFactory`, and gains a `Ghost` property (`ghost="true"` in UXML).
- New `HHProgressBar` control and `.hh-progress-bar` styles.
- New `hh-theme.tss`: pick it as the PanelSettings theme instead of adding stylesheets per document.
- Fix: hovering a ghost button turned its background the same colour as its text.
- Buttons show a focus ring (`--hh-color-focus`), have a pressed state, and dim when disabled.
  UI Toolkit has no `:focus-visible`, so the ring also shows after a pointer click.
- `HHTheme` mirrors every token (adds `violetGhost`, `violetGhostHover`, `textDim`, `focus`,
  `space1`, `space3`, `radiusSm` and the font sizes), checked by `Tools~/check.mjs`, which also fails on a `.meta` that isn't committed.
- Commit `.meta` files, without which Unity skipped the package's assets when installed from git.

## [0.1.0] - 2026-07-31
- Initial package: design tokens (USS), `HHTheme` ScriptableObject, `.hh-*` component styles,
  `HHButton` control, `HHStyles.Apply` helper, editor docs menu, and a Basic UI sample.
