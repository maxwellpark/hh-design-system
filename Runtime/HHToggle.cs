using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHToggle label="Music" value="true" />
    [UxmlElement]
    public partial class HHToggle : Toggle
    {
        public const string UssClassName = "hh-toggle";

        public HHToggle()
        {
            AddToClassList(UssClassName);
        }
    }
}
