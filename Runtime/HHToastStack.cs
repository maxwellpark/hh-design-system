using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Toasts in the bottom-right corner of its parent, newest at the bottom pushing older ones up.
    /// UXML: <hh:HHToastStack max-visible="3" />
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
        public HHToast Show(string text, HHToastTone tone = HHToastTone.Info, float seconds = DefaultSeconds)
        {
            var toast = new HHToast { Text = text, Tone = tone };
            Add(toast);
            Trim();
            if (seconds > 0f)
            {
                toast.schedule.Execute(toast.Dismiss).StartingIn((long)(seconds * 1000f));
            }
            return toast;
        }

        private void Trim()
        {
            while (childCount > maxVisible)
            {
                ((HHToast)this[0]).Dismiss();
            }
        }
    }
}
