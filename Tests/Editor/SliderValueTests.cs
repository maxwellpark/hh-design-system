using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class SliderValueTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        private static Label ValueLabel(HHSlider slider)
        {
            return slider.Q<Label>(className: HHSlider.ValueUssClassName);
        }

        [Test]
        public void TheValueIsHiddenUntilAsked()
        {
            var slider = new HHSlider { lowValue = 0f, highValue = 1f, value = 0.6f };
            Assert.That(ValueLabel(slider).style.display.value, Is.EqualTo(DisplayStyle.None));

            slider.ShowValue = true;
            Assert.That(ValueLabel(slider).style.display.value, Is.EqualTo(DisplayStyle.Flex));
        }

        [Test]
        public void PercentFollowsTheValueAcrossTheRange()
        {
            var slider = new HHSlider { lowValue = 0f, highValue = 1f, ShowValue = true };
            slider.value = 0.6f;
            Assert.That(slider.ValueText, Is.EqualTo("60%"));

            slider.SetValueWithoutNotify(1f);
            Assert.That(slider.ValueText, Is.EqualTo("100%"));

            var offset = new HHSlider { lowValue = 10f, highValue = 20f, value = 15f };
            Assert.That(offset.ValueText, Is.EqualTo("50%"));
        }

        [Test]
        public void PercentFollowsARangeChangeThatLeavesTheValue()
        {
            var slider = new HHSlider { value = 0.6f };
            slider.highValue = 1f;
            Assert.That(slider.ValueText, Is.EqualTo("60%"));

            slider.lowValue = 0.2f;
            Assert.That(slider.ValueText, Is.EqualTo("50%"));
        }

        [Test]
        public void NumberShowsTheValueToTwoPlaces()
        {
            var slider = new HHSlider { lowValue = 0f, highValue = 10f, ValueFormat = HHSliderValueFormat.Number };
            slider.value = 3f;
            Assert.That(slider.ValueText, Is.EqualTo("3"));

            slider.value = 2.456f;
            Assert.That(slider.ValueText, Is.EqualTo("2.46"));
        }

        [Test]
        public void UxmlAttributesApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var slider = root.Q<HHSlider>("valued-slider");

            Assert.That(slider.ShowValue, Is.True);
            Assert.That(slider.ValueFormat, Is.EqualTo(HHSliderValueFormat.Number));
            Assert.That(slider.ValueText, Is.EqualTo("3"));
        }
    }
}
