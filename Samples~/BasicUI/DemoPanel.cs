using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// Drop this on a GameObject with a UIDocument and press Play; assign the USS unless the panel uses hh-theme.tss.
[RequireComponent(typeof(UIDocument))]
public class DemoPanel : MonoBehaviour
{
    public StyleSheet tokens;      // Runtime/Styles/hh-tokens.uss
    public StyleSheet components;  // Runtime/Styles/hh-components.uss
    public ThemeStyleSheet[] themes;  // hh-theme.tss, hh-theme-high-contrast.tss; empty hides the Theme button

    private HHProgressBar progress;
    private UIDocument document;
    private PanelSettings panelCopy;

    private bool CyclesThemes => themes != null && themes.Length > 0;

    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        // A runtime copy before building, so cycling themes in Play mode doesn't rewrite the PanelSettings asset
        if (CyclesThemes && panelCopy == null && document.panelSettings != null)
        {
            panelCopy = Instantiate(document.panelSettings);
            document.panelSettings = panelCopy;
        }
        var root = document.rootVisualElement;
        // Sheets on the root would outrank the panel's theme, so the theme alone styles a cycling panel
        if (!CyclesThemes)
        {
            HHStyles.Apply(root, tokens, components);
        }
        root.style.paddingTop = 40;
        root.style.paddingLeft = 40;

        var card = new HHCard { Title = "Hooton Heroics" };
        card.style.width = 320;

        var sub = new Label("Design system demo");
        sub.AddToClassList("hh-text-dim");

        progress = new HHProgressBar { title = "Loading", value = 40 };
        progress.style.marginTop = 16;

        var play = new HHButton(() => progress.value = Mathf.Min(progress.value + 10, progress.highValue)) { text = "Play" };
        play.style.marginTop = 16;

        var back = new HHButton(() => progress.value = progress.lowValue) { text = "Reset", Ghost = true };
        back.style.marginTop = 8;

        card.Add(sub);
        card.Add(progress);
        card.Add(play);
        card.Add(back);
        if (CyclesThemes)
        {
            var theme = new HHButton(() => HHThemes.Next(document.panelSettings, themes)) { text = "Theme", Ghost = true };
            theme.style.marginTop = 8;
            card.Add(theme);
        }
        root.Add(card);
    }

    private void OnDestroy()
    {
        if (panelCopy != null)
        {
            Destroy(panelCopy);
        }
    }
}
