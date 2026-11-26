using NUnit.Framework;
using UnityEditor;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class ProgressVariantTests
    {
        private const string Fixture = "Packages/com.hootonheroics.designsystem/Tests/Editor/Fixtures/controls.uxml";

        [Test]
        public void ProgressBarIsTealUntilGivenAnotherTone()
        {
            var bar = new HHProgressBar();
            Assert.That(bar.Tone, Is.EqualTo(HHTone.Teal));
            Assert.That(bar.ClassListContains(HHProgressBar.PinkUssClassName), Is.False);

            bar.Tone = HHTone.Pink;
            Assert.That(bar.ClassListContains(HHProgressBar.PinkUssClassName), Is.True);

            bar.Tone = HHTone.Teal;
            Assert.That(bar.ClassListContains(HHProgressBar.PinkUssClassName), Is.False);
        }

        [Test]
        public void SegmentedBarBuildsOneChildPerSegment()
        {
            var bar = new HHSegmentedBar { Segments = 6 };
            Assert.That(bar.childCount, Is.EqualTo(6));

            bar.Segments = 2;
            Assert.That(bar.childCount, Is.EqualTo(2));

            bar.Segments = 0;
            Assert.That(bar.Segments, Is.EqualTo(1));
            bar.Segments = 500;
            Assert.That(bar.Segments, Is.EqualTo(HHSegmentedBar.MaxSegments));
        }

        [Test]
        public void SegmentedBarFillsFromTheStartAndClamps()
        {
            var bar = new HHSegmentedBar { Segments = 4, Filled = 3 };
            Assert.That(bar[2].ClassListContains(HHSegmentedBar.FilledUssClassName), Is.True);
            Assert.That(bar[3].ClassListContains(HHSegmentedBar.FilledUssClassName), Is.False);

            bar.Segments = 2;
            Assert.That(bar.Filled, Is.EqualTo(2));

            bar.Filled = -1;
            Assert.That(bar.Filled, Is.EqualTo(0));
        }

        [Test]
        public void FilledSetBeforeSegmentsSurvivesTheGrowth()
        {
            var bar = new HHSegmentedBar { Filled = 8, Segments = 10 };
            Assert.That(bar.Filled, Is.EqualTo(8));
        }

        [Test]
        public void UxmlAttributesReachTheVariants()
        {
            var root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Fixture).Instantiate();

            Assert.That(root.Q<HHProgressBar>("pink-progress").Tone, Is.EqualTo(HHTone.Pink));
            var health = root.Q<HHSegmentedBar>("health");
            Assert.That(health.Segments, Is.EqualTo(4));
            Assert.That(health.Filled, Is.EqualTo(3));
            Assert.That(health.ClassListContains(HHSegmentedBar.PinkUssClassName), Is.True);
            Assert.That(root.Q<HHSegmentedBar>("ammo").Filled, Is.EqualTo(8));
        }
    }
}
