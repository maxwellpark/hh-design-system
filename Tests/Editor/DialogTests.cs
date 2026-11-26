using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class DialogTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void OpensAndClosesOnceRaisingClosed()
        {
            var dialog = new HHDialog();
            var closed = 0;
            dialog.Closed += () => closed++;
            Assert.That(dialog.IsOpen, Is.False);

            dialog.Open();
            Assert.That(dialog.IsOpen, Is.True);
            Assert.That(dialog.ClassListContains(HHDialog.OpenUssClassName), Is.True);

            dialog.Close();
            dialog.Close();
            Assert.That(dialog.IsOpen, Is.False);
            Assert.That(closed, Is.EqualTo(1));
        }

        [Test]
        public void CancelAndScrimCloseUnlessAChoiceIsRequired()
        {
            var dialog = new HHDialog();
            dialog.Open();
            Assert.That(dialog.HandleCancel(), Is.True);
            Assert.That(dialog.IsOpen, Is.False);

            dialog.Open();
            dialog.HandleScrimPointer(dialog);
            Assert.That(dialog.IsOpen, Is.False);

            dialog.RequireChoice = true;
            dialog.Open();
            Assert.That(dialog.HandleCancel(), Is.False);
            dialog.HandleScrimPointer(dialog);
            Assert.That(dialog.IsOpen, Is.True);
        }

        [Test]
        public void APointerOnTheCardDoesNotClose()
        {
            var dialog = new HHDialog();
            dialog.Open();
            dialog.HandleScrimPointer(dialog.Actions);
            Assert.That(dialog.IsOpen, Is.True);
        }

        [Test]
        public void ActionsRunThenCloseUnlessToldNot()
        {
            var dialog = new HHDialog { RequireChoice = true };
            var ran = 0;
            var keep = dialog.AddAction("Keep", () => ran++, closes: false);
            var quit = dialog.AddAction("Quit", () => ran++, ghost: true);
            Assert.That(dialog.Actions.childCount, Is.EqualTo(2));
            Assert.That(quit.Ghost, Is.True);
            Assert.That(keep.text, Is.EqualTo("Keep"));

            dialog.Open();
            dialog.RunAction(() => ran++, closes: false);
            Assert.That(dialog.IsOpen, Is.True);
            dialog.RunAction(() => ran++, closes: true);
            Assert.That(dialog.IsOpen, Is.False);
            Assert.That(ran, Is.EqualTo(2));
        }

        [Test]
        public void UxmlChildrenGoInTheBodyAndAttributesApply()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();
            var dialog = root.Q<HHDialog>("dialog");

            Assert.That(dialog.Title, Is.EqualTo("Quit?"));
            Assert.That(dialog.RequireChoice, Is.True);
            Assert.That(dialog.contentContainer.Q<Label>("dialog-body"), Is.Not.Null);
            Assert.That(dialog.contentContainer.ClassListContains(HHDialog.BodyUssClassName), Is.True);
        }
    }
}
