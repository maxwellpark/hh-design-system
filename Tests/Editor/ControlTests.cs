using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class ControlTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        private static VisualElement LoadFixture()
        {
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture);
            Assert.That(tree, Is.Not.Null, $"no fixture at {Fixture}");
            return tree.Instantiate();
        }

        [Test]
        public void ButtonCarriesItsStyleClass()
        {
            var button = new HHButton();

            Assert.That(button.ClassListContains(HHButton.UssClassName), Is.True);
            Assert.That(button.Ghost, Is.False);
        }

        [Test]
        public void GhostTogglesItsClass()
        {
            var button = new HHButton { Ghost = true };
            Assert.That(button.ClassListContains(HHButton.GhostUssClassName), Is.True);

            button.Ghost = false;
            Assert.That(button.ClassListContains(HHButton.GhostUssClassName), Is.False);
            Assert.That(button.ClassListContains(HHButton.UssClassName), Is.True);
        }

        [Test]
        public void ProgressBarCarriesItsStyleClass()
        {
            Assert.That(new HHProgressBar().ClassListContains(HHProgressBar.UssClassName), Is.True);
        }

        [Test]
        public void UxmlAttributesReachTheControls()
        {
            var root = LoadFixture();

            var progress = root.Q<HHProgressBar>("progress");
            Assert.That(progress, Is.Not.Null);
            Assert.That(progress.value, Is.EqualTo(40f));
            Assert.That(progress.title, Is.EqualTo("Loading"));

            Assert.That(root.Q<HHButton>("ghost").Ghost, Is.True);
            Assert.That(root.Q<HHButton>("plain").Ghost, Is.False);
        }

        [Test]
        public void FormControlsCarryTheirStyleClasses()
        {
            Assert.That(new HHToggle().ClassListContains(HHToggle.UssClassName), Is.True);
            Assert.That(new HHSlider().ClassListContains(HHSlider.UssClassName), Is.True);
            Assert.That(new HHDropdown().ClassListContains(HHDropdown.UssClassName), Is.True);
            Assert.That(new HHTextField().ClassListContains(HHTextField.UssClassName), Is.True);
        }

        [Test]
        public void FormControlsKeepTheirUxmlAttributes()
        {
            var root = LoadFixture();

            Assert.That(root.Q<HHToggle>("toggle").value, Is.True);
            Assert.That(root.Q<HHToggle>("toggle").label, Is.EqualTo("Music"));
            Assert.That(root.Q<HHSlider>("slider").value, Is.EqualTo(0.5f));
            Assert.That(root.Q<HHSlider>("slider").highValue, Is.EqualTo(1f));
            Assert.That(root.Q<HHDropdown>("dropdown").value, Is.EqualTo("High"));
            Assert.That(root.Q<HHTextField>("text-field").value, Is.EqualTo("Northwind"));
        }

        [Test]
        public void ApplyAddsEachStyleSheetOnce()
        {
            var root = new VisualElement();
            var tokens = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-tokens.uss");
            var components = AssetDatabase.LoadAssetAtPath<StyleSheet>("Packages/com.hootonheroics.designsystem/Runtime/Styles/hh-components.uss");
            Assert.That(tokens, Is.Not.Null);
            Assert.That(components, Is.Not.Null);

            HHStyles.Apply(root, tokens, components);
            HHStyles.Apply(root, tokens, components);

            Assert.That(root.styleSheets.count, Is.EqualTo(2));
        }
    }
}
