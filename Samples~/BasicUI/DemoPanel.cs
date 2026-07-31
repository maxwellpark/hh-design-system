using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// Drop this on a GameObject with a UIDocument, assign the two USS assets from the package,
/// and press Play to see the tokens + components. Builds the panel in code (no scene needed).
[RequireComponent(typeof(UIDocument))]
public class DemoPanel : MonoBehaviour
{
    public StyleSheet tokens;      // Runtime/Styles/hh-tokens.uss
    public StyleSheet components;  // Runtime/Styles/hh-components.uss

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        HHStyles.Apply(root, tokens, components);
        root.style.paddingTop = 40;
        root.style.paddingLeft = 40;

        var card = new VisualElement();
        card.AddToClassList("hh-card");
        card.style.width = 320;

        var title = new Label("Hooton Heroics");
        title.AddToClassList("hh-title");

        var sub = new Label("Design system demo");
        sub.AddToClassList("hh-text-dim");

        var play = new HHButton { text = "Play" };
        play.style.marginTop = 16;

        card.Add(title);
        card.Add(sub);
        card.Add(play);
        root.Add(card);
    }
}
