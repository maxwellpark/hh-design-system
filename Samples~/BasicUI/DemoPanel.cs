using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// Drop this on a GameObject with a UIDocument, assign the two USS assets and press Play.
[RequireComponent(typeof(UIDocument))]
public class DemoPanel : MonoBehaviour
{
    public StyleSheet tokens;      // Runtime/Styles/hh-tokens.uss
    public StyleSheet components;  // Runtime/Styles/hh-components.uss

    private HHProgressBar progress;

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

        progress = new HHProgressBar { title = "Loading", value = 40 };
        progress.style.marginTop = 16;

        var play = new HHButton(() => progress.value = Mathf.Min(progress.value + 10, progress.highValue)) { text = "Play" };
        play.style.marginTop = 16;

        var back = new HHButton(() => progress.value = progress.lowValue) { text = "Reset", Ghost = true };
        back.style.marginTop = 8;

        card.Add(title);
        card.Add(sub);
        card.Add(progress);
        card.Add(play);
        card.Add(back);
        root.Add(card);
    }
}
