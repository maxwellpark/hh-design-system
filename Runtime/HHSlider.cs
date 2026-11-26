using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// How an HHSlider writes its value beside the track.
    public enum HHSliderValueFormat
    {
        /// Where the value sits between low and high, as a whole percentage.
        Percent,

        /// The value itself, to at most two decimal places.
        Number,
    }

    /// UXML: <hh:HHSlider label="Volume" low-value="0" high-value="1" show-value="true" value-format="Percent" />
    [UxmlElement]
    public partial class HHSlider : Slider
    {
        public const string UssClassName = "hh-slider";
        public const string ValueUssClassName = UssClassName + "__value";

        // Percent text is drawn from this table, so dragging a percent slider allocates nothing
        private static readonly string[] Percents = BuildPercents();

        private readonly Label valueLabel;
        private bool showValue;
        private HHSliderValueFormat valueFormat;

        public HHSlider()
        {
            AddToClassList(UssClassName);
            valueLabel = new Label();
            valueLabel.AddToClassList(ValueUssClassName);
            hierarchy.Add(valueLabel);
            ShowValue = false;
            // A range change that leaves the value alone doesn't set it, so follow the range too
            ((INotifyBindablePropertyChanged)this).propertyChanged += (_, e) =>
            {
                if (e.propertyName == "lowValue" || e.propertyName == "highValue")
                {
                    UpdateValueText();
                }
            };
        }

        /// Writes the value beside the track.
        [UxmlAttribute]
        public bool ShowValue
        {
            get => showValue;
            set
            {
                showValue = value;
                valueLabel.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
                UpdateValueText();
            }
        }

        [UxmlAttribute]
        public HHSliderValueFormat ValueFormat
        {
            get => valueFormat;
            set
            {
                valueFormat = value;
                UpdateValueText();
            }
        }

        /// The text beside the track, whether or not it shows.
        public string ValueText => valueLabel.text;

        public override void SetValueWithoutNotify(float newValue)
        {
            base.SetValueWithoutNotify(newValue);
            UpdateValueText();
        }

        private void UpdateValueText()
        {
            // Set during the base constructor, before the label exists
            if (valueLabel == null)
            {
                return;
            }
            if (valueFormat == HHSliderValueFormat.Percent)
            {
                var range = highValue - lowValue;
                var fraction = Mathf.Approximately(range, 0f) ? 0f : Mathf.Clamp01((value - lowValue) / range);
                if (float.IsNaN(fraction))
                {
                    fraction = 0f;
                }
                valueLabel.text = Percents[Mathf.RoundToInt(fraction * 100f)];
            }
            else
            {
                // Adding 0 turns -0 into 0, so a tiny negative value doesn't read "-0"
                var rounded = Mathf.Round(value * 100f) / 100f + 0f;
                var text = rounded.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
                if (text != valueLabel.text)
                {
                    valueLabel.text = text;
                }
            }
        }

        private static string[] BuildPercents()
        {
            var table = new string[101];
            for (var i = 0; i <= 100; i++)
            {
                table[i] = i + "%";
            }
            return table;
        }
    }
}
