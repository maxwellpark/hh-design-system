using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHDropdown label="Quality" choices="Low,High" index="1" />
    [UxmlElement]
    public partial class HHDropdown : DropdownField
    {
        public const string UssClassName = "hh-dropdown";

        public HHDropdown()
        {
            AddToClassList(UssClassName);
        }
    }
}
