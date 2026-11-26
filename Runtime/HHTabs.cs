using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A TabView with the .hh-tabs style. UXML: <hh:HHTabs><ui:Tab label="Audio" /><ui:Tab label="Video" /></hh:HHTabs>
    [UxmlElement]
    public partial class HHTabs : TabView
    {
        public const string UssClassName = "hh-tabs";

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
