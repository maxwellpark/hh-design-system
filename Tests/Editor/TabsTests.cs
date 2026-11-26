using System.Linq;
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
        public void HeadersBecomeFocusableAndSubmitSelectsTheirTab()
        {
            var tabs = new HHTabs();
            var audio = new Tab("Audio");
            var video = new Tab("Video");
            tabs.Add(audio);
            tabs.Add(video);

            tabs.MakeHeadersFocusable();
            Assert.That(audio.tabHeader.focusable, Is.True);
            Assert.That(video.tabHeader.focusable, Is.True);

            tabs.Select(video);
            Assert.That(tabs.activeTab, Is.SameAs(video));
        }

        [Test]
        public void UxmlTabsLandInTheView()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var tabs = root.Q<HHTabs>("tabs");

            Assert.That(tabs.Q<Tab>("audio").label, Is.EqualTo("Audio"));
            Assert.That(tabs.Q<Tab>("video").label, Is.EqualTo("Video"));
        }

        [Test]
        public void ABadgeShowsOnATabHeaderAndUpdatesInPlace()
        {
            var tabs = new HHTabs();
            var items = new Tab("Items");
            tabs.Add(items);

            var badge = tabs.SetBadge(items, 3);
            Assert.That(badge.Count, Is.EqualTo(3));
            Assert.That(badge.ClassListContains(HHTabs.BadgeUssClassName), Is.True);
            Assert.That(items.tabHeader.Contains(badge), Is.True);

            Assert.That(tabs.SetBadge(items, 5), Is.SameAs(badge));
            Assert.That(badge.Count, Is.EqualTo(5));
            Assert.That(items.tabHeader.Query<HHBadge>().ToList().Count, Is.EqualTo(1));
        }

        [Test]
        public void ZeroRemovesTheBadge()
        {
            var tabs = new HHTabs();
            var items = new Tab("Items");
            tabs.Add(items);
            tabs.SetBadge(items, 2);

            Assert.That(tabs.SetBadge(items, 0), Is.Null);
            Assert.That(items.tabHeader.Q<HHBadge>(), Is.Null);
            Assert.That(tabs.SetBadge(null, 1), Is.Null);
        }
    }
}
