using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A TabView with the .hh-tabs style. UXML: <hh:HHTabs><ui:Tab label="Audio" /><ui:Tab label="Video" /></hh:HHTabs>
    [UxmlElement]
    public partial class HHTabs : TabView
    {
        public const string UssClassName = "hh-tabs";
        public const string BadgeUssClassName = UssClassName + "__badge";

        public HHTabs()
        {
            AddToClassList(UssClassName);
            RegisterCallback<AttachToPanelEvent>(_ => MakeHeadersFocusable());
            RegisterCallback<GeometryChangedEvent>(_ => MakeHeadersFocusable());
        }

        // Unity's tab headers aren't focusable; an index loop, since this runs on every layout pass
        internal void MakeHeadersFocusable()
        {
            var tabs = contentContainer;
            for (var i = 0; i < tabs.childCount; i++)
            {
                if (tabs[i] is Tab tab)
                {
                    Wire(tab);
                }
            }
        }

        /// Shows count on the tab's header (new items in an inventory tab), or removes the badge when count is 0 or less.
        /// The badge lives on the tab's one header, which Unity only moves between parents, and is found again by class.
        public HHBadge SetBadge(Tab tab, int count)
        {
            var header = tab?.tabHeader;
            if (header == null)
            {
                return null;
            }
            var badge = header.Q<HHBadge>(className: BadgeUssClassName);
            if (count <= 0)
            {
                badge?.RemoveFromHierarchy();
                return null;
            }
            if (badge == null)
            {
                badge = new HHBadge { pickingMode = PickingMode.Ignore };
                badge.AddToClassList(BadgeUssClassName);
                // Right after the label, so a close button added later stays last
                var label = tab.headerLabel;
                header.Insert(label != null && label.parent == header ? header.IndexOf(label) + 1 : header.childCount, badge);
            }
            badge.Count = count;
            return badge;
        }

        internal void Select(Tab tab)
        {
            activeTab = tab;
        }

        private void Wire(Tab tab)
        {
            var header = tab.tabHeader;
            if (header == null || header.focusable)
            {
                return;
            }
            header.focusable = true;
            header.RegisterCallback<NavigationSubmitEvent>(_ => Select(tab));
            header.RegisterCallback<KeyDownEvent>(e =>
            {
                if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Space)
                {
                    Select(tab);
                    e.StopPropagation();
                }
            });
        }
    }
}
