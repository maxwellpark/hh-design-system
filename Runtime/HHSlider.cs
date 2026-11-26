using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHSlider label="Volume" low-value="0" high-value="1" />
    [UxmlElement]
    public partial class HHSlider : Slider
    {
        public const string UssClassName = "hh-slider";

        public HHSlider()
        {
            AddToClassList(UssClassName);
        }
    }
}
