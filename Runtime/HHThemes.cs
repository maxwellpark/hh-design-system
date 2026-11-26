using System.Collections.Generic;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Switches a panel's theme sheet; the change is to the PanelSettings asset, so in the editor it outlives Play mode.
    public static class HHThemes
    {
        public static void Apply(PanelSettings panel, ThemeStyleSheet theme)
        {
            if (panel != null && theme != null)
            {
                panel.themeStyleSheet = theme;
            }
        }

        /// Moves to the theme after the current one, wrapping round; a panel on a theme not listed starts at the first.
        public static ThemeStyleSheet Next(PanelSettings panel, IReadOnlyList<ThemeStyleSheet> themes)
        {
            if (panel == null || themes == null || themes.Count == 0)
            {
                return null;
            }
            var index = -1;
            for (var i = 0; i < themes.Count; i++)
            {
                if (themes[i] != null && themes[i] == panel.themeStyleSheet)
                {
                    index = i;
                    break;
                }
            }
            // Null entries are skipped; a list of nothing but nulls changes nothing
            for (var step = 1; step <= themes.Count; step++)
            {
                var next = themes[(index + step) % themes.Count];
                if (next != null)
                {
                    Apply(panel, next);
                    return next;
                }
            }
            return null;
        }
    }
}
