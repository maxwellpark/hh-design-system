# Getting started

Build a small themed screen: a card with a toggle and a button, styled by the design system,
switchable to high contrast and large text. It takes about ten minutes in Unity 6.

Every C# block on this page is compiled by `Tools~/unity-compile.mjs`, and every `hh:` element and
attribute in its UXML is checked against [the component reference](components.md), so what you
copy from here compiles against this version.

## 1. Install

Package Manager, **+**, **Add package from git URL**:

```
https://github.com/maxwellpark/hh-design-system.git
```

Pin a release by adding its tag, for example `#v0.2.0`.

## 2. Give a panel the theme

Create a **Panel Settings** asset (Assets, Create, UI Toolkit, Panel Settings Asset) and set its
**Theme Style Sheet** to `Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-theme.tss`.
The theme brings Unity's default control styles, the tokens and the components, for every document
on that panel. `hh-theme-high-contrast.tss` is the same with the high-contrast tokens.

## 3. Lay out the screen in UXML

Create `Menu.uxml`:

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:hh="HootonHeroics.DesignSystem">
    <hh:HHCard title="Northwind" style="width: 320px;">
        <hh:HHToggle name="music" label="Music" value="true" />
        <hh:HHButton name="play" text="Play" />
    </hh:HHCard>
</ui:UXML>
```

Every `hh:` control and its attributes are in [the component reference](components.md).

## 4. Wire it up

Add a GameObject with a **UIDocument**, give it the Panel Settings and `Menu.uxml`, and add this
script:

```csharp
using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Menu : MonoBehaviour
{
    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var music = root.Q<HHToggle>("music");
        var play = root.Q<HHButton>("play");
        play.clicked += () => Debug.Log(music.value ? "Playing with music" : "Playing in silence");
    }
}
```

Press Play: a dark card titled Northwind, a Music toggle and a violet Play button.

## 5. Let players change the look

Themes swap the panel's sheet; accessibility settings are classes on the document root, which
work under any theme:

```csharp
using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuOptions : MonoBehaviour
{
    public UIDocument document;
    public ThemeStyleSheet highContrast;

    public void UseHighContrast()
    {
        HHThemes.Apply(document.panelSettings, highContrast);
    }

    public void SetAccess(bool reduceMotion, bool largeText)
    {
        HHAccessibility.SetReducedMotion(document.rootVisualElement, reduceMotion);
        HHAccessibility.SetLargeText(document.rootVisualElement, largeText);
    }
}
```

`HHThemes.Apply` changes the Panel Settings asset itself, so in the editor the change outlives Play
mode; the Basic UI sample cycles themes on a runtime copy instead.

## Next

- The **Settings**, **HUD** and **Pause Menu** samples, from the Package Manager, are fuller screens.
- **Hooton Heroics, Design System, Gallery** shows every control in its states.
- [components.md](components.md) lists each control's UXML, classes, attributes, states and tokens.
