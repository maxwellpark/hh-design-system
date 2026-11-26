using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHProgressBar value="40" title="Loading" />
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
