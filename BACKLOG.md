# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

Each item names its check. Unity items are compiled by the compile gate (below) once it lands: batch mode
compiles without a licence (it refuses only to run tests), using the Unity 6000.0.85f2 editor
installed in the cloud session. "Tests written" items are also held by `check.mjs`'s reference check,
which fails on a control with no EditMode test.

1. Unity compile gate: `Tools~/unity-compile.sh` installs a `git archive` tarball of the package
   into a throwaway Unity 6000.0 project, with the package in `testables` and the Basic UI sample
   copied into `Assets`, compiles in batch mode, and fails on a compile error, a missing or
   uncommitted `.meta`, or a USS, TSS or UXML import error. Check: exits 0 on main, and fails on a
   planted compile error and a planted meta-less file.
2. Fix each compile error and import warning the first gate run of `main` reports, one commit per
   cause; if it reports none, this item closes with the clean run as its evidence. Check: the compile
   gate exits 0 on `main`.
3. Font and motion tokens: `--hh-font-weight-*`, `--hh-duration-fast` and `-normal`, and an easing
   token, replacing hard-coded transition times. Check: `check.mjs` (mirror and references), gate.
4. Icon button and badge: `HHIconButton` (square, icon-only, needs an accessible name via
   `tooltip`) and `HHBadge` (count or dot, tones). Check: contrast pairs, EditMode tests written,
   gate.
5. Progress bar variants: tone modifiers (teal, pink, peach) and `HHSegmentedBar` for health and
   resource bars. Check: contrast pairs for each tone on the track, tests written, gate.
6. `HHDialog`: a modal with scrim, title, body and actions; Escape and the scrim close it unless
   it's set to require a choice; focus moves to its first action and returns on close. Check: tests
   written for open, close and focus; gate.
7. `HHToast`: timed notifications stacked in a corner, with info, success, warning and danger
   tones; a newer one pushes older ones up and each removes itself. Check: tone contrast pairs,
   tests written, gate.
8. Tooltip: an `HHTooltip` manipulator that shows a label after a hover or focus delay and hides on
   leave, blur or Escape. Check: tests written, gate.
9. Tabs: style Unity 6's `TabView` and `Tab` (header, selected, hover, focus) as `.hh-tabs`.
   Check: contrast pairs for selected and unselected tab text, gate.
10. Scroll view and list view styling: scrollers, rows, hover and selection. Check: contrast pairs
    for selected-row text, gate.
11. The dropdown's open list (`.unity-base-dropdown`), styled from the panel root by the theme
    sheet. Check: contrast pairs, gate.
12. `HHTheme` reads tokens from a resolved element at runtime, so the hand-kept mirror and its
    check can go. Check: tests written, gate.
13. Light and high-contrast themes: `hh-tokens-light.uss`, `hh-tokens-high-contrast.uss` and a
    theme sheet for each; the contrast check runs every pair against every theme. Check:
    `check.mjs` over all three, gate.
14. Runtime theme switching: `HHThemes.Apply(PanelSettings, ThemeStyleSheet)` and a sample button
    that cycles the three themes. Check: tests written, gate.
15. Component gallery: an editor window (Hooton Heroics > Design System > Gallery) showing every
    control in each state under the chosen theme. Check: gate compiles it; extend `check.mjs`'s
    reference check to fail on a control missing from it.
16. Settings screen sample: a second sample building a real settings menu from the toggle, slider,
    dropdown, tabs and dialog, serving the "menus, settings and HUD" Done-when line. Check: gate
    compiles it; extend the reference check to fail on a form control it doesn't use.
17. Component reference: `Documentation~/components.md` with each control's UXML, classes, states
    and tokens. Check: extend the reference check to fail on a control missing from it.

## Blocked on owner

- Confirm GOAL.md.
- Open v0.2 in Unity 6: it compiles, the Basic UI sample runs, `hh-theme.tss` finds its two
  stylesheets, `HHProgressBar` takes `value` and `title` from UXML. Nothing here has run in Unity.
- Tag `v0.2.0` once it opens cleanly; tags wait on an ask.
- Button contrast, from the 0.1 palette: white on `violet` is 4.0:1 and on the `violet-light`
  hover 2.5:1, both under AA's 4.5:1 (accepted in `Tools~/contrast.json` until decided). Options:
  a darker button violet (`#754EE6`, 4.8:1) with a darker hover (`#6E4AD9`, 5.2:1); or dark text
  (`bg`) on `violet-light` buttons, 7.0:1. Recommended: the darker violets, which keep the look.
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

- 2026-11-26: reference check: undefined tokens, classes without rules and controls without tests fail `Tools~/check.mjs` (branch `max/reference-check`).
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
