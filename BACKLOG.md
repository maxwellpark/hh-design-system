# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

1. Overlays and navigation: `HHDialog`, `HHTabs`, tooltip and toast.
2. Small parts: icon button, badge, scroll view and list styling.
3. Progress bar variants: colour modifiers, segmented, indeterminate.
4. Light and high-contrast themes, and a runtime theme switcher.
5. `HHTheme` reads tokens from the resolved stylesheet at runtime, so the mirror and its check go.
6. Font and motion tokens.
7. A component gallery editor window with live token editing.

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
- Run the EditMode tests (`Tests/Editor`, seven tests) in Unity 6: none has run, since batch mode
  refuses tests without a licence and there's no editor here. Also decide whether they run through
  unity-packages' unity-verify, as other Unity repos do.
- The open-core split: which controls stay MIT and which go in the paid Pro pack.
- Publishing: an OpenUPM listing, then the Asset Store.

## Ideas

- Style the dropdown's open list (`.unity-base-dropdown`), which opens at the panel root and
  keeps Unity's default look.

## Done

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
