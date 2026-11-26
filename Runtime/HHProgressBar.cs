using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// UXML: <hh:HHProgressBar value="40" title="Loading" tone="Pink" />
    [UxmlElement]
    public partial class HHProgressBar : ProgressBar
    {
        public const string UssClassName = "hh-progress-bar";
        public const string VioletUssClassName = UssClassName + "--violet";
        public const string PinkUssClassName = UssClassName + "--pink";
        public const string PeachUssClassName = UssClassName + "--peach";

        private HHTone tone = HHTone.Teal;

        public HHProgressBar()
        {
            AddToClassList(UssClassName);
        }

        /// Teal by default, the colour the bar had before tones.
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
