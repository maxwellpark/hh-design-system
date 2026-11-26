# Changelog

## [0.2.0] - 2026-11-26
- `Brand~/`: engine-neutral brand kit. `tokens.json` is now the single source of the palette, spacing, radius, type sizes and studio splash timing; the studio mark (`hh-logo.png`); the splash, logo and credits rules.
- `Godot~/addons/hh_studio`: the studio splash for Godot 4 (`hh_splash.gd`) and the tokens as GDScript constants, with a headless test harness.
- `Tools~/check-tokens.mjs`: fails when the USS, `HHTheme` or Godot copies drift from `tokens.json`. `Tools~/verify.sh` runs it and the Godot tests.
- Folders ending in `~` are ignored by Unity, so a Unity project pulling this package sees no change.

## [0.1.0] - 2026-07-31
- Initial package: design tokens (USS), `HHTheme` ScriptableObject, `.hh-*` component styles,
  `HHButton` control, `HHStyles.Apply` helper, editor docs menu, and a Basic UI sample.
