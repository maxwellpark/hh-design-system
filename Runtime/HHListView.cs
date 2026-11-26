using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A ListView with the .hh-list-view style; bind items in code as usual. UXML: <hh:HHListView fixed-item-height="36" />
    [UxmlElement]
    public partial class HHListView : ListView
    {
        public const string UssClassName = "hh-list-view";

        public HHListView()
        {
            AddToClassList(UssClassName);
        }
    }
}
