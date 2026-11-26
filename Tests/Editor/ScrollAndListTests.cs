using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class ScrollAndListTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void ScrollAndListCarryTheirClassesOverUnitys()
        {
            var scroll = new HHScrollView();
            var list = new HHListView();

            Assert.That(scroll.ClassListContains(HHScrollView.UssClassName), Is.True);
            Assert.That(scroll.ClassListContains(ScrollView.ussClassName), Is.True);
            Assert.That(list.ClassListContains(HHListView.UssClassName), Is.True);
            Assert.That(list.ClassListContains(ListView.ussClassName), Is.True);
        }

        [Test]
        public void ListBindsItemsAsAListView()
        {
            var list = new HHListView
            {
                itemsSource = new List<string> { "Low", "High" },
                makeItem = () => new Label(),
                bindItem = (element, index) => ((Label)element).text = index.ToString(),
            };
            Assert.That(list.itemsSource.Count, Is.EqualTo(2));
        }

        [Test]
        public void UxmlContentAndAttributesApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();

            Assert.That(root.Q<HHScrollView>("scroll").contentContainer.Q<Label>("scroll-content"), Is.Not.Null);
            Assert.That(root.Q<HHListView>("list").fixedItemHeight, Is.EqualTo(36f));
        }
    }
}
