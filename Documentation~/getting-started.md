# Getting started

Build a small themed screen: a card with a toggle and a button, styled by the design system,
switchable to high contrast and large text. It takes about ten minutes in Unity 6.

## 1. Install

Package Manager, **+**, **Add package from git URL**:

```
https://github.com/maxwellpark/hh-design-system.git
```

Once a release is tagged, pin it by adding the tag to the URL, for example `#v0.2.0`.

## 2. Give a panel the theme

Create a **Panel Settings** asset (Assets > Create > UI Toolkit > Panel Settings Asset) and set its
**Theme Style Sheet** to `Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-theme.tss`.
The theme brings Unity's default control styles, the tokens and the components, for every document
on that panel. `hh-theme-high-contrast.tss` is the same with the high-contrast tokens.

## 3. Lay out the screen in UXML

Create a UXML file named `Menu.uxml` (Assets > Create > UI Toolkit > UI Document) and replace its
contents with:

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

Add a GameObject with a **UIDocument** component. Set its **Panel Settings** to the asset from step 2
and its **Source Asset** to `Menu.uxml`. Then add a script named `Menu.cs` (the file name must match
the class) with this code:

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

Press Play: a dark card titled Northwind, a Music toggle and a violet Play button. Click Play and
open the Console window to see the message.

## 5. Let players change the look

Themes swap the panel's sheet; accessibility settings are classes on the document root, which
work under any theme. Add this script to the same GameObject, drag the UIDocument into its
**Document** field and `hh-theme-high-contrast.tss` into **High Contrast**. It adds three buttons to
the card:

```csharp
using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuOptions : MonoBehaviour
{
    public UIDocument document;
    public ThemeStyleSheet highContrast;

    private void OnEnable()
    {
        var root = document.rootVisualElement;
        var card = root.Q<HHCard>();
        card.Add(new HHButton(() => HHThemes.Apply(document.panelSettings, highContrast)) { text = "High contrast", Ghost = true });
        card.Add(new HHButton(() => HHAccessibility.SetLargeText(root, true)) { text = "Large text", Ghost = true });
        card.Add(new HHButton(() => HHAccessibility.SetReducedMotion(root, true)) { text = "Reduce motion", Ghost = true });
    }
}
```

`HHThemes.Apply` changes the Panel Settings asset itself, so in the editor the change outlives Play
mode; the Basic UI sample cycles themes on a runtime copy instead.

## Next

- The **Settings**, **HUD** and **Pause Menu** samples, from the Package Manager, are fuller screens.
- **Hooton Heroics > Design System > Gallery** shows every control in its states.
- [components.md](components.md) lists each control's UXML, classes, attributes, states and tokens.

The gate compiles every C# block on this page, and `Tools~/check.mjs` checks every `hh:` element
and attribute in its UXML against the component reference, so what you copy compiles against this
version.
