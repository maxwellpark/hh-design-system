using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class TabsTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void TabsCarryTheirStyleClassAndStayATabView()
        {
            var tabs = new HHTabs();
            Assert.That(tabs.ClassListContains(HHTabs.UssClassName), Is.True);
            Assert.That(tabs.ClassListContains(TabView.ussClassName), Is.True);
        }

        [Test]
        public void UxmlTabsLandInTheView()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var tabs = root.Q<HHTabs>("tabs");

            Assert.That(tabs.Q<Tab>("audio").label, Is.EqualTo("Audio"));
            Assert.That(tabs.Q<Tab>("video").label, Is.EqualTo("Video"));
        }
    }
}
