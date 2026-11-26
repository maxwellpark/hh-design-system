# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

1. EditMode tests in `Tests/Editor` for `HHButton` (class, ghost toggle) and `HHProgressBar`
   (class, value and title from UXML).
2. Form controls with hover, pressed, focus and disabled states: `HHToggle`, `HHSlider`,
   `HHDropdown`, `HHTextField`.
3. Overlays and navigation: `HHDialog`, `HHTabs`, tooltip and toast.
4. Small parts: icon button, badge, scroll view and list styling.
5. Progress bar variants: colour modifiers, segmented, indeterminate.
6. Light and high-contrast themes, and a runtime theme switcher.
7. `HHTheme` reads tokens from the resolved stylesheet at runtime, so the mirror and its check go.
8. Font and motion tokens.
9. A component gallery editor window with live token editing.

## Blocked on owner

- Confirm GOAL.md.
- Open v0.2 in Unity 6: it compiles, the Basic UI sample runs, `hh-theme.tss` finds its two
  stylesheets, `HHProgressBar` takes `value` and `title` from UXML. Nothing here has run in Unity.
- Tag `v0.2.0` once it opens cleanly; tags wait on an ask.
- Button contrast, from the 0.1 palette: white on `violet` is 4.0:1 and on the `violet-light`
  hover 2.5:1, both under AA's 4.5:1 (accepted in `Tools~/contrast.json` until decided). Options:
  a darker button violet (`#754EE6`, 4.8:1) with a darker hover (`#6E4AD9`, 5.2:1); or dark text
  (`bg`) on `violet-light` buttons, 7.0:1. Recommended: the darker violets, which keep the look.
- Whether this repo's Unity tests run through unity-packages' unity-verify, as other Unity repos do.
- The open-core split: which controls stay MIT and which go in the paid Pro pack.
- Publishing: an OpenUPM listing, then the Asset Store.

## Ideas

## Done

- 2026-11-26: WCAG AA contrast check over `Tools~/contrast.json`; ghost hover tint to 22% and the
  progress title above the bar to pass it (branch `max/contrast-check`).
- 2026-11-26: Unity 6 and `UxmlElement`, `HHProgressBar`, `hh-theme.tss`, ghost hover fix, focus
  and disabled states, committed `.meta` files, `HHTheme` mirroring every token with a check and
  its test (branch `max/unity6-progress-bar`).
