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
`.hh-text[-dim]` / `.hh-button` (+ ghost, focus, disabled) / `.hh-progress-bar` · `HHButton` and
`HHProgressBar` controls · `HHTheme` asset · `HHStyles.Apply` helper · Basic UI sample.

## Notes
- Target: Unity **6** (6000.0)+. Controls use `[UxmlElement]`, which replaced `UxmlFactory`.
- Every imported file needs a committed `.meta`: a package installed from git is read-only, so
  Unity skips any asset without one. When adding a file, open the package embedded under
  `Packages/` and commit the `.meta` Unity generates.
- `node Tools~/check.mjs` checks the `.meta` files and that `HHTheme`'s defaults still match
  `hh-tokens.uss`. Run it before every commit; change a token in both places.
  `node Tools~/check.test.mjs` tests the check itself.
- Roadmap: full component set, a theme-switcher, an editor preview window, OpenUPM + Asset Store,
  then a paid "Pro" pack (open-core). Siblings planned: `com.hootonheroics.ai`, `...procgen`.

## Licence
MIT. Built by [Hooton Heroics](https://hootonheroics.com).
