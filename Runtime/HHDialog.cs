using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A modal: a scrim covering its parent with a card holding a title, the body (UXML children) and actions.
    /// UXML: <hh:HHDialog title="Quit?" require-choice="true"><ui:Label text="..." class="hh-text" /></hh:HHDialog>
    [UxmlElement]
    public partial class HHDialog : VisualElement
    {
        public const string UssClassName = "hh-dialog";
        public const string OpenUssClassName = UssClassName + "--open";
        public const string PanelUssClassName = UssClassName + "__panel";
        public const string TitleUssClassName = UssClassName + "__title";
        public const string BodyUssClassName = UssClassName + "__body";
        public const string ActionsUssClassName = UssClassName + "__actions";

        private readonly Label titleLabel;
        private readonly VisualElement body;
        private readonly VisualElement actions;
        private Focusable focusBeforeOpen;

        /// Raised once each time an open dialog closes, however it closed.
        public event Action Closed;

        public HHDialog()
        {
            AddToClassList(UssClassName);
            var panel = new VisualElement();
            panel.AddToClassList(PanelUssClassName);
            titleLabel = new Label();
            titleLabel.AddToClassList(TitleUssClassName);
            body = new VisualElement();
            body.AddToClassList(BodyUssClassName);
            actions = new VisualElement();
            actions.AddToClassList(ActionsUssClassName);
            panel.Add(titleLabel);
            panel.Add(body);
            panel.Add(actions);
            hierarchy.Add(panel);

            RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Escape && HandleCancel())
                {
                    e.StopPropagation();
                }
            });
            RegisterCallback<NavigationCancelEvent>(e =>
            {
                if (HandleCancel())
                {
                    e.StopPropagation();
                }
            });
            RegisterCallback<PointerDownEvent>(e => HandleScrimPointer(e.target));
            RegisterCallback<FocusOutEvent>(OnFocusOut);
        }

        public override VisualElement contentContainer => body;

        public VisualElement Actions => actions;

        public bool IsOpen => ClassListContains(OpenUssClassName);

        [UxmlAttribute]
        public string Title
        {
            get => titleLabel.text;
            set => titleLabel.text = value;
        }

        /// When set, Escape, cancel and clicks on the scrim leave the dialog open; only an action closes it.
        [UxmlAttribute]
        public bool RequireChoice { get; set; }

        /// Adds a button to the actions row; it runs onClick, then closes the dialog unless told not to.
        public HHButton AddAction(string label, Action onClick = null, bool ghost = false, bool closes = true)
        {
            var button = new HHButton(() => RunAction(onClick, closes)) { text = label, Ghost = ghost };
            actions.Add(button);
            return button;
        }

        public void Open()
        {
            if (IsOpen)
            {
                return;
            }
            focusBeforeOpen = focusController?.focusedElement;
            AddToClassList(OpenUssClassName);
            BringToFront();
            if (actions.childCount > 0)
            {
                actions[0].Focus();
            }
        }

        public void Close()
        {
            if (!IsOpen)
            {
                return;
            }
            RemoveFromClassList(OpenUssClassName);
            if (focusBeforeOpen is VisualElement previous && previous.panel != null)
            {
                previous.Focus();
            }
            focusBeforeOpen = null;
            Closed?.Invoke();
        }

        internal void RunAction(Action onClick, bool closes)
        {
            onClick?.Invoke();
            if (closes)
            {
                Close();
            }
        }

        /// True when the cancel was used to close the dialog.
        internal bool HandleCancel()
        {
            if (!IsOpen || RequireChoice)
            {
                return false;
            }
            Close();
            return true;
        }

        // Focus is mid-switch during FocusOut, so it comes back on the next tick
        private void OnFocusOut(FocusOutEvent e)
        {
            var back = RefocusTarget(e.target as VisualElement, e.relatedTarget as VisualElement);
            if (back != null)
            {
                schedule.Execute(() =>
                {
                    if (IsOpen)
                    {
                        back.Focus();
                    }
                });
            }
        }

        /// While open, focus that leaves for an element outside comes back: to the last stop when it left
        /// the first (Shift+Tab), otherwise to the first. Null when focus may go where it's going.
        internal VisualElement RefocusTarget(VisualElement leaving, VisualElement arriving)
        {
            if (!IsOpen || arriving == null || Contains(arriving))
            {
                return null;
            }
            var stops = FocusStops();
            if (stops.Count == 0)
            {
                return null;
            }
            var first = stops[0];
            return leaving != null && (first == leaving || first.Contains(leaving)) ? stops[stops.Count - 1] : first;
        }

        /// The dialog's tab stops in order: enabled, displayed, focusable elements with a tab index of
        /// zero or more, not counting the parts of a field that is itself a stop.
        internal List<VisualElement> FocusStops()
        {
            var stops = new List<VisualElement>();
            CollectStops(this, stops);
            return stops;
        }

        private static void CollectStops(VisualElement element, List<VisualElement> stops)
        {
            for (var i = 0; i < element.hierarchy.childCount; i++)
            {
                var child = element.hierarchy[i];
                if (!child.enabledInHierarchy || child.resolvedStyle.display == DisplayStyle.None)
                {
                    continue;
                }
                if (child.focusable && child.tabIndex >= 0)
                {
                    stops.Add(child);
                    continue;
                }
                CollectStops(child, stops);
            }
        }

        internal void HandleScrimPointer(IEventHandler target)
        {
            if (target == this)
            {
                HandleCancel();
            }
        }
    }
}
