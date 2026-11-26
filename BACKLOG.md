# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

Each item names its check. Unity items go through `Tools~/unity-compile.sh`, which compiles every
asmdef with Unity 6000.0.85f2's own compiler and source generators; it doesn't import USS or UXML,
since the editor won't open a project without a licence. "Tests written" items are also held by `check.mjs`'s reference check,
which fails on a control with no EditMode test.

1. Toast action: `HHToast` gains an optional ghost button that runs a callback and dismisses the
   toast, and `HHToastStack.Show` takes its label and callback as parameters after `seconds`, so
   existing calls compile; the gallery shows one. Check: EditMode tests written, "on a toast card"
   contrast pairs for the button, the reference, and the compile gate.
2. Disabled states in the gallery: a Disabled row builds every focusable control (button, icon
   button, toggle, slider, dropdown, text field, radio group, stepper, tabs, list) in a variable
   and calls `SetEnabled(false)` on it, and tabs and list gain the `:disabled` rule they lack.
   Check: `check.mjs` fails on a focusable control the gallery never disables (matched by the
   variable's type, not its name) or with no `:disabled` rule, with planted cases.

## Blocked on owner

- A transparent studio mark. `Brand~/hh-logo.png` (and the website's copy) is opaque on a ground darker than `color.bg`, so a faint square shows around it on the splash. Needs the owner's art: a cut-out of the mark, or a decision to match the field to it.
- Confirm GOAL.md.
- Open v0.2 in Unity 6: it compiles, the Basic UI sample runs, `hh-theme.tss` finds its two
  stylesheets, `HHProgressBar` takes `value` and `title` from UXML. Nothing here has run in Unity.
- Tag `v0.2.0` once it opens cleanly; tags wait on an ask.
- Button contrast, from the 0.1 palette: white on `--hh-color-button` is 4.0:1 and on
  `--hh-color-button-hover` 2.5:1, both under AA's 4.5:1 (accepted in `Tools~/contrast.json` until
  decided). Options: set `button` to `#754EE6` (4.8:1) and `button-hover` to `#6E4AD9` (5.2:1), or
  dark (`bg`) text on the current hover colour, 7.0:1. Recommended: the darker violets, which keep
  the look. Since the button tokens are their own, this no longer touches accent text or borders.
- A Unity licence for batch mode, so the gate can also import USS, TSS and UXML and run the tests:
  on Linux, 6000.0 exits 198 ("No valid Unity Editor license found") before importing anything.
  The house standards' route is a Unity account made for automation, activated through `op run`.
- Run the EditMode tests (`Tests/Editor`) in Unity 6: none has run, since batch mode refuses tests
  without a licence. Also decide whether they run through unity-packages' unity-verify, as other
  Unity repos do.
- The open-core split: which controls stay MIT and which go in the paid Pro pack.
- Publishing: an OpenUPM listing, then the Asset Store.
- A light theme's palette. Mechanically it's a `hh-tokens-light.uss` the contrast check holds to AA,
  but it inverts every colour role (accent text dark on white, button text dark or light), which is
  a brand choice. With the palette, the theme sheet and check are a short item.
- Drop `HHTheme`'s hand-kept mirror, as the runtime-token item first asked? `HHTokens` now reads
  resolved tokens, but `HHTheme` also serves code with no styled element (sprites, materials),
  so the mirror and its check were kept. Keep it, or drop `HHTheme`?
- Captures of the component gallery and both samples in each theme, from an editor with graphics.

## Ideas

- Changelog sections: group `[Unreleased]` under Added, Changed and Fixed before a release. It
  changes how every session adds an entry, so it waits until the owner plans a tag.
- A shared compile smoke in unity-packages that takes any package path, so this repo's gate and
  `tools/smoke-artifacts.sh` become one tool.

## Done

- 2026-11-26: placeholder text: `HHTextField`'s placeholder (`.unity-base-text-field__input--placeholder`, which the gate's class reader confirms) is `--hh-color-text-dim`, with a pair held in every theme; the gallery shows one (branch `max/placeholder`).
- 2026-11-26: scroller focus: Unity 6000.0's IL shows a scroller is a plain `Slider`, so it takes focus (recorded in `decisions.md`); a focused scroller's handle in a scroll view or list turns the focus colour, with a 3:1 pair, and the focus check holds `ScrollView`. The IL was read once by hand rather than by the gate, since the lasting check is the focus rule (branch `max/scroller-focus`).
- 2026-11-26: Settings sample shows the newer controls: a Resolution `HHStepper` on the Video tab and a Difficulty `HHRadioGroup` on the Player tab, in the saved settings; the sample check requires both (branch `max/settings-newer`).
- 2026-11-26: `HHStepper`: an ordered list stepped with arrows or left and right when focused, clamped with the end arrow disabled; tests, gallery, reference, four contrast pairs, and `BaseField` added to the focus check's focusable bases (branch `max/stepper`).
- 2026-11-26: sample README check: `check.mjs` fails on a listed sample with no `README.md`, or with a `*.cs` its README doesn't name; all four samples pass (branch `max/sample-readmes`).
- 2026-11-26: switch toggle: `HHToggle`'s `switch` attribute draws a sliding switch, its knob a child added in C# sliding on the duration tokens; tests, reference attribute, four 3:1 pairs, gallery on and off, and the Settings sample's Music (branch `max/switch-toggle`).
- 2026-11-26: getting started guide: `Documentation~/getting-started.md` goes from install to a themed card with a toggle and a button, then themes and accessibility; the gate compiles its two C# blocks and `check.mjs` checks its UXML (branch `max/getting-started`).
- 2026-11-26: focus rule check: `check.mjs` fails on a control built on a focusable Unity base (from a table) with no `:focus` rule on it or a part; `HHListView`, the one without, gains a focus ring (branch `max/focus-rules`).
- 2026-11-26: doc snippet gate: `unity-compile.mjs` compiles each fenced `csharp` block in `Documentation~/*.md` as a sample-like assembly, errors at the guide's line; `check.mjs` fails on guide UXML naming an `hh:` element or attribute the reference doesn't have (branch `max/doc-snippets`).
- 2026-11-26: slider value: `HHSlider` writes its value beside the track with `show-value`, as a percentage (from a table, so dragging allocates nothing) or a number with `value-format`; tests, reference section and a contrast pair (branch `max/slider-value`).
- 2026-11-26: pause menu sample: `Samples~/PauseMenu/PauseMenu.cs`, an `HHCard` of Resume, Settings and Quit, Quit asking through a `require-choice` dialog, opened by its own Pause button so it needs neither input system; `check.mjs` holds its controls (branch `max/pause-menu`).
- 2026-11-26: accessibility modifiers: `.hh-reduced-motion` (every duration 0s) and `.hh-large-text` (every font size 1.25 times, as pixels) override tokens under any theme; `HHAccessibility` toggles them, the Settings sample has an Access tab and the gallery a toggle for each; `check.mjs` holds both to the tokens (branch `max/a11y-modifiers`).
- 2026-11-26: `HHCard`: the card as a control with a `title` attribute and a body; the gallery and the Basic UI and Settings samples use it instead of hand-built cards (branch `max/card`).
- 2026-11-26: `HHRadioGroup`: one choice of several as a segmented row, the checked choice teal with bg text; tests, gallery, reference and six contrast pairs (branch `max/radio-group`).
- 2026-11-26: dialog focus trap: while an `HHDialog` is open, Tab past its last stop wraps to the first and Shift+Tab before the first wraps to the last; focus that leaves by any other route comes back on the next tick (branch `max/dialog-focus`).
- 2026-11-26: non-text contrast: every state-bearing part the audit found has a 3:1 pair in `Tools~/contrast.json`, with new pairs for the slider's focus ring and the dropdown arrow; the one new failure, the empty track on a card at 1.39:1, is accepted and waits on the owner's palette choice (branch `max/non-text-contrast`).
- 2026-11-26: HUD sample: `Samples~/HUD/HudOverlay.cs`, a health bar, lives as a segmented bar, a bag button with a badge and toasts, driven by its own buttons so it needs neither input system; `check.mjs` fails if it stops using any of them (branch `max/hud-sample`).
- 2026-11-26: component reference: `Documentation~/components.md` gives each control and `HHTooltip` its UXML, classes, attributes, states and tokens; `check.mjs` fails on a missing section, class, attribute or token, a token that isn't defined, or a section for no control (branch `max/component-reference`).
- 2026-11-26: Settings sample: `Samples~/Settings/SettingsMenu.cs`, a settings screen of tabs, toggle, slider, dropdown and text field, Save with a toast and Back asking through a dialog before discarding; `check.mjs` fails if it stops using any of them (branch `max/settings-sample`).
- 2026-11-26: component gallery: `Editor/HHGallery.cs` (Hooton Heroics > Design System > Gallery) shows every control in its states under a picked token file; `check.mjs` fails on a control missing from it (branch `max/gallery`).
- 2026-11-26: runtime theme switching: `HHThemes.Apply` and `HHThemes.Next` set or cycle a PanelSettings' theme sheet; the Basic UI sample's Theme button cycles them on a runtime copy of its PanelSettings (branch `max/theme-switching`).
- 2026-11-26: high-contrast theme: `hh-tokens-high-contrast.uss` and `hh-theme-high-contrast.tss`; the contrast check runs every pair in every theme, and only the default theme may hold accepted exceptions (branch `max/high-contrast`).
- 2026-11-26: button tokens: `--hh-color-button` and `--hh-color-button-hover` split from `violet` and `violet-light`, at the same values, so a theme can set the button apart from accent text (branch `max/button-tokens`).
- 2026-11-26: `HHTokens`: read resolved token values (`TryGetColor`, `TryGetFloat`, `WhenResolved`) from a styled element at runtime (branch `max/runtime-tokens`).
- 2026-11-26: dropdown lists: `hh-dropdown-menu.uss`, imported by `hh-theme.tss`, styles Unity's `GenericDropdownMenu` (card, track hover, teal checkmark, separators) (branch `max/dropdown-menu`).
- 2026-11-26: Unity class names in the gate: `Tools~/unity-compile.mjs` reads every `*ussClassName` Unity 6000.0's UI Toolkit assembly assigns (`Tools~/unity-classes.cs`, Mono.Cecil) and fails on a `.unity-*` class a stylesheet styles that isn't one (branch `max/unity-class-check`).
- 2026-11-26: `HHScrollView` and `HHListView`: slim scrollers with a violet-light handle and no arrow buttons, rows with a track hover and a selection tint plus violet-light edge (branch `max/scroll-list`).
- 2026-11-26: `HHTabs`: Unity 6's `TabView` with dim unselected labels, white selected labels, a teal underline on the `:checked` tab, and headers made focusable with submit selecting the tab (branch `max/tabs`).
- 2026-11-26: `HHTooltip`: a manipulator that shows a label below its target after a 500ms hover or focus delay and hides on leave, blur, Escape or detach; `HHIconButton` carries one for its label (branch `max/tooltip`).
- 2026-11-26: `HHToast` and `HHToastStack`: cards with info, success, warning and danger edges, stacked in a corner, newest last, capped by `max-visible`, each dismissing itself after its time (branch `max/toast`).
- 2026-11-26: `HHDialog`: modal card on a `--hh-color-scrim` scrim, title, UXML body, `AddAction`; Escape, gamepad cancel and scrim clicks close it unless `require-choice`; focus moves to the first action and back on close (branch `max/dialog`).
- 2026-11-26: stylesheet check: `Tools~/check.mjs` fails on a stylesheet that doesn't parse as flat rules (a missing or stray brace, a declaration without a colon or semicolon) or uses a property Unity 6000.0 doesn't know (branch `max/stylesheet-check`).
- 2026-11-26: progress bar tones (violet, pink, peach; teal stays the default) and `HHSegmentedBar` (1 to 50 segments, filled count, tones) (branch `max/progress-variants`).
- 2026-11-26: first compile of `main` (057e229): every assembly compiles clean, so nothing to fix.
- 2026-11-26: Unity compile gate `Tools~/unity-compile.sh`: every asmdef compiled as Unity would with 6000.0.85f2's own compiler and UI Toolkit generators, editor and player builds, in `Tools~/verify.sh`, with ten planted failures in `Tools~/unity-compile.test.sh` (branch `max/unity-compile-gate`).
- 2026-11-26: `HHIconButton` (icon as tinted background, label as tooltip) and `HHBadge` (count capped at 99+, dot, four tones with dark text), with `HHTone` (branch `max/icon-button-badge`).
- 2026-11-26: motion tokens (`--hh-duration-fast`, `-normal`, `--hh-easing-standard`) on buttons, inputs and the progress fill; no font-weight token, since USS has none (branch `max/motion-tokens`).
- 2026-11-26: reference check: undefined tokens, classes without rules and controls without tests fail `Tools~/check.mjs` (branch `max/reference-check`).
- 2026-11-26: engine-neutral brand kit in `Brand~/` (tokens mirror, studio mark, splash, logo and credits rules), the Godot studio splash in `Godot~/addons/hh_studio` with its headless tests, and `Tools~/check-tokens.mjs` (branch `max/brand-kit`).
- 2026-11-26: `HHToggle`, `HHSlider`, `HHDropdown` and `HHTextField` with label, hover, focus,
  checked and disabled styles, a `--hh-color-track` token, and their tests (branch
  `max/form-controls`).
- 2026-11-26: EditMode tests for the controls' classes, the ghost toggle, UXML attributes and
  `HHStyles.Apply` (branch `max/editmode-tests`); written, not yet run.
- 2026-11-26: WCAG AA contrast check over `Tools~/contrast.json`; ghost hover tint to 22% and the
  progress title above the bar to pass it (branch `max/contrast-check`).
- 2026-11-26: Unity 6 and `UxmlElement`, `HHProgressBar`, `hh-theme.tss`, ghost hover fix, focus
  and disabled states, committed `.meta` files, `HHTheme` mirroring every token with a check and
  its test (branch `max/unity6-progress-bar`).
