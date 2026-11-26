using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHScrollView><ui:Label text="Long content" /></hh:HHScrollView>
    [UxmlElement]
    public partial class HHScrollView : ScrollView
    {
        public const string UssClassName = "hh-scroll-view";

        public HHScrollView()
        {
            AddToClassList(UssClassName);
        }
    }
}
