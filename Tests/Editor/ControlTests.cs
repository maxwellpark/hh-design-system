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
