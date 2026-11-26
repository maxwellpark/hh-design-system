# Changelog

## [Unreleased]
- Motion tokens `--hh-duration-fast` (0.12s), `--hh-duration-normal` (0.2s) and
  `--hh-easing-standard` (ease-out), mirrored in `HHTheme`. Buttons and inputs ease their colour
  changes, and the progress fill eases its width.
- `Tools~/check.mjs` fails on a stylesheet using an undefined token, a control's class with no
  rule in `hh-components.uss`, or a control with no EditMode test.
- `Brand~/`: the engine-neutral brand kit. `tokens.json` mirrors `hh-tokens.uss` for engines other than Unity and adds the studio splash timing; `hh-logo.png` is the studio mark; `README.md` has the splash, logo and credits rules.
- `Godot~/addons/hh_studio`: the studio splash for Godot 4 (`hh_splash.gd`) and the tokens as GDScript constants, with a headless test harness.
- `Tools~/check-tokens.mjs` fails when `tokens.json` or the Godot constants drift from `hh-tokens.uss`; `Tools~/verify.sh` runs it, `Tools~/check.mjs` and the Godot tests. Unity ignores folders ending in `~`, so Unity projects see no change.
- `HHToggle`, `HHSlider`, `HHDropdown` and `HHTextField`, styled through the tokens, with hover,
  focus and disabled states. The dropdown's open list keeps Unity's default look for now.
- New `--hh-color-track` token for slider and progress tracks, which were invisible on a card.
- Form-control selectors carry `.unity-base-field` and `:enabled` so they outrank Unity's defaults.
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
