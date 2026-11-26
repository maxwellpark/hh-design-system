using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class HoldButtonTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void ItLooksLikeAButtonAndHoldsForAtLeastATenthOfASecond()
        {
            var button = new HHHoldButton { HoldSeconds = 0f };
            Assert.That(button.ClassListContains("hh-button"), Is.True);
            Assert.That(button.ClassListContains(HHHoldButton.UssClassName), Is.True);
            Assert.That(button.HoldSeconds, Is.EqualTo(0.1f));
        }

        [Test]
        public void HoldingFillsThenConfirmsOnceAndResets()
        {
            var button = new HHHoldButton { HoldSeconds = 1f };
            var confirmed = 0;
            button.Confirmed += () => confirmed++;

            Assert.That(button.Advance(5f), Is.False, "not holding");
            button.StartHold();
            Assert.That(button.Advance(0.5f), Is.False);
            Assert.That(button.Progress, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(button.Advance(0.6f), Is.True);
            Assert.That(confirmed, Is.EqualTo(1));
            Assert.That(button.IsHolding, Is.False);
            Assert.That(button.Progress, Is.EqualTo(0f));
            Assert.That(button.Advance(1f), Is.False, "a finished hold doesn't confirm again");
            Assert.That(confirmed, Is.EqualTo(1));
        }

        [Test]
        public void LettingGoEarlyCancels()
        {
            var button = new HHHoldButton { HoldSeconds = 1f };
            var confirmed = 0;
            button.Confirmed += () => confirmed++;

            button.StartHold();
            button.Advance(0.9f);
            button.Cancel();
            button.StartHold();
            Assert.That(button.Advance(0.5f), Is.False, "a new hold starts from nothing");
            Assert.That(confirmed, Is.EqualTo(0));
        }

        [Test]
        public void ADisabledButtonDoesNotStartAHold()
        {
            var button = new HHHoldButton();
            button.SetEnabled(false);
            button.StartHold();
            Assert.That(button.IsHolding, Is.False);
        }

        [Test]
        public void AConfirmedHoldWaitsForReleaseBeforeAnother()
        {
            var button = new HHHoldButton { HoldSeconds = 1f };
            var confirmed = 0;
            button.Confirmed += () => confirmed++;

            button.StartHold();
            button.Advance(1f);
            button.StartHold();
            Assert.That(button.IsHolding, Is.False, "the key or pointer is still down");
            button.Release();
            button.StartHold();
            Assert.That(button.IsHolding, Is.True);
            Assert.That(confirmed, Is.EqualTo(1));
        }

        [Test]
        public void ItHasNoClickableSoNothingButAHoldConfirms()
        {
            Assert.That(new HHHoldButton().clickable, Is.Null);
        }

        [Test]
        public void DisablingMidHoldStopsIt()
        {
            var button = new HHHoldButton { HoldSeconds = 1f };
            button.StartHold();
            button.SetEnabled(false);
            Assert.That(button.Advance(2f), Is.False);
            Assert.That(button.IsHolding, Is.False);
        }

        [Test]
        public void BeginOnADetachedButtonDoesNothing()
        {
            var button = new HHHoldButton();
            button.Begin();
            Assert.That(button.IsHolding, Is.False);
        }

        [Test]
        public void UxmlHoldSecondsApplies()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            Assert.That(root.Q<HHHoldButton>("hold").HoldSeconds, Is.EqualTo(1.5f));
        }
    }
}
