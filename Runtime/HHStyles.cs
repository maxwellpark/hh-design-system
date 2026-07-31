using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Apply the design-system stylesheets (tokens + components) to a root element.
    /// Pass the USS assets from the package (reference them in the inspector or load them).
    public static class HHStyles
    {
        public static void Apply(VisualElement root, StyleSheet tokens, StyleSheet components)
        {
            if (root == null) return;
            if (tokens != null && !root.styleSheets.Contains(tokens)) root.styleSheets.Add(tokens);
            if (components != null && !root.styleSheets.Contains(components)) root.styleSheets.Add(components);
        }
    }
}
