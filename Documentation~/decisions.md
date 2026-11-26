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
  which a ScriptableObject used outside UI Toolkit doesn't have. To change: the backlog item for reading tokens at runtime.
- Default 2026-11-26 [needs-review]: the progress fill is teal and the focus ring is a 2px teal
  border, transparent when unfocused. Why: teal is the palette's accent, and an always-present
  border means focus doesn't shift layout. To change: the `--hh-color-focus` token and
  `.hh-progress-bar .unity-progress-bar__progress`.
- Default 2026-11-26 [needs-review]: the package checks run by hand (`node Tools~/check.mjs`). Why:
  GitHub Actions is off on these repos. To change: add a workflow once Actions is on.
- Default 2026-11-26 [needs-review]: tracks use a new `--hh-color-track` (#2E3166), and text
  fields and dropdowns use `bg` with a `violet-light` border. Why: tracks in `surface` vanished on
  a card, and a control's edge needs 3:1 against the card (a `track` border was 1.4:1). To change:
  the token and the input rules in `hh-components.uss`.
- Default 2026-11-26 [needs-review]: form-control selectors add `.unity-base-field` and `:enabled`.
  Why: Unity's default theme reaches its parts through longer selectors, which would otherwise win.
  To change: drop the extra class once UI Builder shows the plain `.hh-*` rules applying.
- Default 2026-11-26 [needs-review]: the progress title sits above the bar. Why: white on the teal
  fill is 1.6:1 and dark text fails on the track, so no single colour reads on both. To change:
  `.hh-progress-bar .unity-progress-bar__title-container` and its padding.
- Default 2026-11-26 [needs-review]: the ghost hover tint is 22%, not 28%. Why: 28% put its text at
  4.4:1. To change: `--hh-color-violet-ghost-hover` and `HHTheme.violetGhostHover`.
- Default 2026-11-26 [needs-review]: the 0.1 button colours are accepted exceptions in the
  contrast check rather than changed. Why: they are the brand palette, so changing them is the
  owner's call (Blocked on owner in BACKLOG.md). To change: drop the two `accepted` entries.
- Default 2026-11-26 [needs-review]: motion tokens only, no font tokens beyond sizes. Why: USS has
  no `font-weight`, and whether a `var()` can carry `-unity-font-style` keywords is unverified.
  To change: add `--hh-font-style-*` tokens once a capture shows the keyword resolving.
- Default 2026-11-26 [needs-review]: 0.12s for state changes, 0.2s for the progress fill, ease-out.
  Why: quick enough not to lag input, slow enough to read as motion. To change: the three tokens.
- Default 2026-11-26 [needs-review]: the compile gate calls Unity's bundled `csc` with its UI
  Toolkit source generators, not the editor. Why: in a cloud session the editor exits 198 without
  a licence before importing anything, while its compiler needs none. To change: once batch mode is
  licensed, open a throwaway project instead, which also imports USS and UXML.
- Default 2026-11-26 [needs-review]: warnings are errors in the gate. Why: deprecated Unity APIs
  only warn, and the move off `UxmlFactory` showed they matter. To change: drop `-warnaserror`.
- Default 2026-11-26 [needs-review]: the gate compiles against 6000.0.85f2, the package's minimum
  line. Why: an API added after 6000.0 should fail here before a game on 6000.0 meets it. To
  change: `HH_UNITY_EDITOR`.
- Default 2026-11-26 [needs-review]: the gate builds Unity's test runner from the editor's bundled
  test-framework source each run, rather than skipping test assemblies' runner references. Why: a
  test using `[UnityTest]` or `IPrebuildSetup` should compile here exactly when it would in Unity.
  To change: drop the two `addAssembly` calls for the runner, at the cost of that coverage.
- Default 2026-11-26 [needs-review]: badges use dark (`bg`) text on every tone, and violet badges
  use `violet-light`. Why: white is 1.6 to 2.7:1 on the tones, dark is 6.4:1 or more. To change:
  the `.hh-badge` rules.
- Default 2026-11-26 [needs-review]: `HHIconButton` sets its icon as the element's tinted
  background, not through `Button.iconImage`. Why: one tint rule colours any icon, and it works the
  same in UXML through the `icon` attribute. To change: `HHIconButton.Icon`.
- Default 2026-11-26 [needs-review]: a badge sits beside or over a control by the game's own
  layout; nothing positions it inside a button. Why: a Label inside a Button overlaps the button's
  text, which isn't a layout child. To change: add a corner-anchored `.hh-badge--corner` rule.
