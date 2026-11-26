using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class BadgeAndIconButtonTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void IconButtonUsesItsLabelAsTooltipAndShowsTheIcon()
        {
            var icon = new Texture2D(2, 2);
            var button = new HHIconButton(icon, "Settings", null);

            Assert.That(button.ClassListContains(HHIconButton.UssClassName), Is.True);
            Assert.That(button.tooltip, Is.EqualTo("Settings"));
            Assert.That(button.style.backgroundImage.value.texture, Is.SameAs(icon));

            button.Icon = null;
            Assert.That(button.style.backgroundImage.keyword, Is.EqualTo(StyleKeyword.Null));
            Object.DestroyImmediate(icon);
        }

        [Test]
        public void BadgeShowsItsCountCappedAt99()
        {
            var badge = new HHBadge { Count = 7 };
            Assert.That(badge.text, Is.EqualTo("7"));

            badge.Count = 100;
            Assert.That(badge.text, Is.EqualTo("99+"));

            badge.Count = -3;
            Assert.That(badge.Count, Is.EqualTo(0));
        }

        [Test]
        public void DotBadgeHidesItsCountAndShowsItAgain()
        {
            var badge = new HHBadge { Count = 5, Dot = true };
            Assert.That(badge.text, Is.Empty);
            Assert.That(badge.ClassListContains(HHBadge.DotUssClassName), Is.True);

            badge.Dot = false;
            Assert.That(badge.text, Is.EqualTo("5"));
        }

        [Test]
        public void ToneSetsExactlyOneToneClass()
        {
            var badge = new HHBadge { Tone = HHTone.Pink };
            Assert.That(badge.ClassListContains(HHBadge.PinkUssClassName), Is.True);

            badge.Tone = HHTone.Teal;
            Assert.That(badge.ClassListContains(HHBadge.PinkUssClassName), Is.False);
            Assert.That(badge.ClassListContains(HHBadge.TealUssClassName), Is.True);

            badge.Tone = HHTone.Violet;
            Assert.That(badge.ClassListContains(HHBadge.TealUssClassName), Is.False);
        }

        [Test]
        public void UxmlAttributesReachTheBadgeAndIconButton()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();

            Assert.That(root.Q<HHIconButton>("icon-button").tooltip, Is.EqualTo("Settings"));
            Assert.That(root.Q<HHBadge>("badge").text, Is.EqualTo("99+"));
            Assert.That(root.Q<HHBadge>("badge").Tone, Is.EqualTo(HHTone.Pink));
            Assert.That(root.Q<HHBadge>("dot").Dot, Is.True);
            Assert.That(root.Q<HHBadge>("dot").text, Is.Empty);
        }
    }
}
