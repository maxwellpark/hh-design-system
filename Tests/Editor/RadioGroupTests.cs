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

        // Off a panel the group doesn't check its buttons (it waits to attach), so this holds the value, not the checks
        [Test]
        public void ChoicesBecomeButtonsInOrderAndValueHolds()
        {
            var group = new HHRadioGroup("Difficulty", new List<string> { "Easy", "Normal", "Hard" }) { value = 2 };
            var buttons = group.Query<RadioButton>().ToList();

            Assert.That(buttons.Select(b => b.text), Is.EqualTo(new[] { "Easy", "Normal", "Hard" }));
            Assert.That(group.value, Is.EqualTo(2));
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
