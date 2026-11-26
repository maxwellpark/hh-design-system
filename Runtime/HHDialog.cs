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
            // Focusable outside the tab order, so a click on the scrim focuses the dialog rather than clearing focus
            focusable = true;
            tabIndex = -1;
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
            var stops = FocusStops();
            if (stops.Count > 0)
            {
                stops[0].Focus();
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

        // Focus is mid-switch during FocusOut, so the trap looks again on the next tick, at where focus landed
        private void OnFocusOut(FocusOutEvent e)
        {
            if (!IsOpen || (e.relatedTarget is VisualElement arriving && Contains(arriving)))
            {
                return;
            }
            var leaving = e.target as VisualElement;
            schedule.Execute(() =>
            {
                var back = RefocusTarget(leaving, focusController?.focusedElement as VisualElement);
                if (back != null && back.panel != null && back.canGrabFocus)
                {
                    back.Focus();
                }
            });
        }

        /// Where focus goes back to once it has left an open dialog for `focused` (null when nothing has focus):
        /// the last stop when Shift+Tab left the first, otherwise the first. Null when it may stay: the dialog is
        /// closed or has no stops, focus is on or inside it, or focus is in an open dialog above it, which traps it instead.
        internal VisualElement RefocusTarget(VisualElement leaving, VisualElement focused)
        {
            if (!IsOpen || (focused != null && (focused == this || Contains(focused) || InOpenDialogAbove(focused))))
            {
                return null;
            }
            var stops = FocusStops();
            if (stops.Count == 0)
            {
                return null;
            }
            var first = stops[0];
            var wentBack = focused != null && leaving != null && (first == leaving || first.Contains(leaving));
            return wentBack ? stops[stops.Count - 1] : first;
        }

        // Open() brings a dialog to the front, so of two open dialogs the later in tree order is on top
        private bool InOpenDialogAbove(VisualElement element)
        {
            for (var ancestor = element.hierarchy.parent; ancestor != null; ancestor = ancestor.hierarchy.parent)
            {
                if (ancestor is HHDialog dialog && dialog.IsOpen && IsAfterInTree(dialog, this))
                {
                    return true;
                }
            }
            return false;
        }

        /// True when `a` comes after `b` in a depth-first walk of their shared tree; a descendant comes after its ancestor.
        internal static bool IsAfterInTree(VisualElement a, VisualElement b)
        {
            if (a == b)
            {
                return false;
            }
            var aPath = PathFromRoot(a);
            var bPath = PathFromRoot(b);
            var i = 0;
            while (i < aPath.Count && i < bPath.Count && aPath[i] == bPath[i])
            {
                i++;
            }
            if (i == 0)
            {
                return false;
            }
            if (i == bPath.Count)
            {
                return true;
            }
            if (i == aPath.Count)
            {
                return false;
            }
            var parent = aPath[i - 1];
            return parent.hierarchy.IndexOf(aPath[i]) > parent.hierarchy.IndexOf(bPath[i]);
        }

        private static List<VisualElement> PathFromRoot(VisualElement element)
        {
            var path = new List<VisualElement>();
            for (var node = element; node != null; node = node.hierarchy.parent)
            {
                path.Insert(0, node);
            }
            return path;
        }

        /// The dialog's tab stops in tree order: displayed elements that can take focus with a tab index of zero or
        /// more. A field that delegates its focus is one stop, not its parts; positive tab indices aren't sorted first.
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
                if (child.resolvedStyle.display == DisplayStyle.None)
                {
                    continue;
                }
                if (child.canGrabFocus && child.tabIndex >= 0)
                {
                    stops.Add(child);
                    if (child.delegatesFocus)
                    {
                        continue;
                    }
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
