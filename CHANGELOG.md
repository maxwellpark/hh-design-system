# Changelog

## [Unreleased]
- Toast actions: `HHToast.SetAction(label, onAction)` adds a ghost button (Undo, View) that runs the
  callback and dismisses the toast, and `HHToastStack.Show` takes the label and callback after
  `seconds`, so existing calls compile; a toast with an action stays at least 8 seconds. A toast now lays out as a row, its text wrapping beside the button.
- A focused scroller in an `HHScrollView` or `HHListView` shows it: its handle turns the focus colour.
  Unity puts scrollers in the tab order, and `check.mjs` now holds scroll views to a focus rule.
- Settings sample: a Resolution `HHStepper` on the Video tab and a Difficulty `HHRadioGroup` on the
  Player tab, saved and reverted with the rest.
- `HHStepper`: one value from an ordered list, stepped with its arrows or, when focused, left and
  right on a keyboard or gamepad; it stops at the ends, disabling the arrow there.
- `Tools~/check.mjs` fails on a listed sample without a README, or with a script its README doesn't
  name, so a sample's setup steps can't miss one.
- `HHToggle` gains `switch="true"`: a sliding switch for on/off settings, its knob sliding on the
  duration tokens so reduced motion holds. The gallery shows it on and off, and the Settings
  sample's Music uses it.
- Getting started guide: `Documentation~/getting-started.md` builds a themed card with a toggle and a
  button, then theme and accessibility switching. Its C# is compiled by the gate and its UXML
  checked against the component reference.
- `HHListView` shows focus with a ring in the focus colour, and `check.mjs` now fails on any control
  built on a focusable Unity base without a `:focus` rule.
- Doc snippets: the compile gate compiles each fenced `csharp` block in `Documentation~/*.md`, with
  errors reported at the guide's line, and `check.mjs` fails on guide UXML using an `hh:` element
  or attribute the component reference doesn't have.
- `HHSlider` can show its value beside the track: `show-value` turns it on, and `value-format`
  picks Percent (the default) or Number. The gallery and the Settings sample's volume show it.
- Pause menu sample: a card of Resume, Settings and Quit, with Quit confirmed through a dialog only
  its buttons can close.
- Accessibility: `HHAccessibility.SetReducedMotion` and `SetLargeText` add `.hh-reduced-motion`
  (every duration 0s) and `.hh-large-text` (every font size 1.25 times) to an element, under any
  theme. The Settings sample gains an Access tab with both (applied on Save), and the gallery a toggle
  for each.
- `HHCard`: the `.hh-card` surface as a control with a `title` attribute; children go in its body.
  The gallery and the Basic UI and Settings samples now use it. The `.hh-card` class alone still works.
- `HHRadioGroup`: one choice of several, such as difficulty, shown as a segmented row. The checked
  choice fills teal with dark text; every pair it adds passes AA in both themes.
- `HHDialog` keeps focus inside while open: Tab past the last stop wraps to the first, and
  Shift+Tab before the first wraps to the last. Focus moved outside by a pointer or code, or
  cleared, comes back on the next tick. With two open, only the one on top traps. A dialog with no
  focusable stops doesn't trap. Opening focuses the first stop, not only the first action.
- Contrast: the slider's focus ring and the dropdown arrow now have 3:1 pairs, and the empty
  track of a slider, progress bar, segmented bar or scroller is checked against the card at WCAG
  1.4.11's 3:1. It's 1.39:1 in the default theme, accepted until the owner picks a
  fix in BACKLOG.md, and passes in high contrast.
- HUD sample: a health bar, lives as a segmented bar, a bag button with a badge and toasts, driven
  by its own buttons so it runs under either input system.
- Component reference: `Documentation~/components.md` lists each control's UXML, classes,
  attributes, states and tokens. `Tools~/check.mjs` fails when it falls out of step with the code
  or `hh-components.uss`.
- Settings sample: a settings screen built from tabs, a toggle, a slider, a dropdown and a text field;
  Save shows a toast and Back asks through a dialog before throwing changes away.
- Component gallery: Hooton Heroics > Design System > Gallery opens an editor window with every
  control in its states, previewed under the default or high-contrast tokens. `Tools~/check.mjs`
  fails on a control missing from it.
