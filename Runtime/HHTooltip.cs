using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Shows a label beside its target after a hover or focus delay; runtime panels don't show `tooltip` themselves.
    public class HHTooltip : Manipulator
    {
        public const string UssClassName = "hh-tooltip";
        public const long DefaultDelayMs = 500;

        private readonly string text;
        private readonly Label label;
        private IVisualElementScheduledItem pending;

        /// With no text, the target's own `tooltip` is shown.
        public HHTooltip(string text = null, long delayMs = DefaultDelayMs)
        {
            this.text = text;
            DelayMs = delayMs;
            label = new Label { pickingMode = PickingMode.Ignore };
            label.AddToClassList(UssClassName);
        }

        public long DelayMs { get; }

        public bool IsShowing => label.parent != null;

        public string Text => string.IsNullOrEmpty(text) ? target?.tooltip : text;

        /// The element the label is drawn in; the target's panel root unless a test sets another.
        internal VisualElement Layer { get; set; }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerEnterEvent>(OnEnter);
            target.RegisterCallback<FocusInEvent>(OnEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnLeave);
            target.RegisterCallback<FocusOutEvent>(OnLeave);
            target.RegisterCallback<DetachFromPanelEvent>(OnLeave);
            target.RegisterCallback<KeyDownEvent>(OnKey);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnEnter);
            target.UnregisterCallback<FocusInEvent>(OnEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnLeave);
            target.UnregisterCallback<FocusOutEvent>(OnLeave);
            target.UnregisterCallback<DetachFromPanelEvent>(OnLeave);
            target.UnregisterCallback<KeyDownEvent>(OnKey);
            Hide();
        }

        private void OnEnter(EventBase e)
        {
            pending?.Pause();
            pending = target.schedule.Execute(Show).StartingIn(DelayMs);
        }

        private void OnLeave(EventBase e)
        {
            Hide();
        }

        private void OnKey(KeyDownEvent e)
        {
            if (e.keyCode == KeyCode.Escape)
            {
                Hide();
            }
        }

        /// Shows the label below the target at once, if there's text and somewhere to draw it.
        internal void Show()
        {
            var layer = Layer ?? target?.panel?.visualTree;
            if (layer == null || string.IsNullOrEmpty(Text))
            {
                return;
            }
            label.text = Text;
            var bounds = target.worldBound;
            var origin = layer.WorldToLocal(new Vector2(bounds.xMin, bounds.yMax));
            label.style.left = origin.x;
            label.style.top = origin.y + 4f;
            layer.Add(label);
        }

        internal void Hide()
        {
            pending?.Pause();
            pending = null;
            label.RemoveFromHierarchy();
        }

        internal Label Label => label;
    }
}
