using NUnit.Framework;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class TooltipTests
    {
        private static (VisualElement layer, VisualElement target, HHTooltip tooltip) Setup(string text, string targetTooltip = null)
        {
            var layer = new VisualElement();
            var target = new VisualElement { tooltip = targetTooltip };
            layer.Add(target);
            var tooltip = new HHTooltip(text) { Layer = layer };
            target.AddManipulator(tooltip);
            return (layer, target, tooltip);
        }

        [Test]
        public void ShowAddsAStyledLabelToTheLayerAndHideRemovesIt()
        {
            var (layer, _, tooltip) = Setup("Settings");

            tooltip.Show();
            Assert.That(tooltip.IsShowing, Is.True);
            Assert.That(tooltip.Label.parent, Is.SameAs(layer));
            Assert.That(tooltip.Label.text, Is.EqualTo("Settings"));
            Assert.That(tooltip.Label.ClassListContains(HHTooltip.UssClassName), Is.True);

            tooltip.Hide();
            Assert.That(tooltip.IsShowing, Is.False);
        }

        [Test]
        public void FallsBackToTheTargetsTooltipAndShowsNothingWithoutText()
        {
            var (_, _, fromTarget) = Setup(null, "From target");
            Assert.That(fromTarget.Text, Is.EqualTo("From target"));

            var (_, _, empty) = Setup(null);
            empty.Show();
            Assert.That(empty.IsShowing, Is.False);
        }

        [Test]
        public void RemovingTheManipulatorHidesTheLabel()
        {
            var (_, target, tooltip) = Setup("Bye");
            tooltip.Show();

            target.RemoveManipulator(tooltip);
            Assert.That(tooltip.IsShowing, Is.False);
        }

        [Test]
        public void TheLayerIsTheTopmostAncestorWithoutAPanel()
        {
            var document = new VisualElement();
            var card = new VisualElement();
            var target = new VisualElement();
            document.Add(card);
            card.Add(target);

            Assert.That(HHTooltip.TopLevel(target), Is.SameAs(document));
        }

        [Test]
        public void IconButtonsCarryATooltipForTheirLabel()
        {
            var button = new HHIconButton(null, "Settings", null);
            var layer = new VisualElement();
            layer.Add(button);
            button.Tooltip.Layer = layer;

            button.Tooltip.Show();
            Assert.That(button.Tooltip.Label.text, Is.EqualTo("Settings"));
            Assert.That(button.Tooltip.Label.parent, Is.SameAs(layer));
        }
    }
}
