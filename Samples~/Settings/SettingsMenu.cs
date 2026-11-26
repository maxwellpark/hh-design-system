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
    private HHTextField playerName;
    private HHDialog discard;
    private HHToastStack toasts;
    private Settings saved = new Settings { Music = true, Volume = 0.8f, Quality = 1, PlayerName = "Northwind" };

    private struct Settings
    {
        public bool Music;
        public float Volume;
        public int Quality;
        public string PlayerName;
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
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

        music = new HHToggle { label = "Music" };
        volume = new HHSlider { label = "Volume", lowValue = 0f, highValue = 1f };
        quality = new HHDropdown { label = "Quality", choices = new List<string> { "Low", "Medium", "High" } };
        playerName = new HHTextField { label = "Name" };
        var tabs = new HHTabs();
        tabs.Add(TabWith("Audio", music, volume));
        tabs.Add(TabWith("Video", quality));
        tabs.Add(TabWith("Player", playerName));
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

    private Settings Current => new Settings { Music = music.value, Volume = volume.value, Quality = quality.index, PlayerName = playerName.value };

    private bool Changed => !Current.Equals(saved);

    private void Save()
    {
        saved = Current;
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
        playerName.value = saved.PlayerName;
    }
}
