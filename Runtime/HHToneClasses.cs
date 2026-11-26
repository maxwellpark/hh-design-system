using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Applies a tone as one `<block>--<tone>` class, removing the others; the default tone has none.
    internal static class HHToneClasses
    {
        public static void Apply(VisualElement element, string block, HHTone tone, HHTone defaultTone)
        {
            foreach (HHTone each in new[] { HHTone.Violet, HHTone.Teal, HHTone.Pink, HHTone.Peach })
            {
                element.EnableInClassList($"{block}--{each.ToString().ToLowerInvariant()}", each == tone && each != defaultTone);
            }
        }
    }
}
