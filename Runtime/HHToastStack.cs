using System;
using System.Linq;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Toasts in its parent's bottom-right corner, newest last. UXML: <hh:HHToastStack max-visible="3" />
    [UxmlElement]
    public partial class HHToastStack : VisualElement
    {
        public const string UssClassName = "hh-toast-stack";
        public const float DefaultSeconds = 4f;

        private int maxVisible = 4;

        public HHToastStack()
        {
            AddToClassList(UssClassName);
            pickingMode = PickingMode.Ignore;
        }

        /// The oldest toasts are dismissed once more than this many show; at least 1.
        [UxmlAttribute]
        public int MaxVisible
        {
            get => maxVisible;
            set
            {
                maxVisible = value < 1 ? 1 : value;
                Trim();
            }
        }

        /// Shows a toast that dismisses itself after the given seconds, or stays until dismissed if seconds is 0.
        /// With an action label it carries a button that runs onAction and dismisses it.
        public HHToast Show(string text, HHToastTone tone = HHToastTone.Info, float seconds = DefaultSeconds, string actionLabel = null, Action onAction = null)
        {
            var toast = new HHToast { Text = text, Tone = tone };
            if (actionLabel != null)
            {
                toast.SetAction(actionLabel, onAction);
            }
            Add(toast);
            Trim();
            if (seconds > 0f)
            {
                toast.schedule.Execute(toast.Dismiss).StartingIn((long)(seconds * 1000f));
            }
            return toast;
        }

        // Counts and dismisses only toasts, so other children placed in the stack are left alone
        private void Trim()
        {
            var toasts = Children().OfType<HHToast>().ToList();
            for (var i = 0; i < toasts.Count - maxVisible; i++)
            {
                toasts[i].Dismiss();
            }
        }
    }
}
