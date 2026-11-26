using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// A pause menu built from the design system; put it on a GameObject with a UIDocument whose panel uses hh-theme.tss.
/// Its own Pause button stands in for a key binding, so it runs under either input system.
[RequireComponent(typeof(UIDocument))]
public class PauseMenu : MonoBehaviour
{
    private VisualElement menu;
    private HHButton pause;
    private HHButton resume;
    private HHDialog quit;
    private HHToastStack toasts;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        // The sample owns the whole document; re-enabling this component alone keeps its root, so start it empty
        root.Clear();
        root.style.flexGrow = 1;
        root.style.alignItems = Align.Center;
        root.style.justifyContent = Justify.Center;

        pause = new HHButton(Pause) { text = "Pause", Ghost = true };
        pause.style.position = Position.Absolute;
        pause.style.top = 24;
        pause.style.left = 24;
        root.Add(pause);
        root.Add(BuildMenu());
        root.Add(BuildQuitDialog());
        toasts = new HHToastStack();
        root.Add(toasts);
        Resume();
    }

    private VisualElement BuildMenu()
    {
        var card = new HHCard { Title = "Paused" };
        card.style.width = 280;
        resume = MenuButton("Resume", Resume, ghost: false);
        card.Add(resume);
        card.Add(MenuButton("Settings", () => toasts.Show("Import the Settings sample for a settings screen"), ghost: true));
        card.Add(MenuButton("Quit", () => quit.Open(), ghost: true));
        menu = card;
        return card;
    }

    private static HHButton MenuButton(string text, System.Action onClick, bool ghost)
    {
        var button = new HHButton(onClick) { text = text, Ghost = ghost };
        button.style.marginTop = 8;
        return button;
    }

    private HHDialog BuildQuitDialog()
    {
        quit = new HHDialog { Title = "Quit to the title screen?", RequireChoice = true };
        quit.Add(new Label("Progress since your last save will be lost."));
        quit.AddAction("Stay", ghost: true);
        quit.AddAction("Quit", () => toasts.Show("Quit chosen: a game would load its title scene here", HHToastTone.Warning));
        return quit;
    }

    private void Pause()
    {
        menu.style.display = DisplayStyle.Flex;
        pause.style.display = DisplayStyle.None;
        // A just-displayed element can't take focus until styles resolve, so focus it on the next tick
        resume.schedule.Execute(() => resume.Focus());
    }

    private void Resume()
    {
        menu.style.display = DisplayStyle.None;
        pause.style.display = DisplayStyle.Flex;
    }
}
