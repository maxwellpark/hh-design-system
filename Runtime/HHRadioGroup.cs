using System.Collections.Generic;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// One choice of several, shown as a segmented row. UXML: <hh:HHRadioGroup label="Difficulty" choices="Easy,Normal,Hard" value="1" />
    [UxmlElement]
    public partial class HHRadioGroup : RadioButtonGroup
    {
        public const string UssClassName = "hh-radio-group";

        public HHRadioGroup() : this(null, null) { }

        public HHRadioGroup(string label, List<string> choices = null) : base(label, choices)
        {
            AddToClassList(UssClassName);
        }
    }
}
