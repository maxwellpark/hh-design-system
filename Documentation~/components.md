# Component reference

Every control in the package: its UXML, the USS classes it carries, its attributes, its states and the tokens its rules use. Declare the namespaces once per UXML file: `xmlns:ui="UnityEngine.UIElements"` and `xmlns:hh="HootonHeroics.DesignSystem"`. `node Tools~/check.mjs` fails when a control, class, attribute or token here falls out of step with the code and `hh-components.uss`.

## HHButton

The main action button, with a ghost style for secondary actions. Extends `Button`.

```xml
<hh:HHButton text="Play" />
<hh:HHButton text="Back" ghost="true" />
```

- **Classes:** `.hh-button`, `.hh-button--ghost`
- **Attributes:**
  - `ghost` (bool, default false): A faint violet fill with violet text.
- **States:** `:hover`, `:active`, `:focus` (focus ring), `:disabled`.
- **Tokens:** `--hh-color-button`, `--hh-color-button-hover`, `--hh-color-focus`, `--hh-color-text`, `--hh-color-violet-ghost`, `--hh-color-violet-ghost-hover`, `--hh-color-violet-light`, `--hh-duration-fast`, `--hh-easing-standard`, `--hh-radius-md`, `--hh-space-2`, `--hh-space-3`, `--hh-space-6`

## HHIconButton

A square button showing an icon, with its label as its tooltip. Extends `Button`.

```xml
<hh:HHIconButton icon="project://database/Assets/Icons/gear.png" tooltip="Settings" />
```

- **Classes:** `.hh-icon-button`
- **Attributes:**
  - `icon` (Texture2D, default none): The image drawn in the button.
- **States:** `:hover`, `:active`, `:focus`, `:disabled`. Hovering or focusing shows its `HHTooltip`.
- **Tokens:** `--hh-color-focus`, `--hh-color-violet-ghost`, `--hh-color-violet-ghost-hover`, `--hh-color-violet-light`, `--hh-duration-fast`, `--hh-easing-standard`, `--hh-radius-md`

## HHBadge

A count or dot pinned to something that needs attention. Extends `Label`.

```xml
<hh:HHBadge count="3" />
<hh:HHBadge dot="true" tone="Pink" />
```

- **Classes:** `.hh-badge`, `.hh-badge--dot`, `.hh-badge--teal`, `.hh-badge--pink`, `.hh-badge--peach`
- **Attributes:**
  - `count` (int, default 0): Shown as text; above 99 it reads `99+`; ignored while `dot` is set.
  - `dot` (bool, default false): A small dot with no number.
  - `tone` (HHTone, default Violet): Violet, Teal, Pink or Peach.
- **States:** No interactive states. Tone and dot are classes.
- **Tokens:** `--hh-color-bg`, `--hh-color-peach`, `--hh-color-pink`, `--hh-color-teal`, `--hh-color-violet-light`, `--hh-font-size-sm`, `--hh-space-1`

## HHProgressBar

A labelled progress bar. Extends `ProgressBar`.

```xml
<hh:HHProgressBar title="Loading" value="40" />
<hh:HHProgressBar title="Health" value="70" tone="Pink" />
```

- **Classes:** `.hh-progress-bar`, `.hh-progress-bar--violet`, `.hh-progress-bar--pink`, `.hh-progress-bar--peach`
- **Attributes:**
  - `tone` (HHTone, default Teal): The fill colour.
- **States:** No interactive states. The fill width animates over `--hh-duration-normal`.
- **Tokens:** `--hh-color-peach`, `--hh-color-pink`, `--hh-color-teal`, `--hh-color-text`, `--hh-color-track`, `--hh-color-violet-light`, `--hh-duration-normal`, `--hh-easing-standard`, `--hh-font-size-sm`, `--hh-radius-sm`, `--hh-space-1`, `--hh-space-3`, `--hh-space-6`

## HHSegmentedBar

A bar of discrete segments, for lives, charges or steps. Extends `VisualElement`.

```xml
<hh:HHSegmentedBar segments="5" filled="3" tone="Peach" />
```

