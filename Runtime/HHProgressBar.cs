using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A ProgressBar with the .hh-progress-bar style. UXML: <hh:HHProgressBar value="40" title="Loading" />
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
