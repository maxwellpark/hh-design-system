# Hooton Heroics Design System

A small, themeable **UI Toolkit** design system for Unity: design tokens (USS variables), a
ScriptableObject theme, and ready-to-use components. Change a token, everything follows.
Open-core, MIT. Part of a family of Hooton Heroics Unity packages (`com.hootonheroics.*`).

## Install (UPM via Git URL)
Package Manager → **+** → **Add package from git URL**:
```
https://github.com/maxwellpark/hh-design-system.git
```
Pin a version with a tag: `...hh-design-system.git#v0.1.0`.

## Use
1. Add a **UIDocument** to a GameObject (with PanelSettings).
2. Add the two stylesheets to the document (or call `HHStyles.Apply(root, tokens, components)`):
   - `Runtime/Styles/hh-tokens.uss` (the tokens)
   - `Runtime/Styles/hh-components.uss` (the components)
3. Use the classes / components:
```xml
<ui:VisualElement class="hh-card">
  <ui:Label text="Hooton Heroics" class="hh-title" />
  <hh:HHButton text="Play" />
</ui:VisualElement>
```
Import the **Basic UI** sample from the Package Manager for a working demo.

## Theming
All components are styled through `var(--hh-*)` tokens in `hh-tokens.uss`, so restyling is a
one-file change. `HHTheme` (a ScriptableObject) mirrors the palette for driving non-UI-Toolkit
colours from the same source. Create via **Assets → Create → Hooton Heroics → Design System → Theme**.

## What's in v0.1
Tokens (colour, spacing, radius, type) · `.hh-card` / `.hh-title` / `.hh-text[-dim]` / `.hh-button`
(+ ghost) · `HHButton` control · `HHTheme` asset · `HHStyles.Apply` helper · Basic UI sample.

## Notes
- Target: Unity **2022.3 LTS**+ (UI Toolkit runtime).
- `.meta` files: open once in Unity and commit the generated `.meta` for stable asset GUIDs.
- Roadmap: full component set, a theme-switcher, an editor preview window, OpenUPM + Asset Store,
  then a paid "Pro" pack (open-core). Siblings planned: `com.hootonheroics.ai`, `...procgen`.

## Brand kit and other engines

`Brand~/tokens.json` is the source for every Hooton Heroics game, whatever the engine; `Brand~/README.md` has the studio splash, logo and credits rules. Unity ignores folders ending in `~`, so these ride along without touching a Unity project.

- **Godot 4:** copy `Godot~/addons/hh_studio` into the game's `addons/` from a release tag, and record the tag in the game's `shared.json` as `"hh-design-system": "<tag>"`. Show the splash first:
  ```gdscript
  const HHSplash := preload("res://addons/hh_studio/hh_splash.gd")
  var splash := HHSplash.create(null)  # or a sized Russell studio-host texture
  add_child(splash)
  await splash.finished
  ```
- **Unity:** the tokens are `Runtime/Styles/hh-tokens.uss` and `HHTheme`. The splash component is planned and needs a Unity compile and play check before it ships.
- **Check:** `bash Tools~/verify.sh` (Node and a Godot 4.7 binary as `godot` or `GODOT`).

## Licence
MIT. Built by [Hooton Heroics](https://hootonheroics.com).