- **Classes:** `.hh-segmented-bar`, `.hh-segmented-bar__segment`, `.hh-segmented-bar__segment--filled`, `.hh-segmented-bar--violet`, `.hh-segmented-bar--pink`, `.hh-segmented-bar--peach`
- **Attributes:**
  - `segments` (int, default 5): How many segments, 1 to 50.
  - `filled` (int, default 0): How many are lit, clamped to `segments`; a larger value is kept and applied once `segments` grows.
  - `tone` (HHTone, default Teal): The lit colour.
- **States:** Each lit segment has `.hh-segmented-bar__segment--filled`.
- **Tokens:** `--hh-color-peach`, `--hh-color-pink`, `--hh-color-teal`, `--hh-color-track`, `--hh-color-violet-light`, `--hh-radius-sm`, `--hh-space-1`, `--hh-space-3`

## HHToggle

An on/off checkbox with a label. Extends `Toggle`.

```xml
<hh:HHToggle label="Music" value="true" />
```

- **Classes:** `.hh-toggle`
- **States:** `:checked` fills the box, plus `:hover`, `:focus` and `:disabled`.
- **Tokens:** `--hh-color-bg`, `--hh-color-focus`, `--hh-color-text`, `--hh-color-violet`, `--hh-color-violet-light`, `--hh-font-size-md`, `--hh-radius-sm`

## HHSlider

A horizontal value slider with a label. Extends `Slider`.

```xml
<hh:HHSlider label="Volume" low-value="0" high-value="1" value="0.6" />
```

- **Classes:** `.hh-slider`
- **States:** `:focus` (dragger border) and `:disabled`; hover and active keep the resting look.
- **Tokens:** `--hh-color-teal`, `--hh-color-text`, `--hh-color-track`, `--hh-font-size-md`, `--hh-radius-sm`

## HHDropdown

A labelled choice list. Its open menu is drawn at the panel root, so its rules live in `hh-dropdown-menu.uss`, which only the theme (`hh-theme.tss`) imports. Extends `DropdownField`.

```xml
<hh:HHDropdown label="Quality" choices="Low,High" index="1" />
```

- **Classes:** `.hh-dropdown`
- **States:** `:hover`, `:focus`, `:disabled`. The open menu's rules and tokens are in `hh-dropdown-menu.uss`.
- **Tokens:** `--hh-color-bg`, `--hh-color-focus`, `--hh-color-text`, `--hh-color-violet-light`, `--hh-duration-fast`, `--hh-easing-standard`, `--hh-font-size-md`, `--hh-radius-sm`, `--hh-space-1`, `--hh-space-2`

## HHTextField

A single-line text input with a label. Extends `TextField`.

```xml
<hh:HHTextField label="Name" value="Northwind" />
```

- **Classes:** `.hh-text-field`
- **States:** `:hover`, `:focus` (focus ring), `:disabled`.
- **Tokens:** `--hh-color-bg`, `--hh-color-focus`, `--hh-color-text`, `--hh-color-violet-light`, `--hh-duration-fast`, `--hh-easing-standard`, `--hh-font-size-md`, `--hh-radius-sm`, `--hh-space-1`, `--hh-space-2`

## HHTabs

Tabs whose headers take keyboard focus. Extends `TabView`.

```xml
<hh:HHTabs>
  <ui:Tab label="Audio" />
  <ui:Tab label="Video" />
</hh:HHTabs>
```

- **Classes:** `.hh-tabs`
- **States:** The selected header is `:checked`, with `:hover` and `:focus` on headers.
- **Tokens:** `--hh-color-focus`, `--hh-color-teal`, `--hh-color-text`, `--hh-color-text-dim`, `--hh-font-size-md`, `--hh-radius-sm`, `--hh-space-2`, `--hh-space-4`

## HHScrollView

A scroll view with thin, token-coloured scrollers. Extends `ScrollView`.

```xml
<hh:HHScrollView />
```

- **Classes:** `.hh-scroll-view`
- **States:** No interactive states of its own.
- **Tokens:** `--hh-color-track`, `--hh-color-violet-light`, `--hh-radius-sm`, `--hh-space-2`

