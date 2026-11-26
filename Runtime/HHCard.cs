using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem
{
    /// A surface card with an optional title; children go in its body.
    /// UXML: <hh:HHCard title="Settings"><hh:HHToggle label="Music" /></hh:HHCard>
    [UxmlElement]
    public partial class HHCard : VisualElement
    {
        public const string UssClassName = "hh-card";
        public const string TitleUssClassName = UssClassName + "__title";
        public const string BodyUssClassName = UssClassName + "__body";

        private readonly Label titleLabel;
        private readonly VisualElement body;

        public HHCard()
        {
            AddToClassList(UssClassName);
            titleLabel = new Label();
            titleLabel.AddToClassList("hh-title");
            titleLabel.AddToClassList(TitleUssClassName);
            body = new VisualElement();
            body.AddToClassList(BodyUssClassName);
            hierarchy.Add(titleLabel);
            hierarchy.Add(body);
            Title = null;
        }

        public override VisualElement contentContainer => body;

        /// The heading; empty hides it.
        [UxmlAttribute]
        public string Title
        {
            get => titleLabel.text;
            set
            {
                titleLabel.text = value;
                titleLabel.style.display = string.IsNullOrEmpty(value) ? DisplayStyle.None : DisplayStyle.Flex;
            }
        }
    }
}
