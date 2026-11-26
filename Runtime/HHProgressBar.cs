using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A ProgressBar pre-styled with the design system's .hh-progress-bar class. It keeps
    /// ProgressBar's value, lowValue, highValue and title.
    /// UXML: <hh:HHProgressBar value="40" title="Loading" />.
    [UxmlElement]
    public partial class HHProgressBar : ProgressBar
    {
        public const string UssClassName = "hh-progress-bar";

        public HHProgressBar()
        {
            AddToClassList(UssClassName);
        }
    }
}
