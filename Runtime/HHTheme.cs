using UnityEngine;

namespace HootonHeroics.DesignSystem
{
    /// The tokens for non-UI-Toolkit use; defaults mirror hh-tokens.uss, enforced by Tools~/check.mjs.
    [CreateAssetMenu(menuName = "Hooton Heroics/Design System/Theme", fileName = "HHTheme")]
    public class HHTheme : ScriptableObject
    {
        [Header("Colours")]
        public Color background = new Color32(0x0E, 0x0F, 0x20, 0xFF);
        public Color surface = new Color32(0x19, 0x1B, 0x38, 0xFF);
        public Color track = new Color32(0x2E, 0x31, 0x66, 0xFF);
        public Color violet = new Color32(0x82, 0x57, 0xFF, 0xFF);
        public Color violetLight = new Color32(0xA7, 0x8B, 0xFA, 0xFF);
        public Color violetGhost = new Color32(0x82, 0x57, 0xFF, 0x24);
        public Color violetGhostHover = new Color32(0x82, 0x57, 0xFF, 0x38);
        public Color teal = new Color32(0x45, 0xD6, 0xC4, 0xFF);
        public Color pink = new Color32(0xFF, 0x5C, 0x8A, 0xFF);
        public Color peach = new Color32(0xFF, 0xB2, 0x5C, 0xFF);
        public Color text = new Color32(0xF3, 0xF3, 0xFB, 0xFF);
        public Color textDim = new Color32(0xF3, 0xF3, 0xFB, 0x9E);
        public Color focus = new Color32(0x45, 0xD6, 0xC4, 0xFF);

        [Header("Spacing (px)")]
        public float space1 = 4f;
        public float space2 = 8f;
        public float space3 = 12f;
        public float space4 = 16f;
        public float space6 = 24f;

        [Header("Radius (px)")]
        public float radiusSm = 6f;
        public float radiusMd = 12f;
        public float radiusLg = 20f;

        [Header("Font size (px)")]
        public float fontSizeSm = 12f;
        public float fontSizeMd = 14f;
        public float fontSizeLg = 18f;
        public float fontSizeXl = 28f;

        [Header("Motion")]
        public float durationFast = 0.12f;
        public float durationNormal = 0.2f;
        public string easingStandard = "ease-out";
    }
}
