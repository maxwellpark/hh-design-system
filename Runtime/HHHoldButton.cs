using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A button that confirms only after it is held for HoldSeconds (quit, skip), with a bar filling along its foot.
    /// The hold time is its own attribute, not a duration token, so reduced motion never shortens it.
    /// UXML: <hh:HHHoldButton text="Hold to quit" hold-seconds="1.5" />
    [UxmlElement]
    public partial class HHHoldButton : Button
    {
        public const string UssClassName = "hh-hold-button";
        public const string FillUssClassName = UssClassName + "__fill";
        private const long TickMs = 16;

        private readonly VisualElement fill;
        private IVisualElementScheduledItem ticker;
        private float holdSeconds = 1f;
        private float held;
        // The scheduler's clock at the last tick of this hold, or -1 before its first; a resumed item's own delta can span the pause
        private long lastTickMs = -1;

        /// Raised once each time a hold reaches HoldSeconds.
        public event Action Confirmed;

        public HHHoldButton()
        {
            AddToClassList("hh-button");
            AddToClassList(UssClassName);
            fill = new VisualElement { pickingMode = PickingMode.Ignore };
            fill.AddToClassList(FillUssClassName);
            Add(fill);
            SetFill(0f);
            // Trickle down, so the hold starts before the button's own click handling captures the pointer
            RegisterCallback<PointerDownEvent>(_ => Begin(), TrickleDown.TrickleDown);
            RegisterCallback<PointerUpEvent>(_ => Cancel());
            RegisterCallback<PointerLeaveEvent>(_ => Cancel());
            RegisterCallback<PointerCancelEvent>(_ => Cancel());
            RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Space || e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
                {
                    Begin();
                    e.StopPropagation();
                }
            });
            RegisterCallback<KeyUpEvent>(_ => Cancel());
            RegisterCallback<FocusOutEvent>(_ => Cancel());
            RegisterCallback<DetachFromPanelEvent>(_ => Cancel());
        }

        /// Seconds the button must be held, at least 0.1.
        [UxmlAttribute]
        public float HoldSeconds
        {
            get => holdSeconds;
            set => holdSeconds = Mathf.Max(0.1f, value);
        }

        public bool IsHolding { get; private set; }

        /// How far the current hold has got, from 0 to 1.
        public float Progress => IsHolding ? Mathf.Clamp01(held / holdSeconds) : 0f;

        internal void Begin()
        {
            if (IsHolding || !enabledInHierarchy)
            {
                return;
            }
            IsHolding = true;
            held = 0f;
            SetFill(0f);
            if (panel != null)
            {
                // One scheduled item for the button's life, paused between holds, so holding allocates nothing per frame
                lastTickMs = -1;
                ticker ??= schedule.Execute(Tick).Every(TickMs);
                ticker.Resume();
            }
        }

        private void Tick(TimerState state)
        {
            if (lastTickMs >= 0)
            {
                Advance((state.now - lastTickMs) / 1000f);
            }
            lastTickMs = state.now;
        }

        /// Moves the hold on by elapsedSeconds; true when that completes it, which raises Confirmed once.
        internal bool Advance(float elapsedSeconds)
        {
            if (!IsHolding)
            {
                return false;
            }
            held += Mathf.Max(0f, elapsedSeconds);
            if (held < holdSeconds)
            {
                SetFill(held / holdSeconds);
                return false;
            }
            Cancel();
            Confirmed?.Invoke();
            return true;
        }

        internal void Cancel()
        {
            IsHolding = false;
            held = 0f;
            ticker?.Pause();
            SetFill(0f);
        }

        private void SetFill(float fraction)
        {
            fill.style.width = Length.Percent(Mathf.Clamp01(fraction) * 100f);
        }
    }
}
