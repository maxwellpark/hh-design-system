# Decisions

- Default 2026-11-26 [needs-review]: Unity 6 (6000.0) is the minimum. Why: `[UxmlElement]` replaced
  `UxmlFactory`, which Unity 6 deprecates. To change: keep
  both registration styles behind `#if UNITY_6000_0_OR_NEWER` and set `"unity"` back.
- Default 2026-11-26 [needs-review]: acceptance packs and this file live in `Documentation~/`,
  not `docs/`. Why: Unity imports every top-level folder of a package without a `~`, so `docs/`
  would need `.meta` files or warn in every project that installs it. To change: move them and
  add the `.meta` files.
- Default 2026-11-26 [needs-review]: `GOAL.md` and `BACKLOG.md` sit at the root with `.meta`
  files, so the backlog tooling finds them. Why: same import rule. To change: move them under
  `Documentation~/` and point the tooling there.
- Default 2026-11-26 [needs-review]: `HHTheme` mirrors the tokens and `Tools~/check.mjs` fails on
  drift. Why: reading USS variables from C# needs an element on a panel with its styles resolved,
  which a ScriptableObject used outside UI Toolkit doesn't have. To change: Ready item 8.
- Default 2026-11-26 [needs-review]: the progress fill is teal and the focus ring is a 2px teal
  border, transparent when unfocused. Why: teal is the palette's accent, and an always-present
  border means focus doesn't shift layout. To change: the `--hh-color-focus` token and
  `.hh-progress-bar .unity-progress-bar__progress`.
- Default 2026-11-26 [needs-review]: the package checks run by hand (`node Tools~/check.mjs`). Why:
  GitHub Actions is off on these repos. To change: add a workflow once Actions is on.
