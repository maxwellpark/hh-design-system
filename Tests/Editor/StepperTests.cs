using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class StepperTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        private static HHStepper Resolutions(int index)
        {
            return new HHStepper("Resolution", new List<string> { "720p", "1080p", "1440p" }) { Index = index };
        }

        [Test]
        public void StepperCarriesItsClassAndShowsTheChosenValue()
        {
            var stepper = Resolutions(1);
            Assert.That(stepper.ClassListContains(HHStepper.UssClassName), Is.True);
            Assert.That(stepper.Text, Is.EqualTo("1080p"));
        }

        [Test]
        public void SteppingStopsAtTheEndsAndDisablesThatButton()
        {
            var stepper = Resolutions(1);
            var buttons = stepper.Query<HHButton>(className: HHStepper.ButtonUssClassName).ToList();

            stepper.Step(1);
            stepper.Step(1);
            Assert.That(stepper.Index, Is.EqualTo(2));
            Assert.That(buttons[1].enabledSelf, Is.False);
            Assert.That(buttons[0].enabledSelf, Is.True);

            stepper.Step(-5);
            Assert.That(stepper.Index, Is.EqualTo(0));
            Assert.That(buttons[0].enabledSelf, Is.False);
        }

        [Test]
        public void TheIndexStaysWithinTheChoices()
        {
            var stepper = Resolutions(9);
            Assert.That(stepper.Index, Is.EqualTo(2));

            stepper.Choices = new List<string> { "Only" };
            Assert.That(stepper.Index, Is.EqualTo(0));
            Assert.That(stepper.Text, Is.EqualTo("Only"));

            stepper.Choices = null;
            Assert.That(stepper.Text, Is.Empty);
        }

        [Test]
        public void AnIndexSetBeforeItsChoicesLandsWhenTheyArrive()
        {
            var stepper = new HHStepper { Index = 2 };
            Assert.That(stepper.Index, Is.EqualTo(0));

            stepper.Choices = new List<string> { "720p", "1080p", "1440p" };
            Assert.That(stepper.Index, Is.EqualTo(2));
            Assert.That(stepper.Text, Is.EqualTo("1440p"));
        }

        [Test]
        public void ItsButtonsLeaveFocusToTheField()
        {
            var stepper = Resolutions(0);
            foreach (var button in stepper.Query<HHButton>().ToList())
            {
                Assert.That(button.focusable, Is.False);
            }
        }

        [Test]
        public void UxmlChoicesAndIndexApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var stepper = root.Q<HHStepper>("stepper");

            Assert.That(stepper.label, Is.EqualTo("Resolution"));
            Assert.That(stepper.Choices, Is.EqualTo(new[] { "720p", "1080p", "1440p" }));
            Assert.That(stepper.Text, Is.EqualTo("1080p"));
        }
    }
}
