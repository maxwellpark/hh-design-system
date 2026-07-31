using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A Button pre-styled with the design system's .hh-button class. Usable in UXML as <hh:HHButton/>.
    public class HHButton : Button
    {
        public new class UxmlFactory : UxmlFactory<HHButton, UxmlTraits> { }

        public HHButton()
        {
            AddToClassList("hh-button");
        }
    }
}
