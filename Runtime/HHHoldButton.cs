using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A button that confirms only after it is held for HoldSeconds (quit, skip), with a bar filling along its foot.
    /// Listen to Confirmed: it has no Clickable, so `clicked` never fires and a pad's submit, which has no release,
    /// does nothing. The hold time is its own attribute, not a duration token, so reduced motion never shortens it.
    /// UXML: <hh:HHHoldButton text="Hold to quit" hold-seconds="1.5" />
    [UxmlElement]
    public partial class HHHoldButton : Button
    {
        public const string UssClassName = "hh-hold-button";
        public const string FillUssClassName = UssClassName + "__fill";
        private const long TickMs = 16;
        private const int NoPointer = -1;

        private readonly VisualElement fill;
        private IVisualElementScheduledItem ticker;
        private float holdSeconds = 1f;
        private float held;
        // The scheduler's clock at this hold's last tick, or -1 before its first
        private long lastTickMs = -1;
        private int holdPointer = NoPointer;
        private KeyCode holdKey = KeyCode.None;
        // Set when a hold confirms, so the still-pressed key or pointer can't start another until it is released
        private bool awaitRelease;

        /// Raised once each time a hold reaches HoldSeconds.
        public event Action Confirmed;

        public HHHoldButton()
        {
            AddToClassList("hh-button");
            AddToClassList(UssClassName);
            clickable = null;
            fill = new VisualElement { pickingMode = PickingMode.Ignore };
            fill.AddToClassList(FillUssClassName);
            Add(fill);
            SetFill(0f);
            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerUpEvent>(e => ReleasePointer(e.pointerId));
            RegisterCallback<PointerLeaveEvent>(e => ReleasePointer(e.pointerId));
            RegisterCallback<PointerCancelEvent>(e => ReleasePointer(e.pointerId));
            RegisterCallback<PointerCaptureOutEvent>(e => ReleasePointer(e.pointerId));
            RegisterCallback<KeyDownEvent>(OnKeyDown);
            RegisterCallback<KeyUpEvent>(e =>
            {
                if (e.keyCode == holdKey)
                {
                    holdKey = KeyCode.None;
                    Release();
                }
            });
            RegisterCallback<FocusOutEvent>(_ => Release());
            RegisterCallback<DetachFromPanelEvent>(_ => Release());
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

        private void OnPointerDown(PointerDownEvent e)
        {
            if (e.button != 0 || holdPointer != NoPointer)
            {
                return;
            }
            holdPointer = e.pointerId;
            this.CapturePointer(e.pointerId);
            Begin();
        }

        private void OnKeyDown(KeyDownEvent e)
        {
            if (e.keyCode != KeyCode.Space && e.keyCode != KeyCode.Return && e.keyCode != KeyCode.KeypadEnter)
            {
                return;
            }
            if (holdKey == KeyCode.None)
            {
                holdKey = e.keyCode;
                Begin();
            }
            e.StopPropagation();
        }

        private void ReleasePointer(int pointerId)
        {
            if (pointerId != holdPointer)
            {
                return;
            }
            holdPointer = NoPointer;
            if (this.HasPointerCapture(pointerId))
            {
                this.ReleasePointer(pointerId);
            }
            Release();
        }

        internal void Begin()
        {
            if (IsHolding || awaitRelease || !enabledInHierarchy)
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
            if (!enabledInHierarchy)
            {
                Cancel();
                return false;
            }
            held += Mathf.Max(0f, elapsedSeconds);
            if (held < holdSeconds)
            {
                SetFill(held / holdSeconds);
                return false;
            }
            Cancel();
            awaitRelease = true;
            Confirmed?.Invoke();
            return true;
        }

        /// The key or pointer let go: stops any hold, and allows the next.
        internal void Release()
        {
            awaitRelease = false;
            Cancel();
        }

        internal void Cancel()
        {
            if (!IsHolding)
            {
                return;
            }
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
