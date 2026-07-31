using UnityEngine;

namespace HootonHeroics.DesignSystem
{
    /// Design tokens as a ScriptableObject. Mirrors hh-tokens.uss for C#/inspector use
    /// (e.g. driving non-UI-Toolkit colours from the same palette). Create via
    /// Assets > Create > Hooton Heroics > Design System > Theme.
    [CreateAssetMenu(menuName = "Hooton Heroics/Design System/Theme", fileName = "HHTheme")]
    public class HHTheme : ScriptableObject
    {
        [Header("Colours")]
        public Color background = new Color32(0x0E, 0x0F, 0x20, 0xFF);
        public Color surface = new Color32(0x19, 0x1B, 0x38, 0xFF);
        public Color violet = new Color32(0x82, 0x57, 0xFF, 0xFF);
        public Color violetLight = new Color32(0xA7, 0x8B, 0xFA, 0xFF);
        public Color teal = new Color32(0x45, 0xD6, 0xC4, 0xFF);
        public Color pink = new Color32(0xFF, 0x5C, 0x8A, 0xFF);
        public Color peach = new Color32(0xFF, 0xB2, 0x5C, 0xFF);
        public Color text = new Color32(0xF3, 0xF3, 0xFB, 0xFF);

        [Header("Spacing (px)")]
        public float space2 = 8f;
        public float space4 = 16f;
        public float space6 = 24f;

        [Header("Radius (px)")]
        public float radiusMd = 12f;
        public float radiusLg = 20f;
    }
}
