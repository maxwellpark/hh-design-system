using UnityEditor;
using UnityEngine;

namespace HootonHeroics.DesignSystem.Editor
{
    internal static class HHDesignSystemMenu
    {
        [MenuItem("Hooton Heroics/Design System/Documentation")]
        private static void OpenDocs() =>
            Application.OpenURL("https://github.com/maxwellpark/hh-design-system");
    }
}
