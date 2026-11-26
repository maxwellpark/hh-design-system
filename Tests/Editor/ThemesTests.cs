using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class ThemesTests
    {
        private PanelSettings panel;
        private ThemeStyleSheet dark;
        private ThemeStyleSheet contrast;

        [SetUp]
        public void Create()
        {
            panel = ScriptableObject.CreateInstance<PanelSettings>();
            dark = ScriptableObject.CreateInstance<ThemeStyleSheet>();
            contrast = ScriptableObject.CreateInstance<ThemeStyleSheet>();
        }

        [TearDown]
        public void Destroy()
        {
            Object.DestroyImmediate(panel);
            Object.DestroyImmediate(dark);
            Object.DestroyImmediate(contrast);
        }

        [Test]
        public void ApplySetsTheThemeAndIgnoresNulls()
        {
            HHThemes.Apply(panel, dark);
            Assert.That(panel.themeStyleSheet, Is.SameAs(dark));

            HHThemes.Apply(panel, null);
            HHThemes.Apply(null, contrast);
            Assert.That(panel.themeStyleSheet, Is.SameAs(dark));
        }

        [Test]
        public void NextCyclesAndStartsAtTheFirstForAnUnlistedTheme()
        {
            var themes = new[] { dark, contrast };

            Assert.That(HHThemes.Next(panel, themes), Is.SameAs(dark));
            Assert.That(HHThemes.Next(panel, themes), Is.SameAs(contrast));
            Assert.That(HHThemes.Next(panel, themes), Is.SameAs(dark));
            Assert.That(HHThemes.Next(panel, new ThemeStyleSheet[0]), Is.Null);
            Assert.That(HHThemes.Next(panel, new ThemeStyleSheet[] { null, contrast }), Is.SameAs(contrast));
            Assert.That(HHThemes.Next(panel, new ThemeStyleSheet[] { null }), Is.Null);
        }
    }
}
