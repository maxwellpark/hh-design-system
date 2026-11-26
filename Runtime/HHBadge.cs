using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHBadge count="3" tone="Pink" /> or <hh:HHBadge dot="true" />
    [UxmlElement]
    public partial class HHBadge : Label
    {
        public const string UssClassName = "hh-badge";
        public const string DotUssClassName = UssClassName + "--dot";
        public const string TealUssClassName = UssClassName + "--teal";
        public const string PinkUssClassName = UssClassName + "--pink";
        public const string PeachUssClassName = UssClassName + "--peach";
        public const int MaxShown = 99;

        private int count;
        private HHTone tone;

        public HHBadge()
        {
            AddToClassList(UssClassName);
            Count = 0;
        }

        /// Shown as text, capped at "99+"; ignored while the badge is a dot.
        [UxmlAttribute]
        public int Count
        {
            get => count;
            set
            {
                count = value < 0 ? 0 : value;
                text = Dot ? string.Empty : count > MaxShown ? $"{MaxShown}+" : count.ToString();
            }
        }

        [UxmlAttribute]
        public bool Dot
        {
            get => ClassListContains(DotUssClassName);
            set
            {
                EnableInClassList(DotUssClassName, value);
                Count = count;
            }
        }

        [UxmlAttribute]
        public HHTone Tone
        {
            get => tone;
            set
            {
                tone = value;
                EnableInClassList(TealUssClassName, value == HHTone.Teal);
                EnableInClassList(PinkUssClassName, value == HHTone.Pink);
                EnableInClassList(PeachUssClassName, value == HHTone.Peach);
            }
        }
    }
}