## HHListView

A virtualised list with hoverable rows and slim scrollers. Extends `ListView`.

```xml
<hh:HHListView fixed-item-height="32" />
```

- **Classes:** `.hh-list-view`
- **States:** `:hover` on rows; the selected row (Unity's `--selected` item class) gets a violet edge.
- **Tokens:** `--hh-color-text`, `--hh-color-text-dim`, `--hh-color-track`, `--hh-color-violet-ghost-hover`, `--hh-color-violet-light`, `--hh-radius-sm`, `--hh-space-2`, `--hh-space-3`

## HHDialog

A modal over a scrim, with a title, body and action buttons. Call `Open()`, `Close()` and `AddAction(label, onClick, ghost, closes)`; `Closed` fires on close. Extends `VisualElement`.

```xml
<hh:HHDialog title="Quit?" require-choice="true">
  <ui:Label text="Unsaved progress will be lost." />
</hh:HHDialog>
```

- **Classes:** `.hh-dialog`, `.hh-dialog--open`, `.hh-dialog__panel`, `.hh-dialog__title`, `.hh-dialog__body`, `.hh-dialog__actions`
- **Attributes:**
  - `title` (string, default empty): The heading.
  - `require-choice` (bool, default false): When true, Escape, cancel and scrim clicks leave it open; only an action closes it.
- **States:** Open adds `.hh-dialog--open` and focuses the first stop; closed dialogs aren't displayed. While open, Tab and Shift+Tab wrap inside it and focus that leaves comes back; Close returns focus to where it was.
- **Tokens:** `--hh-color-card-edge`, `--hh-color-scrim`, `--hh-color-surface`, `--hh-color-text`, `--hh-font-size-lg`, `--hh-font-size-md`, `--hh-radius-lg`, `--hh-space-2`, `--hh-space-3`, `--hh-space-6`

## HHToast

One notification card. `HHToastStack.Show` creates these; `Dismiss()` removes one and fires `Dismissed`. Extends `VisualElement`.

```xml
<hh:HHToast text="Saved" tone="Success" />
```

- **Classes:** `.hh-toast`, `.hh-toast__text`, `.hh-toast--success`, `.hh-toast--warning`, `.hh-toast--danger`
- **Attributes:**
  - `text` (string, default empty): The message.
  - `tone` (HHToastTone, default Info): Info, Success, Warning or Danger; Info has no modifier class.
- **States:** No interactive states. Tone is a class.
- **Tokens:** `--hh-color-peach`, `--hh-color-pink`, `--hh-color-surface`, `--hh-color-teal`, `--hh-color-text`, `--hh-color-violet-light`, `--hh-font-size-md`, `--hh-radius-md`, `--hh-space-2`, `--hh-space-3`, `--hh-space-4`

## HHToastStack

The corner that holds toasts. `Show(text, tone, seconds)` adds one that dismisses itself after `seconds` (4 by default); 0 or less keeps it until dismissed. Extends `VisualElement`.

```xml
<hh:HHToastStack max-visible="3" />
```

- **Classes:** `.hh-toast-stack`
- **Attributes:**
  - `max-visible` (int, default 4): At least 1; the oldest toasts beyond it are dismissed.
- **States:** No interactive states.
- **Tokens:** `--hh-space-4`

## HHTooltip

A manipulator, not an element: `element.AddManipulator(new HHTooltip("Save"))`. Without text it shows the element's `tooltip`. It shows after a hover or focus delay (500 ms by default), draws below the element, left-aligned with a 4 px gap, in the document just under the panel root so the HH stylesheets reach it, and hides on click, Escape, pointer leave, blur or detach.

- **Classes:** `.hh-tooltip`
- **States:** Shown while hovered or focused; `IsShowing` reports it.
- **Tokens:** `--hh-color-bg`, `--hh-color-text`, `--hh-color-track`, `--hh-font-size-sm`, `--hh-radius-sm`, `--hh-space-1`, `--hh-space-2`
