using System;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A Button with the .hh-button style. UXML: <hh:HHButton text="Play" ghost="true" />
    [UxmlElement]
    public partial class HHButton : Button
    {
        public const string UssClassName = "hh-button";
        public const string GhostUssClassName = UssClassName + "--ghost";

        public HHButton() : this(null) { }

        public HHButton(Action clickEvent) : base(clickEvent)
        {
            AddToClassList(UssClassName);
        }

        /// The low-emphasis variant: tinted background, violet text.
        [UxmlAttribute]
        public bool Ghost
        {
            get => ClassListContains(GhostUssClassName);
            set => EnableInClassList(GhostUssClassName, value);
        }
    }
}
