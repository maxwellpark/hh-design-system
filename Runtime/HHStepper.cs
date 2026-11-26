using System.Collections.Generic;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// One value from an ordered list, stepped with previous and next, or left and right on a gamepad or keyboard.
    /// UXML: <hh:HHStepper label="Resolution" choices="720p,1080p,1440p,4K" index="1" />
    [UxmlElement]
    public partial class HHStepper : BaseField<int>
    {
        public const string UssClassName = "hh-stepper";
        public const string ButtonUssClassName = UssClassName + "__button";
        public const string ValueUssClassName = UssClassName + "__value";

        private readonly HHButton previous;
        private readonly HHButton next;
        private readonly Label valueLabel;
        private List<string> choices = new List<string>();
        // The index last asked for, so one set before its choices (as UXML may) lands once they arrive
        private int requested;

        public HHStepper() : this(null, null) { }

        public HHStepper(string label, List<string> choices = null) : base(label, new VisualElement())
        {
            AddToClassList(UssClassName);
            // The field takes focus and handles left and right, so its buttons are for the pointer only
            previous = new HHButton(() => Step(-1)) { text = "<", Ghost = true, focusable = false };
            next = new HHButton(() => Step(1)) { text = ">", Ghost = true, focusable = false };
            valueLabel = new Label();
            previous.AddToClassList(ButtonUssClassName);
            next.AddToClassList(ButtonUssClassName);
            valueLabel.AddToClassList(ValueUssClassName);
            var input = this.Q(className: inputUssClassName);
            input.Add(previous);
            input.Add(valueLabel);
            input.Add(next);
            RegisterCallback<NavigationMoveEvent>(OnNavigationMove);
            Choices = choices ?? new List<string>();
        }

        /// The values in order; the index is kept within them.
        [UxmlAttribute]
        public List<string> Choices
        {
            get => choices;
            set
            {
                choices = value ?? new List<string>();
                SetValueWithoutNotify(requested);
            }
        }

        /// The chosen value's position in Choices.
        [UxmlAttribute]
        public int Index
        {
            get => value;
            set => this.value = value;
        }

        /// The chosen value, or empty when there are no choices.
        public string Text => valueLabel.text;

        /// Moves by delta, stopping at the ends.
        public void Step(int delta)
        {
            value = this.value + delta;
        }

        public override void SetValueWithoutNotify(int newValue)
        {
            requested = newValue;
            var last = choices == null ? -1 : choices.Count - 1;
            var clamped = last < 0 ? 0 : newValue < 0 ? 0 : newValue > last ? last : newValue;
            base.SetValueWithoutNotify(clamped);
            // Set during the base constructor, before the parts exist
            if (valueLabel == null)
            {
                return;
            }
            valueLabel.text = last < 0 ? string.Empty : choices[clamped];
            previous.SetEnabled(clamped > 0);
            next.SetEnabled(clamped < last);
        }

        private void OnNavigationMove(NavigationMoveEvent e)
        {
            var delta = e.direction == NavigationMoveEvent.Direction.Left ? -1 : e.direction == NavigationMoveEvent.Direction.Right ? 1 : 0;
            if (delta != 0)
            {
                Step(delta);
                e.StopPropagation();
            }
        }
    }
}
