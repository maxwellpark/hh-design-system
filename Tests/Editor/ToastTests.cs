using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class ToastTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void ShowAddsAToastWithItsTextAndTone()
        {
            var stack = new HHToastStack();
            var toast = stack.Show("Saved", HHToastTone.Success, 0f);

            Assert.That(stack.childCount, Is.EqualTo(1));
            Assert.That(toast.Text, Is.EqualTo("Saved"));
            Assert.That(toast.ClassListContains(HHToast.SuccessUssClassName), Is.True);
            Assert.That(toast.ClassListContains(HHToast.DangerUssClassName), Is.False);
        }

        [Test]
        public void NewestGoesLastAndTheOldestIsDismissedPastTheCap()
        {
            var stack = new HHToastStack { MaxVisible = 2 };
            var dismissed = 0;
            var first = stack.Show("One", seconds: 0f);
            first.Dismissed += () => dismissed++;
            stack.Show("Two", seconds: 0f);
            var third = stack.Show("Three", seconds: 0f);

            Assert.That(stack.childCount, Is.EqualTo(2));
            Assert.That(stack[1], Is.SameAs(third));
            Assert.That(first.parent, Is.Null);
            Assert.That(dismissed, Is.EqualTo(1));
        }

        [Test]
        public void DismissIsSafeToCallTwice()
        {
            var stack = new HHToastStack();
            var toast = stack.Show("Gone", seconds: 0f);
            var dismissed = 0;
            toast.Dismissed += () => dismissed++;

            toast.Dismiss();
            toast.Dismiss();
            Assert.That(dismissed, Is.EqualTo(1));
            Assert.That(stack.childCount, Is.EqualTo(0));
        }

        [Test]
        public void LoweringTheCapTrimsAndItNeverDropsBelowOne()
        {
            var stack = new HHToastStack();
            stack.Show("One", seconds: 0f);
            stack.Show("Two", seconds: 0f);
            stack.MaxVisible = 0;
            Assert.That(stack.MaxVisible, Is.EqualTo(1));
            Assert.That(stack.childCount, Is.EqualTo(1));
        }

        [Test]
        public void UxmlAttributesReachTheStackAndToast()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();

            Assert.That(root.Q<HHToastStack>("toasts").MaxVisible, Is.EqualTo(2));
            Assert.That(root.Q<HHToast>("toast").Text, Is.EqualTo("Saved"));
            Assert.That(root.Q<HHToast>("toast").Tone, Is.EqualTo(HHToastTone.Success));
        }
    }
}
