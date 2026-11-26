using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Tests
{
    public class TokensTests
    {
        [Test]
        public void ShortAndFullTokenNamesMeanTheSameProperty()
        {
            Assert.That(HHTokens.PropertyName("color-teal"), Is.EqualTo("--hh-color-teal"));
            Assert.That(HHTokens.PropertyName("--hh-color-teal"), Is.EqualTo("--hh-color-teal"));
        }

        [Test]
        public void NothingResolvesBeforeStylesDo()
        {
            var element = new VisualElement();
            Assert.That(HHTokens.TryGetColor(element, "color-teal", out _), Is.False);
            Assert.That(HHTokens.TryGetFloat(element, "space-4", out _), Is.False);
            Assert.That(HHTokens.TryGetColor(null, "color-teal", out var color), Is.False);
            Assert.That(color, Is.EqualTo(default(Color)));
        }
    }
}
