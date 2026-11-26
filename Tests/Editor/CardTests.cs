using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class CardTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void CardCarriesItsClassAndHidesAnEmptyTitle()
        {
            var card = new HHCard();
            var title = card.Q<Label>(className: HHCard.TitleUssClassName);

            Assert.That(card.ClassListContains(HHCard.UssClassName), Is.True);
            Assert.That(title.ClassListContains("hh-title"), Is.True);
            Assert.That(title.style.display.value, Is.EqualTo(DisplayStyle.None));

            card.Title = "Settings";
            Assert.That(title.text, Is.EqualTo("Settings"));
            Assert.That(title.style.display.value, Is.EqualTo(DisplayStyle.Flex));
        }

        [Test]
        public void ChildrenGoInTheBodyAndClearKeepsTheTitle()
        {
            var card = new HHCard { Title = "Settings" };
            var toggle = new HHToggle();
            card.Add(toggle);

            Assert.That(toggle.parent, Is.SameAs(card.contentContainer));
            Assert.That(card.contentContainer.ClassListContains(HHCard.BodyUssClassName), Is.True);
            card.Clear();
            Assert.That(card.Q<Label>(className: HHCard.TitleUssClassName).text, Is.EqualTo("Settings"));
        }

        [Test]
        public void UxmlTitleAndChildrenApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var card = root.Q<HHCard>("card");

            Assert.That(card.Title, Is.EqualTo("Audio"));
            Assert.That(card.contentContainer.Q<HHToggle>("card-toggle"), Is.Not.Null);
        }
    }
}
