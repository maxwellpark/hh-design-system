using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHTextField label="Name" />
    [UxmlElement]
    public partial class HHTextField : TextField
    {
        public const string UssClassName = "hh-text-field";

        public HHTextField()
        {
            AddToClassList(UssClassName);
        }
    }
}
