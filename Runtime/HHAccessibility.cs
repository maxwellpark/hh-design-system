using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Player accessibility settings as classes on an element, which override tokens for everything under it,
    /// under any theme. Put them on the document root to cover a whole screen.
    public static class HHAccessibility
    {
        /// Every duration token is 0s, so controls change state without animating.
        public const string ReducedMotionUssClassName = "hh-reduced-motion";

        /// Every font-size token is 1.25 times its default, rounded to a whole pixel.
        public const string LargeTextUssClassName = "hh-large-text";

        public static void SetReducedMotion(VisualElement root, bool on)
        {
            root?.EnableInClassList(ReducedMotionUssClassName, on);
        }

        public static void SetLargeText(VisualElement root, bool on)
        {
            root?.EnableInClassList(LargeTextUssClassName, on);
        }
    }
}
