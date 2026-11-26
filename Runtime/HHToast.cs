using System;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    public enum HHToastTone
    {
        Info,
        Success,
        Warning,
        Danger,
    }

    /// One notification card; HHToastStack creates and removes these. UXML: <hh:HHToast text="Saved" tone="Success" />
    [UxmlElement]
    public partial class HHToast : VisualElement
    {
        public const string UssClassName = "hh-toast";
        public const string TextUssClassName = UssClassName + "__text";
        public const string SuccessUssClassName = UssClassName + "--success";
        public const string WarningUssClassName = UssClassName + "--warning";
        public const string DangerUssClassName = UssClassName + "--danger";
        public const string ActionUssClassName = UssClassName + "__action";

        private readonly Label label;
        private HHToastTone tone;
        private Action onAction;

        /// Raised once, when the toast leaves its stack.
        public event Action Dismissed;

        public HHToast()
        {
            AddToClassList(UssClassName);
            label = new Label();
            label.AddToClassList(TextUssClassName);
            Add(label);
        }

        [UxmlAttribute]
        public string Text
        {
            get => label.text;
            set => label.text = value;
        }

        [UxmlAttribute]
        public HHToastTone Tone
        {
            get => tone;
            set
            {
                tone = value;
                EnableInClassList(SuccessUssClassName, value == HHToastTone.Success);
                EnableInClassList(WarningUssClassName, value == HHToastTone.Warning);
                EnableInClassList(DangerUssClassName, value == HHToastTone.Danger);
            }
        }

        /// The action button, or null until SetAction adds one.
        public HHButton ActionButton { get; private set; }

        /// Adds a ghost button (Undo, View) that runs onAction and dismisses the toast; a second call relabels it,
        /// and a null or empty label removes it.
        public void SetAction(string actionLabel, Action onAction)
        {
            this.onAction = onAction;
            if (string.IsNullOrEmpty(actionLabel))
            {
                ActionButton?.RemoveFromHierarchy();
                ActionButton = null;
                return;
            }
            if (ActionButton == null)
            {
                ActionButton = new HHButton(RunAction) { Ghost = true };
                ActionButton.AddToClassList(ActionUssClassName);
                Add(ActionButton);
            }
            ActionButton.text = actionLabel;
        }

        // Dismissed even if the callback throws, so a retry can't run it twice
        internal void RunAction()
        {
            if (parent == null)
            {
                return;
            }
            try
            {
                onAction?.Invoke();
            }
            finally
            {
                Dismiss();
            }
        }

        public void Dismiss()
        {
            if (parent == null)
            {
                return;
            }
            RemoveFromHierarchy();
            Dismissed?.Invoke();
        }
    }
}
