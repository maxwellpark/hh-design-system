using System.Linq;
using NUnit.Framework;
using HootonHeroics.DesignSystem.Editor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class GalleryTests
    {
        [Test]
        public void TheGalleryShowsEveryControl()
        {
            var page = new VisualElement();
            HHGallery.Build(page);

            Assert.That(page.ClassListContains(HHGallery.PageUssClassName), Is.True);
            Assert.That(page.Q<HHButton>(), Is.Not.Null);
            Assert.That(page.Q<HHIconButton>(), Is.Not.Null);
            Assert.That(page.Q<HHBadge>(), Is.Not.Null);
            Assert.That(page.Q<HHProgressBar>(), Is.Not.Null);
            Assert.That(page.Q<HHSegmentedBar>(), Is.Not.Null);
            Assert.That(page.Q<HHToggle>(), Is.Not.Null);
            Assert.That(page.Q<HHSlider>(), Is.Not.Null);
            Assert.That(page.Q<HHDropdown>(), Is.Not.Null);
            Assert.That(page.Q<HHTextField>(), Is.Not.Null);
            Assert.That(page.Q<HHTabs>(), Is.Not.Null);
            Assert.That(page.Q<HHScrollView>(), Is.Not.Null);
            Assert.That(page.Q<HHListView>(), Is.Not.Null);
            Assert.That(page.Q<HHDialog>(), Is.Not.Null);
            Assert.That(page.Q<HHToastStack>(), Is.Not.Null);
            Assert.That(page.Q<HHToast>(), Is.Not.Null);
        }

        [Test]
        public void TheDialogStartsClosed()
        {
            var page = new VisualElement();
            HHGallery.Build(page);
            Assert.That(page.Q<HHDialog>().IsOpen, Is.False);
        }

        private static bool HasDisabled<T>(VisualElement page) where T : VisualElement
        {
            return page.Query<T>().Where(e => !e.enabledSelf).ToList().Any();
        }

        // check.mjs holds the gallery's source to this; the test holds the page it builds
        [Test]
        public void ThePageHoldsEveryFocusableControlDisabled()
        {
            var page = new VisualElement();
            HHGallery.Build(page);

            Assert.That(HasDisabled<HHButton>(page), Is.True, "button");
            Assert.That(HasDisabled<HHIconButton>(page), Is.True, "icon button");
            Assert.That(HasDisabled<HHHoldButton>(page), Is.True, "hold button");
            Assert.That(HasDisabled<HHToggle>(page), Is.True, "toggle");
            Assert.That(HasDisabled<HHSlider>(page), Is.True, "slider");
            Assert.That(HasDisabled<HHDropdown>(page), Is.True, "dropdown");
            Assert.That(HasDisabled<HHTextField>(page), Is.True, "text field");
            Assert.That(HasDisabled<HHRadioGroup>(page), Is.True, "radio group");
            Assert.That(HasDisabled<HHStepper>(page), Is.True, "stepper");
            Assert.That(HasDisabled<HHTabs>(page), Is.True, "tabs");
            Assert.That(HasDisabled<HHListView>(page), Is.True, "list");
            Assert.That(HasDisabled<HHScrollView>(page), Is.True, "scroll view");
        }
    }
}
