# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

Each item names its check. Unity items go through `Tools~/unity-compile.sh`, which compiles every
asmdef with Unity 6000.0.85f2's own compiler and source generators; it doesn't import USS or UXML,
since the editor won't open a project without a licence. "Tests written" items are also held by `check.mjs`'s reference check,
which fails on a control with no EditMode test.

1. Tabs: style Unity 6's `TabView` and `Tab` (header, selected, hover, focus) as `.hh-tabs`.
   Check: contrast pairs for selected and unselected tab text, gate.
2. Scroll view and list view styling: scrollers, rows, hover and selection. Check: contrast pairs
   for selected-row text, gate.
3. The dropdown's open list (`.unity-base-dropdown`), styled from the panel root by the theme
   sheet. Check: contrast pairs, gate.
4. `HHTheme` reads tokens from a resolved element at runtime, so the hand-kept mirror and its
   check can go. Check: tests written, gate.
5. Light and high-contrast themes: `hh-tokens-light.uss`, `hh-tokens-high-contrast.uss` and a
   theme sheet for each; the contrast check runs every pair against every theme. Check:
   `check.mjs` over all three, gate.
6. Runtime theme switching: `HHThemes.Apply(PanelSettings, ThemeStyleSheet)` and a sample button
   that cycles the three themes. Check: tests written, gate.
7. Component gallery: an editor window (Hooton Heroics > Design System > Gallery) showing every
   control in each state under the chosen theme. Check: gate compiles it; extend `check.mjs`'s
   reference check to fail on a control missing from it.
8. Settings screen sample: a second sample building a real settings menu from the toggle, slider,
   dropdown, tabs and dialog, serving the "menus, settings and HUD" Done-when line. Check: gate
   compiles it; extend the reference check to fail on a form control it doesn't use.
9. Component reference: `Documentation~/components.md` with each control's UXML, classes, states
   and tokens. Check: extend the reference check to fail on a control missing from it.

## Blocked on owner

- A transparent studio mark. `Brand~/hh-logo.png` (and the website's copy) is opaque on a ground darker than `color.bg`, so a faint square shows around it on the splash. Needs the owner's art: a cut-out of the mark, or a decision to match the field to it.
- Confirm GOAL.md.
- Open v0.2 in Unity 6: it compiles, the Basic UI sample runs, `hh-theme.tss` finds its two
  stylesheets, `HHProgressBar` takes `value` and `title` from UXML. Nothing here has run in Unity.
- Tag `v0.2.0` once it opens cleanly; tags wait on an ask.
- Button contrast, from the 0.1 palette: white on `violet` is 4.0:1 and on the `violet-light`
  hover 2.5:1, both under AA's 4.5:1 (accepted in `Tools~/contrast.json` until decided). Options:
  a darker button violet (`#754EE6`, 4.8:1) with a darker hover (`#6E4AD9`, 5.2:1); or dark text
  (`bg`) on `violet-light` buttons, 7.0:1. Recommended: the darker violets, which keep the look.
- A Unity licence for batch mode, so the gate can also import USS, TSS and UXML and run the tests:
  on Linux, 6000.0 exits 198 ("No valid Unity Editor license found") before importing anything.
  The house standards' route is a Unity account made for automation, activated through `op run`.
- Run the EditMode tests (`Tests/Editor`) in Unity 6: none has run, since batch mode refuses tests
  without a licence. Also decide whether they run through unity-packages' unity-verify, as other
  Unity repos do.
- The open-core split: which controls stay MIT and which go in the paid Pro pack.
- Publishing: an OpenUPM listing, then the Asset Store.
- Captures of the component gallery and both samples in each theme, from an editor with graphics.

## Ideas

- A shared compile smoke in unity-packages that takes any package path, so this repo's gate and
  `tools/smoke-artifacts.sh` become one tool.

## Done

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
