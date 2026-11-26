using System.Collections.Generic;
using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// A settings screen built from the design system; put it on a GameObject with a UIDocument whose panel uses hh-theme.tss.
[RequireComponent(typeof(UIDocument))]
public class SettingsMenu : MonoBehaviour
{
    private HHToggle music;
    private HHSlider volume;
    private HHDropdown quality;
    private HHStepper resolution;
    private HHRadioGroup difficulty;
    private HHTextField playerName;
    private HHToggle reduceMotion;
    private HHToggle largeText;
    private VisualElement root;
    private HHDialog discard;
    private HHToastStack toasts;
    private Settings saved = new Settings { Music = true, Volume = 0.8f, Quality = 1, Resolution = 1, Difficulty = 1, PlayerName = "Northwind" };

    private struct Settings
    {
        public bool Music;
        public float Volume;
        public int Quality;
        public int Resolution;
        public int Difficulty;
        public string PlayerName;
        public bool ReduceMotion;
        public bool LargeText;
    }

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        // Re-enabling this component alone keeps the document's root, so start it empty
        root.Clear();
        root.style.paddingTop = 40;
        root.style.paddingLeft = 40;
        root.Add(BuildCard());
        root.Add(BuildDialog());
        toasts = new HHToastStack();
        root.Add(toasts);
        Revert();
    }

    private VisualElement BuildCard()
    {
        var card = new HHCard { Title = "Settings" };
        card.style.width = 420;

        music = new HHToggle { label = "Music", IsSwitch = true };
        volume = new HHSlider { label = "Volume", lowValue = 0f, highValue = 1f, ShowValue = true };
        quality = new HHDropdown { label = "Quality", choices = new List<string> { "Low", "Medium", "High" } };
        resolution = new HHStepper("Resolution", new List<string> { "1280x720", "1920x1080", "2560x1440", "3840x2160" });
        difficulty = new HHRadioGroup("Difficulty", new List<string> { "Story", "Normal", "Hard" });
        playerName = new HHTextField { label = "Name" };
        reduceMotion = new HHToggle { label = "Reduce motion" };
        largeText = new HHToggle { label = "Large text" };
        var tabs = new HHTabs();
        tabs.Add(TabWith("Audio", music, volume));
        tabs.Add(TabWith("Video", quality, resolution));
        tabs.Add(TabWith("Player", playerName, difficulty));
        tabs.Add(TabWith("Access", reduceMotion, largeText));
        card.Add(tabs);
        card.Add(BuildActions());
        return card;
    }

    private VisualElement BuildActions()
    {
        var actions = new VisualElement();
        actions.style.flexDirection = FlexDirection.Row;
        actions.style.justifyContent = Justify.FlexEnd;
        actions.style.marginTop = 16;
        actions.Add(new HHButton(Back) { text = "Back", Ghost = true });
        actions.Add(new HHButton(Save) { text = "Save" });
        return actions;
    }

    private HHDialog BuildDialog()
    {
        discard = new HHDialog { Title = "Discard changes?" };
        discard.Add(new Label("Your changes to settings haven't been saved."));
        discard.AddAction("Keep editing", ghost: true);
        discard.AddAction("Discard", Revert);
        return discard;
    }

    private static Tab TabWith(string label, params VisualElement[] controls)
    {
        var tab = new Tab(label);
        foreach (var control in controls)
        {
            control.style.marginBottom = 8;
            tab.Add(control);
        }
        return tab;
    }

    private Settings Current => new Settings
    {
        Music = music.value,
        Volume = volume.value,
        Quality = quality.index,
        Resolution = resolution.Index,
        Difficulty = difficulty.value,
        PlayerName = playerName.value,
        ReduceMotion = reduceMotion.value,
        LargeText = largeText.value,
    };

    private bool Changed => !Current.Equals(saved);

    private void Save()
    {
        saved = Current;
        ApplyAccess();
        toasts.Show("Settings saved", HHToastTone.Success);
    }

    private void Back()
    {
        if (Changed)
        {
            discard.Open();
        }
    }

    private void Revert()
    {
        music.value = saved.Music;
        volume.value = saved.Volume;
        quality.index = saved.Quality;
        resolution.Index = saved.Resolution;
        difficulty.value = saved.Difficulty;
        playerName.value = saved.PlayerName;
        reduceMotion.value = saved.ReduceMotion;
        largeText.value = saved.LargeText;
        ApplyAccess();
    }

    // Saved settings apply to the whole document, so every control follows them
    private void ApplyAccess()
    {
        HHAccessibility.SetReducedMotion(root, saved.ReduceMotion);
        HHAccessibility.SetLargeText(root, saved.LargeText);
    }
}