- `HHThemes.Apply(panelSettings, theme)` and `HHThemes.Next(panelSettings, themes)` switch a panel's
  theme sheet at runtime. They change the PanelSettings asset, so in the editor the change outlives
  Play mode; the Basic UI sample cycles themes on a runtime copy instead.
- High-contrast theme: `hh-theme-high-contrast.tss` with `hh-tokens-high-contrast.uss`, black and near
  black surfaces, white text, a yellow focus ring and darker buttons. Every contrast pair passes AA
  in it outright; `Tools~/check.mjs` now checks each pair in each `hh-tokens-*.uss` theme, which must
  define the same tokens as `hh-tokens.uss`.
- `--hh-color-button` and `--hh-color-button-hover` (mirrored in `HHTheme`, `Brand~/tokens.json` and
  Godot) now colour the button; they take today's violet values, so nothing changes on screen.
- `HHTokens` reads the tokens an element resolves at runtime: `TryGetColor(element, "color-teal",
  out c)`, `TryGetFloat` for sizes, and `WhenResolved` to run code each time styles resolve, so
  code can follow the live theme.
- Dropdown lists: `hh-dropdown-menu.uss` styles the list a dropdown opens (a card with a track
  hover, white labels and a teal checkmark). `hh-theme.tss` imports it; documents styled with the
  two stylesheets alone keep Unity's list, since it opens at the panel root.
- The compile gate also checks every `.unity-*` class the stylesheets style against the class
  names Unity's own UI Toolkit assembly defines, read by `Tools~/unity-classes.cs`.
- `HHScrollView` and `HHListView`: slim scrollers without arrow buttons and a violet-light handle;
  list rows with a track-coloured hover and, when selected, a violet tint with a violet-light edge.
- `HHTabs`: Unity 6's `TabView` styled through the tokens: dim labels, white for the hovered and
  selected tab, a teal underline under the selected one and a teal focus edge. Unlike `TabView`,
  its headers take keyboard and gamepad focus, and Enter, Space or submit selects the tab. Use
  `ui:Tab` children as with `TabView`.
- `HHTooltip`, a manipulator: `element.AddManipulator(new HHTooltip("Text"))` shows the text below
  the element after a 500ms hover or focus, and hides it on leave, blur, Escape, a click or detach. It draws in the element's document root. With no
  text it shows the element's `tooltip`. `HHIconButton` carries one, since runtime panels don't show
  `tooltip` themselves.
- `HHToastStack.Show(text, tone, seconds)` shows an `HHToast` in the bottom-right corner of its
  parent, newest last, and dismisses it after its time (0 keeps it). Past `max-visible` the oldest
  go. Tones are info, success, warning and danger, shown as a coloured edge on the card.
- `HHDialog`: a modal that covers its parent with a `--hh-color-scrim` scrim and shows a card with
  a title, its UXML children as the body, and actions from `AddAction`. Escape, gamepad cancel and
  clicks on the scrim close it unless `require-choice` is set; opening focuses the first action and
  closing returns focus. `Closed` fires once per close.
- `Tools~/check.mjs` fails on a stylesheet with a missing or stray brace, a declaration without a
  colon or final semicolon, or a property not in `Tools~/uss-properties.txt`, Unity 6000.0's list.
- `HHProgressBar.Tone` (`tone` in UXML): violet, pink or peach fills; teal stays the default.
- `HHSegmentedBar`: separate segments for health, ammo or charges, with `segments`, `filled` and
  `tone`.
- `Tools~/unity-compile.sh` compiles every asmdef as Unity would (references, platforms, define
  constraints, editor and player builds) with Unity's compiler and generators, warnings as errors,
  and `Tools~/verify.sh` runs it; `Tools~/unity-compile.test.sh` plants ten cases.
- `HHIconButton`: an icon-only button tinted like ghost text, whose label becomes its tooltip.
- `HHBadge`: a count (capped at `99+`) or a dot, in violet, teal, pink or peach, always with dark
  text, since white is under 3:1 on every tone. `HHTone` names the tones.
- Motion tokens `--hh-duration-fast` (0.12s), `--hh-duration-normal` (0.2s) and
  `--hh-easing-standard` (ease-out), mirrored in `HHTheme`, `Brand~/tokens.json` (`motion`) and
  the Godot constants, with `Tools~/check-tokens.mjs` checking the new group. Buttons and inputs
  ease their colour changes, and the progress fill eases its width.
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
