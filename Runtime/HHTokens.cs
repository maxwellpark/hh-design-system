using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Reads the tokens an element resolves; they exist only once its styles do.
    public static class HHTokens
    {
        /// "color-teal" and "--hh-color-teal" both name the token --hh-color-teal.
        public static string PropertyName(string token)
        {
            return token.StartsWith("--") ? token : "--hh-" + token;
        }

        public static bool TryGetColor(VisualElement element, string token, out Color color)
        {
            color = default;
            return element != null && element.customStyle.TryGetValue(new CustomStyleProperty<Color>(PropertyName(token)), out color);
        }

        /// Lengths such as 12px come back as their number.
        public static bool TryGetFloat(VisualElement element, string token, out float value)
        {
            value = default;
            return element != null && element.customStyle.TryGetValue(new CustomStyleProperty<float>(PropertyName(token)), out value);
        }

        /// Runs the callback each time the element's styles resolve from now on, with the element to read from.
        public static void WhenResolved(VisualElement element, Action<VisualElement> callback)
        {
            if (element == null || callback == null)
            {
                return;
            }
            element.RegisterCallback<CustomStyleResolvedEvent>(e => callback((VisualElement)e.currentTarget));
        }
    }
}
