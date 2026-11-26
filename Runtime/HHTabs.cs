using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A TabView with the .hh-tabs style. UXML: <hh:HHTabs><ui:Tab label="Audio" /><ui:Tab label="Video" /></hh:HHTabs>
    [UxmlElement]
    public partial class HHTabs : TabView
    {
        public const string UssClassName = "hh-tabs";

        public HHTabs()
        {
            AddToClassList(UssClassName);
        }
    }
}
