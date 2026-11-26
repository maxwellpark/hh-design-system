using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class RadioGroupTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void RadioGroupCarriesItsStyleClassAndStaysARadioButtonGroup()
        {
            var group = new HHRadioGroup();
            Assert.That(group.ClassListContains(HHRadioGroup.UssClassName), Is.True);
            Assert.That(group.ClassListContains(RadioButtonGroup.ussClassName), Is.True);
        }

        [Test]
        public void ChoicesBecomeButtonsAndValueChecksOne()
        {
            var group = new HHRadioGroup("Difficulty", new List<string> { "Easy", "Normal", "Hard" }) { value = 2 };
            var buttons = group.Query<RadioButton>().ToList();

            Assert.That(buttons.Select(b => b.text), Is.EqualTo(new[] { "Easy", "Normal", "Hard" }));
            Assert.That(buttons.Select(b => b.value), Is.EqualTo(new[] { false, false, true }));
        }

        [Test]
        public void UxmlChoicesAndValueApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var group = root.Q<HHRadioGroup>("radio");

            Assert.That(group.label, Is.EqualTo("Difficulty"));
            Assert.That(group.choices, Is.EqualTo(new[] { "Easy", "Normal", "Hard" }));
            Assert.That(group.value, Is.EqualTo(1));
        }
    }
}
