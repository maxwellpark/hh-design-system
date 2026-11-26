using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHToggle label="Music" value="true" switch="true" />
    [UxmlElement]
    public partial class HHToggle : Toggle
    {
        public const string UssClassName = "hh-toggle";
        public const string SwitchUssClassName = UssClassName + "--switch";
        public const string KnobUssClassName = UssClassName + "__knob";

        private readonly VisualElement knob;

        public HHToggle()
        {
            AddToClassList(UssClassName);
            // The knob rides in the checkmark, which a switch draws as its track; it only shows on a switch
            knob = new VisualElement { pickingMode = PickingMode.Ignore };
            knob.AddToClassList(KnobUssClassName);
            this.Q(className: checkmarkUssClassName)?.Add(knob);
        }

        /// Draws the toggle as a sliding switch rather than a box; `switch` is a C# keyword, hence the name.
        [UxmlAttribute("switch")]
        public bool IsSwitch
        {
            get => ClassListContains(SwitchUssClassName);
            set => EnableInClassList(SwitchUssClassName, value);
        }

        public VisualElement Knob => knob;
    }
}
