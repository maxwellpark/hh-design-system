using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class SwitchTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void AToggleIsABoxUntilItIsASwitch()
        {
            var toggle = new HHToggle();
            Assert.That(toggle.IsSwitch, Is.False);
            Assert.That(toggle.ClassListContains(HHToggle.SwitchUssClassName), Is.False);

            toggle.IsSwitch = true;
            Assert.That(toggle.ClassListContains(HHToggle.SwitchUssClassName), Is.True);
            toggle.IsSwitch = false;
            Assert.That(toggle.ClassListContains(HHToggle.SwitchUssClassName), Is.False);
        }

        [Test]
        public void TheKnobRidesInTheCheckmark()
        {
            var toggle = new HHToggle();
            Assert.That(toggle.Knob.ClassListContains(HHToggle.KnobUssClassName), Is.True);
            Assert.That(toggle.Knob.parent.ClassListContains(Toggle.checkmarkUssClassName), Is.True);
            Assert.That(toggle.Knob.pickingMode, Is.EqualTo(PickingMode.Ignore));
        }

        [Test]
        public void UxmlSwitchApplies()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            Assert.That(root.Q<HHToggle>("switch").IsSwitch, Is.True);
            Assert.That(root.Q<HHToggle>("toggle").IsSwitch, Is.False);
        }
    }
}
