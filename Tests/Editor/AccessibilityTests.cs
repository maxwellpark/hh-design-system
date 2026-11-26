using NUnit.Framework;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class AccessibilityTests
    {
        [Test]
        public void ModifiersToggleTheirClasses()
        {
            var root = new VisualElement();

            HHAccessibility.SetReducedMotion(root, true);
            HHAccessibility.SetLargeText(root, true);
            Assert.That(root.ClassListContains(HHAccessibility.ReducedMotionUssClassName), Is.True);
            Assert.That(root.ClassListContains(HHAccessibility.LargeTextUssClassName), Is.True);

            HHAccessibility.SetReducedMotion(root, false);
            HHAccessibility.SetLargeText(root, false);
            Assert.That(root.ClassListContains(HHAccessibility.ReducedMotionUssClassName), Is.False);
            Assert.That(root.ClassListContains(HHAccessibility.LargeTextUssClassName), Is.False);
        }

        [Test]
        public void ANullRootIsIgnored()
        {
            Assert.DoesNotThrow(() => HHAccessibility.SetReducedMotion(null, true));
            Assert.DoesNotThrow(() => HHAccessibility.SetLargeText(null, true));
        }
    }
}
