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
                if (themes[i] == panel.themeStyleSheet)
                {
                    index = i;
                    break;
                }
            }
            var next = themes[(index + 1) % themes.Count];
            Apply(panel, next);
            return next;
        }
    }
}
