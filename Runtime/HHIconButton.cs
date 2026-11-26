using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHIconButton icon="project://database/Assets/Icons/gear.png" tooltip="Settings" />
    [UxmlElement]
    public partial class HHIconButton : Button
    {
        public const string UssClassName = "hh-icon-button";

        private Texture2D icon;

        public HHIconButton() : this(null, null, null) { }

        /// The label becomes the tooltip, the button's only text, so give every icon button one.
        public HHIconButton(Texture2D icon, string label, Action clickEvent) : base(clickEvent)
        {
            AddToClassList(UssClassName);
            Icon = icon;
            tooltip = label;
        }

        [UxmlAttribute]
        public Texture2D Icon
        {
            get => icon;
            set
            {
                icon = value;
                style.backgroundImage = value != null ? new StyleBackground(value) : new StyleBackground(StyleKeyword.Null);
            }
        }
    }
}
