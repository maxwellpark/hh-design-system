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
- Default 2026-11-26 [needs-review]: violet fills use `violet-light`. Why: plain violet is 2.7:1 on
  the track, under the 3:1 a fill needs. To change: the `--violet` rules.
- Default 2026-11-26 [needs-review]: empty segments are `track` on the card, 1.4:1, so a bar's
  length reads from the gaps rather than the empty segments' edges. Why: filled segments, which
  carry the value, are 4.1:1 or more against empty ones. To change: give empty segments a
  `violet-light` border, which the contrast check would then hold to 3:1.
- Default 2026-11-26 [needs-review]: a badge sits beside or over a control by the game's own
  layout; nothing positions it inside a button. Why: a Label inside a Button overlaps the button's
  text, which isn't a layout child. To change: add a corner-anchored `.hh-badge--corner` rule.
- Default 2026-11-26 [needs-review]: toast tones show as a 4px left edge on a `surface` card, not
  a filled background. Why: text stays white on `surface` (15:1) for every tone, and each edge is
  5.7:1 or more against the card. To change: the `.hh-toast--*` rules.
- Default 2026-11-26 [needs-review]: four toasts show at most, each for 4 seconds. Why: enough to
  read a short line without covering the game. To change: `MaxVisible` and `DefaultSeconds`.
- Default 2026-11-26 [needs-review]: the toast stack is at most 80% of its parent's height and clips
  beyond that. Why: a high `max-visible` shouldn't push cards off-screen. To change: `.hh-toast-stack`.
- Default 2026-11-26 [needs-review]: `HHDialog` doesn't trap Tab focus inside the card. Why: UI
  Toolkit has no focus scope, and the scrim already blocks pointer input behind it. To change: a
  `FocusOutEvent` handler that returns focus to the first action when it leaves the card.
- Default 2026-11-26 [needs-review]: the scrim is `bg` at 72%. Why: dark enough to set the card
  apart from a busy game scene behind it. To change: `--hh-color-scrim` and its mirrors.
- Default 2026-11-26 [needs-review]: Escape closes a dialog on its `KeyDownEvent`, and the
  `NavigationCancelEvent` Unity sends for the same key goes on to whatever regains focus. Why:
  harmless with one dialog open. To change: with stacked dialogs, ignore a cancel in the frame a
  key Escape was handled.
- Default 2026-11-26 [needs-review]: stylesheets are checked by a small parser in `check.mjs`, with
  the property list taken from strings in Unity 6000.0's `UnityEngine.UIElementsModule.dll`, plus
  `top` and `all`, which aren't among them. Why: the
  compile gate can't import USS without a licence, and a merge once dropped a brace that only a
  reviewer caught. To change: once batch mode is licensed, let the editor's importer judge instead.
  To refresh the list: `strings -e l UnityEngine.UIElementsModule.dll`, keep the property families.
- Default 2026-11-26 [needs-review]: tooltips draw as the last child of the target's document (its
  ancestor just under the panel root), 4px below the target's left edge, after 500ms. Why: the
  document carries the HH stylesheets when they're added per document rather than as a theme, and
  below-left reads naturally under buttons. To change: `HHTooltip.Show` and `DefaultDelayMs`; it doesn't yet flip near screen
  edges.
- Default 2026-11-26 [needs-review]: a selected list row has a 3px violet-light left edge as well
  as its tint. Why: the tint alone is 1.3:1 against the card; the edge is 6.2:1. To change: the
  `.unity-collection-view__item--selected` rule under `.hh-list-view`.
- Default 2026-11-26 [needs-review]: scrollers lose their arrow buttons and thin to 8px. Why: games
  scroll by wheel, drag or gamepad, and the arrows read as editor chrome. To change: the
  `.unity-scroller__low-button` and `__high-button` rules.
- Default 2026-11-26 [needs-review]: `HHTabs` styles Unity's own `TabView` and `Tab` rather than
  building tabs. Why: `TabView` already handles selection, reordering and closing; the class names
  and the `:checked` state were read from 6000.0's IL (`Tab.SetActive` sets pseudo-state 8). To
  change: a custom control, if `TabView`'s structure ever gets in the way.
- Default 2026-11-26 [needs-review]: `HHTabs` makes each tab header focusable and selects its tab on
  submit, Enter or Space. Why: Unity 6000.0's headers aren't focusable, so a menu built on them was
  mouse-only. To change: `HHTabs.Wire`.
- Default 2026-11-26 [needs-review]: dropdown list rules live in their own `hh-dropdown-menu.uss`,
  imported only by `hh-theme.tss`, and style every dropdown list on the panel. Why: Unity opens the
  list at the panel root, outside any document, and `DropdownField` offers no public way to tag the
  list it opens. To change: a custom dropdown that builds its own `GenericDropdownMenu`.
- Default 2026-11-26 [needs-review]: Unity's class names are read from the static constructors of
  `UnityEngine.UIElementsModule.dll` with Mono.Cecil, folding `ldstr`, `ldsfld` and `String.Concat`,
  rather than kept as a hand list. Why: they follow the editor version the gate runs, and a
  misspelt Unity part name otherwise styles nothing, silently. To change: `Tools~/unity-classes.cs`.
- Default 2026-11-26 [needs-review]: runtime token reading is a new `HHTokens`, and `HHTheme`'s mirror
  and its check stay. Why: the backlog item asked to drop the mirror, but `HHTheme` serves code
  with no styled element to read from (sprites, materials), which a resolved read can't. To change:
  remove `HHTheme` once nothing outside UI Toolkit needs the palette.
