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

        private readonly Label label;
        private HHToastTone tone;

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
