# Theming

Every colour, size and duration in the design system is a `--hh-*` token, so a theme is one file
of token values plus a theme sheet that loads it. This page makes one, then lets players switch.

## 1. Copy the tokens

Copy `Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-tokens.uss` into your project, for
example `Assets/UI/Themes/tokens-ember.uss`, and change the values. Keep every token, since every
control reads them; change only the values. Delete the copy's first comment, which describes the
package's own file. A warmer palette might start:

```uss
:root {
    --hh-color-bg: #1A0F0C;
    --hh-color-surface: #2A1712;
    --hh-color-violet: #E0603A;
    --hh-color-violet-light: #F2A07E;
    --hh-color-button: #B8442A;
    --hh-color-button-hover: #9C3722;
    --hh-color-violet-ghost: rgba(224, 96, 58, 0.14);
    --hh-color-violet-ghost-hover: rgba(224, 96, 58, 0.22);
}
```

Every token not shown keeps its default value: review `track`, `card-edge`, `focus`, `teal`, `pink`
and `peach` too, or they stay the default blues and purples. `HHTheme`, the C# mirror of the palette,
stays at the default values, so code reading it won't follow your theme; `HHTokens` reads the
resolved values instead.

The token names keep their roles, not their hues: `--hh-color-violet` is the accent, whatever colour
you give it.

## 2. Write the theme sheet

Create `Assets/UI/Themes/theme-ember.tss` next to it. It loads Unity's default control styles, your
tokens, then the package's component rules, in that order. The package's sheets are imported by
their project path; adjust it if the package is embedded or installed under another name:

```uss
@import url("unity-theme://default");
@import url("tokens-ember.uss");
@import url("/Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-components.uss");
@import url("/Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-dropdown-menu.uss");
```

Set a Panel Settings asset's **Theme Style Sheet** to it, and every control on that panel takes
the new values.

## 3. Check the contrast

`Tools~/contrast.json` lists every pair of colours the controls draw: `text` on `on`, needing
4.5:1 unless the pair's `min` says otherwise (3:1 for edges, rings, fills and icons, as WCAG AA
allows). Check your values against each pair with any contrast checker. A pair with `over` sits on a
translucent colour, such as `violet-ghost`: blend that colour onto `over` first, then measure. The
default values themselves fall short on three pairs (button text, and the empty track on a card),
which wait on a palette choice, so check those too. A theme added to the package itself (as `Runtime/Styles/hh-tokens-<name>.uss`
with a `hh-theme-<name>.tss` that imports it) is checked by `node Tools~/check.mjs`, which fails on
any pair below its minimum, a missing or extra token, or a missing theme sheet.

## 4. Let players switch

`HHThemes` swaps a panel's theme sheet. Give it your themes in order and cycle with `Next`:

```csharp
using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

public class ThemePicker : MonoBehaviour
{
    public PanelSettings panel;
    public ThemeStyleSheet[] themes;

    public void NextTheme()
    {
        HHThemes.Next(panel, themes);
    }
}
```

`HHThemes` changes the Panel Settings asset itself, so in the editor the change outlives Play mode;
the Basic UI sample cycles themes on a runtime copy instead. Accessibility settings (reduced motion,
large text) are classes on top of any theme, set with `HHAccessibility`; see
[getting-started.md](getting-started.md).
