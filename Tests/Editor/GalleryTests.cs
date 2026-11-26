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
    }
}
