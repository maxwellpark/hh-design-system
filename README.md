# Hooton Heroics Design System

A small, themeable **UI Toolkit** design system for Unity: design tokens (USS variables), a
ScriptableObject theme, and ready-to-use components. Change a token, everything follows.
Open-core, MIT. Part of a family of Hooton Heroics Unity packages (`com.hootonheroics.*`).

## Install (UPM via Git URL)
Package Manager → **+** → **Add package from git URL**:
```
https://github.com/maxwellpark/hh-design-system.git
```
Pin a version with a tag: `...hh-design-system.git#v0.2.0`.

## Use
1. Add a **UIDocument** to a GameObject (with PanelSettings).
2. Give it the styles, either way:
   - **Theme (simplest):** set the PanelSettings' **Theme Style Sheet** to
     `Runtime/Styles/hh-theme.tss`. It brings Unity's default control styles plus the tokens
     and components, for every document on that panel.
   - **Per document:** add `Runtime/Styles/hh-tokens.uss` and `Runtime/Styles/hh-components.uss`
     to the document, or call `HHStyles.Apply(root, tokens, components)`.
3. Use the classes / components:
```xml
<ui:VisualElement class="hh-card">
  <ui:Label text="Hooton Heroics" class="hh-title" />
  <hh:HHProgressBar title="Loading" value="40" />
  <hh:HHButton text="Play" />
  <hh:HHButton text="Back" ghost="true" />
</ui:VisualElement>
```
Import the **Basic UI** sample from the Package Manager for a working demo.

## Theming
All components are styled through `var(--hh-*)` tokens in `hh-tokens.uss`, so restyling is a
one-file change. `HHTheme` (a ScriptableObject) mirrors the palette for driving non-UI-Toolkit
colours from the same source. Create via **Assets → Create → Hooton Heroics → Design System → Theme**.

## What's in v0.2
Tokens (colour, spacing, radius, type) · `hh-theme.tss` · `.hh-card` / `.hh-title` /
`.hh-text[-dim]` / `.hh-button` (+ ghost, focus, disabled) / `.hh-progress-bar` and the form
controls · `HHButton`, `HHProgressBar`, `HHToggle`, `HHSlider`, `HHDropdown` and `HHTextField` ·
`HHTheme` asset · `HHStyles.Apply` helper · Basic UI sample.

## Notes
- Target: Unity **6** (6000.0)+. Controls use `[UxmlElement]`, which replaced `UxmlFactory`.
- Every imported file needs a committed `.meta`: a package installed from git is read-only, so
  Unity skips any asset without one. When adding a file, open the package embedded under
  `Packages/` and commit the `.meta` Unity generates.
- `node Tools~/check.mjs` checks the `.meta` files and that `HHTheme`'s defaults still match
  `hh-tokens.uss`. Run it before every commit; change a token in both places.
  It also checks WCAG AA contrast for the pairs in `Tools~/contrast.json`; add a pair when a
  control puts text on a new colour. `node Tools~/check.test.mjs` tests the check itself.
- EditMode tests live in `Tests/Editor`. To run them in a project, add
  `"testables": ["com.hootonheroics.designsystem"]` to its `Packages/manifest.json`.
- Roadmap: full component set, a theme-switcher, an editor preview window, OpenUPM + Asset Store,
  then a paid "Pro" pack (open-core). Siblings planned: `com.hootonheroics.ai`, `...procgen`.

## Brand kit and other engines

`Runtime/Styles/hh-tokens.uss` is the source of the look; `Brand~/tokens.json` carries it to every other engine with the studio splash values, and `Brand~/README.md` has the studio splash, logo and credits rules. Unity ignores folders ending in `~`, so these ride along without touching a Unity project.

- **Godot 4:** copy `Godot~/addons/hh_studio` into the game's `addons/` from a release tag, and record the tag in the game's `shared.json` as `"hh-design-system": "<tag>"`. Show the splash first:
  ```gdscript
  const HHSplash := preload("res://addons/hh_studio/hh_splash.gd")
  var splash := HHSplash.create(null)  # or a sized Russell studio-host texture
  add_child(splash)
  await splash.finished
  ```
- **Unity:** the tokens are `Runtime/Styles/hh-tokens.uss` and `HHTheme`. The splash component is planned and needs a Unity compile and play check before it ships.
- **Check:** `bash Tools~/verify.sh` runs `Tools~/check.mjs`, `Tools~/check-tokens.mjs` and the Godot tests (Node and a Godot 4.7 binary as `godot` or `GODOT`).

## Licence
MIT. Built by [Hooton Heroics](https://hootonheroics.com).
