using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A bar of separate segments, for health, ammo or charges. UXML: <hh:HHSegmentedBar segments="5" filled="3" tone="Pink" />
    [UxmlElement]
    public partial class HHSegmentedBar : VisualElement
    {
        public const string UssClassName = "hh-segmented-bar";
        public const string SegmentUssClassName = UssClassName + "__segment";
        public const string FilledUssClassName = SegmentUssClassName + "--filled";
        public const string VioletUssClassName = UssClassName + "--violet";
        public const string PinkUssClassName = UssClassName + "--pink";
        public const string PeachUssClassName = UssClassName + "--peach";
        public const int MaxSegments = 50;

        private int segments;
        private int filled;
        private HHTone tone = HHTone.Teal;

        public HHSegmentedBar()
        {
            AddToClassList(UssClassName);
            Segments = 5;
        }

        /// Clamped to 1 to 50; extra segments are added or removed to match.
        [UxmlAttribute]
        public int Segments
        {
            get => segments;
            set
            {
                segments = value < 1 ? 1 : value > MaxSegments ? MaxSegments : value;
                while (childCount < segments)
                {
                    var segment = new VisualElement();
                    segment.AddToClassList(SegmentUssClassName);
                    Add(segment);
                }
                while (childCount > segments)
                {
                    RemoveAt(childCount - 1);
                }
                Filled = filled;
            }
        }

        /// Clamped to 0 to Segments.
        [UxmlAttribute]
        public int Filled
        {
            get => filled;
            set
            {
                filled = value < 0 ? 0 : value > segments ? segments : value;
                for (var i = 0; i < childCount; i++)
                {
                    this[i].EnableInClassList(FilledUssClassName, i < filled);
                }
            }
        }

        [UxmlAttribute]
        public HHTone Tone
        {
            get => tone;
            set
            {
                tone = value;
                HHToneClasses.Apply(this, UssClassName, value, HHTone.Teal);
            }
        }
    }
}
