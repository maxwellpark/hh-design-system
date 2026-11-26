using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace HootonHeroics.DesignSystem.Editor
{
    /// Every control in its states, under a chosen token file: Hooton Heroics > Design System > Gallery.
    public class HHGallery : EditorWindow
    {
        public const string StylesPath = "Packages/com.hootonheroics.designsystem/Runtime/Styles";
        public const string PageUssClassName = "hh-page";

        [MenuItem("Hooton Heroics/Design System/Gallery")]
        public static void Open()
        {
            GetWindow<HHGallery>("HH Gallery");
        }

        /// The token files a gallery can preview: hh-tokens.uss first, then each theme's.
        public static IReadOnlyList<string> TokenFiles()
        {
            var files = AssetDatabase.FindAssets("t:StyleSheet hh-tokens", new[] { StylesPath })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => System.IO.Path.GetFileName(path).StartsWith("hh-tokens") && path.EndsWith(".uss"))
                .OrderBy(path => path.EndsWith("/hh-tokens.uss") ? 0 : 1)
                .ThenBy(path => path)
                .ToList();
            return files;
        }

        public void CreateGUI()
        {
            var files = TokenFiles();
            var picker = new DropdownField("Theme", files.Select(System.IO.Path.GetFileNameWithoutExtension).ToList(), 0);
            var page = new VisualElement();
            var reduceMotion = new Toggle("Reduce motion");
            var largeText = new Toggle("Large text");
            reduceMotion.RegisterValueChangedCallback(e => HHAccessibility.SetReducedMotion(rootVisualElement, e.newValue));
            largeText.RegisterValueChangedCallback(e => HHAccessibility.SetLargeText(rootVisualElement, e.newValue));
            rootVisualElement.Add(picker);
            rootVisualElement.Add(reduceMotion);
            rootVisualElement.Add(largeText);
            rootVisualElement.Add(page);
            // On the window root, so the tokens' :root and the dropdown list rules reach everything in it
            picker.RegisterValueChangedCallback(_ => Style(rootVisualElement, files.ElementAtOrDefault(picker.index)));
            Style(rootVisualElement, files.FirstOrDefault());
            Build(page);
        }

        private static void Style(VisualElement target, string tokensPath)
        {
            target.styleSheets.Clear();
            foreach (var path in new[] { tokensPath, $"{StylesPath}/hh-components.uss", $"{StylesPath}/hh-dropdown-menu.uss" })
            {
                var sheet = path == null ? null : AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
                if (sheet != null)
                {
                    target.styleSheets.Add(sheet);
                }
            }
        }

        /// Fills the page with every control, so tests can check it without opening a window.
        public static void Build(VisualElement page)
        {
            page.style.flexGrow = 1;
            page.AddToClassList(PageUssClassName);
            var scroll = new HHScrollView();
            scroll.style.flexGrow = 1;
            page.Add(scroll);

            var buttons = Section(scroll, "Buttons");
            buttons.Add(new HHButton { text = "Play" });
            buttons.Add(new HHButton { text = "Back", Ghost = true });
            var disabled = new HHButton { text = "Locked" };
            disabled.SetEnabled(false);
            buttons.Add(disabled);
            buttons.Add(new HHIconButton(null, "Settings", null));

            var badges = Section(scroll, "Badges");
            badges.Add(new HHBadge { Count = 3 });
            badges.Add(new HHBadge { Count = 120, Tone = HHTone.Pink });
            badges.Add(new HHBadge { Count = 7, Tone = HHTone.Teal });
            badges.Add(new HHBadge { Count = 1, Tone = HHTone.Peach });
            badges.Add(new HHBadge { Dot = true });

            var progress = Section(scroll, "Progress");
            progress.style.flexDirection = FlexDirection.Column;
            progress.Add(new HHProgressBar { title = "Loading", value = 40 });
            progress.Add(new HHProgressBar { title = "Health", value = 70, Tone = HHTone.Pink });
            progress.Add(new HHSegmentedBar { Segments = 5, Filled = 3, Tone = HHTone.Peach });

            var form = Section(scroll, "Form controls");
            form.style.flexDirection = FlexDirection.Column;
            form.Add(new HHToggle { label = "Music", value = true });
            var offToggle = new HHToggle { label = "Subtitles" };
            offToggle.SetEnabled(false);
            form.Add(offToggle);
            form.Add(new HHToggle { label = "Vibration", IsSwitch = true, value = true });
            form.Add(new HHToggle { label = "Hints", IsSwitch = true });
            form.Add(new HHSlider { label = "Volume", lowValue = 0, highValue = 1, value = 0.6f, ShowValue = true });
            form.Add(new HHDropdown { label = "Quality", choices = new List<string> { "Low", "High" }, index = 1 });
            form.Add(new HHTextField { label = "Name", value = "Northwind" });
            var callsign = new HHTextField { label = "Callsign" };
            callsign.textEdition.placeholder = "Pick a callsign";
            form.Add(callsign);
            form.Add(new HHStepper("Resolution", new List<string> { "720p", "1080p", "1440p", "4K" }) { Index = 1 });
            form.Add(new HHRadioGroup("Difficulty", new List<string> { "Easy", "Normal", "Hard" }) { value = 1 });

            var tabs = new HHTabs();
            tabs.Add(new Tab("Audio"));
            tabs.Add(new Tab("Video"));
            Section(scroll, "Tabs").Add(tabs);

            var rows = new List<string> { "Northwind", "Outfitters", "Digital" };
            var list = new HHListView
            {
                fixedItemHeight = 32,
                itemsSource = rows,
                makeItem = () => new Label(),
                bindItem = (element, index) => ((Label)element).text = rows[index],
            };
            list.style.height = 110;
            Section(scroll, "List").Add(list);

            BuildDisabled(Section(scroll, "Disabled"));

            var overlays = Section(scroll, "Overlays");
            var dialog = new HHDialog { Title = "Quit?" };
            dialog.Add(new Label("Unsaved progress will be lost.") { name = "dialog-body" });
            dialog.AddAction("Stay", ghost: true);
            dialog.AddAction("Quit");
            var toasts = new HHToastStack();
            overlays.Add(new HHButton(dialog.Open) { text = "Open dialog" });
            overlays.Add(new HHButton(() => toasts.Show("Saved", HHToastTone.Success)) { text = "Show toast" });
            overlays.Add(new HHToast { Text = "A resting toast", Tone = HHToastTone.Warning });
            page.Add(dialog);
            page.Add(toasts);
        }

        // Every focusable control once more, disabled, so each :disabled look can be seen; check.mjs holds this row
        private static void BuildDisabled(VisualElement row)
        {
            row.style.flexDirection = FlexDirection.Column;
            row.style.alignItems = Align.Stretch;
            var iconButton = new HHIconButton(null, "Locked settings", null);
            var slider = new HHSlider { label = "Volume", lowValue = 0, highValue = 1, value = 0.3f };
            var dropdown = new HHDropdown { label = "Quality", choices = new List<string> { "Low", "High" }, index = 0 };
            var textField = new HHTextField { label = "Name", value = "Northwind" };
            var radio = new HHRadioGroup("Difficulty", new List<string> { "Easy", "Hard" }) { value = 0 };
            var stepper = new HHStepper("Resolution", new List<string> { "720p", "1080p" }) { Index = 0 };
            var lockedTabs = new HHTabs();
            lockedTabs.Add(new Tab("Locked"));
            var lockedList = new HHListView { fixedItemHeight = 32, itemsSource = new List<string> { "Locked" }, makeItem = () => new Label(), bindItem = (e, i) => ((Label)e).text = "Locked" };
            lockedList.style.height = 40;
            var scroller = new HHScrollView();
            scroller.style.height = 40;
            scroller.Add(new Label("Locked"));
            foreach (var control in new VisualElement[] { iconButton, slider, dropdown, textField, radio, stepper, lockedTabs, lockedList, scroller })
            {
                row.Add(control);
            }
            iconButton.SetEnabled(false);
            slider.SetEnabled(false);
            dropdown.SetEnabled(false);
            textField.SetEnabled(false);
            radio.SetEnabled(false);
            stepper.SetEnabled(false);
            lockedTabs.SetEnabled(false);
            lockedList.SetEnabled(false);
            scroller.SetEnabled(false);
        }

        private static VisualElement Section(VisualElement parent, string title)
        {
            var card = new HHCard { Title = title };
            card.style.marginBottom = 12;
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.flexWrap = Wrap.Wrap;
            row.style.alignItems = Align.Center;
            card.Add(row);
            parent.Add(card);
            return row;
        }
    }
}
