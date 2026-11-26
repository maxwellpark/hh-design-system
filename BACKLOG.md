# Backlog

Goal: GOAL.md. Decisions: `Documentation~/decisions.md`. Acceptance packs: `Documentation~/acceptance/`.

## Ready

1. Contrast check: `Tools~/check.mjs` fails when a text token misses WCAG AA (4.5:1) on `bg` or
   `surface`, with alpha blended onto the surface.
2. EditMode tests in `Tests/Editor` for `HHButton` (class, ghost toggle) and `HHProgressBar`
   (class, value and title from UXML).
3. Form controls with hover, pressed, focus and disabled states: `HHToggle`, `HHSlider`,
   `HHDropdown`, `HHTextField`.
4. Overlays and navigation: `HHDialog`, `HHTabs`, tooltip and toast.
5. Small parts: icon button, badge, scroll view and list styling.
6. Progress bar variants: colour modifiers, segmented, indeterminate.
7. Light and high-contrast themes, and a runtime theme switcher.
8. `HHTheme` reads tokens from the resolved stylesheet at runtime, so the mirror and its check go.
9. Font and motion tokens.
10. A component gallery editor window with live token editing.

## Blocked on owner

- Confirm GOAL.md.
- Open v0.2 in Unity 6: it compiles, the Basic UI sample runs, `hh-theme.tss` finds its two
  stylesheets, `HHProgressBar` takes `value` and `title` from UXML. Nothing here has run in Unity.
- Tag `v0.2.0` once it opens cleanly; tags wait on an ask.
- Whether this repo's Unity tests run through unity-packages' unity-verify, as other Unity repos do.
- The open-core split: which controls stay MIT and which go in the paid Pro pack.
- Publishing: an OpenUPM listing, then the Asset Store.

## Ideas

## Done

- 2026-11-26: Unity 6 and `UxmlElement`, `HHProgressBar`, `hh-theme.tss`, ghost hover fix, focus
  and disabled states, committed `.meta` files, `HHTheme` mirroring every token with a check and
  its test (branch `max/unity6-progress-bar`).
