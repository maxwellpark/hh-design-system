using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// Shows a label below its target after a hover or focus delay, since runtime panels don't show `tooltip`; icon buttons have one already.
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

        /// The element the label is drawn in, where a test sets one; otherwise see TopLevel.
        internal VisualElement Layer { get; set; }

        // The target's ancestor just under the panel root: the document, which carries the HH stylesheets
        internal static VisualElement TopLevel(VisualElement element)
        {
            var top = element;
            while (top?.parent != null && top.parent != top.panel?.visualTree)
            {
                top = top.parent;
            }
            return top;
        }

        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerEnterEvent>(OnEnter);
            target.RegisterCallback<FocusInEvent>(OnEnter);
            target.RegisterCallback<PointerLeaveEvent>(OnLeave);
            target.RegisterCallback<FocusOutEvent>(OnLeave);
            target.RegisterCallback<DetachFromPanelEvent>(OnLeave);
            target.RegisterCallback<KeyDownEvent>(OnKey);
            target.RegisterCallback<PointerDownEvent>(OnLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerEnterEvent>(OnEnter);
            target.UnregisterCallback<FocusInEvent>(OnEnter);
            target.UnregisterCallback<PointerLeaveEvent>(OnLeave);
            target.UnregisterCallback<FocusOutEvent>(OnLeave);
            target.UnregisterCallback<DetachFromPanelEvent>(OnLeave);
            target.UnregisterCallback<KeyDownEvent>(OnKey);
            target.UnregisterCallback<PointerDownEvent>(OnLeave);
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
            var layer = Layer ?? (target?.panel != null ? TopLevel(target) : null);
            if (layer == null || layer == target || string.IsNullOrEmpty(Text))
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
