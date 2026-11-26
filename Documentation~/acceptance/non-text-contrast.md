# Acceptance: non-text-contrast

Node 22. A palette audit: no Unity code changed.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every part the audit found that shows a control's state, extent or identity has a 3:1 pair: edges, focus rings, ticks, handles, fills, tracks, underlines, toast edges, icons, the dropdown arrow and checkmark, the badge dot | a read of `hh-components.uss` and `hh-dropdown-menu.uss`; the pairs below | 25 pairs | verified by reading |
| The audit added three: the slider's focus ring, the dropdown arrow and the empty track | `contrast.json` | 3 | verified |
| Each pair is checked in every theme, and only the default theme may hold an accepted one | `check.mjs` contrast check, exit 0 | 88 pairs, 2 themes | verified |
| The one new failure, the empty track on a card, is recorded with its ratio and options, not changed | `contrast.json` accepted entry (1.39), BACKLOG Blocked on owner | | verified |

## Pairs at 3:1 after the audit

| Part | Pair | Result |
|---|---|---|
| Focus ring against a card | focus on surface | passes in both themes |
| Checked toggle tick on its fill | text on violet | passes in both themes |
| Dropdown list checkmark | teal on surface | passes in both themes |
| Selected list row edge against a card | violet-light on surface | passes in both themes |
| Scroller handle against its track | violet-light on track | passes in both themes |
| Selected tab underline against a card | teal on surface | passes in both themes |
| Info toast edge against its card | violet-light on surface | passes in both themes |
| Success toast edge against its card | teal on surface | passes in both themes |
| Warning toast edge against its card | peach on surface | passes in both themes |
| Danger toast edge against its card | pink on surface | passes in both themes |
| Icon button icon | violet-light on violet-ghost over surface | passes in both themes |
| Icon button icon, hovered | violet-light on violet-ghost-hover over surface | passes in both themes |
| Badge dot against a card | violet-light on surface | passes in both themes |
| Focus ring around a text field or dropdown | focus on bg | passes in both themes |
| Toggle box border against a card | violet-light on surface | passes in both themes |
| Text field and dropdown border against a card | violet-light on surface | passes in both themes |
| Checked toggle against a card | violet on surface | passes in both themes |
| Slider handle against its track | teal on track | passes in both themes |
| Slider handle focus ring against its track | text on track | passes in both themes |
| Dropdown arrow in its field | violet-light on bg | passes in both themes |
| Progress fill or segment against its track | teal on track | passes in both themes |
| Violet progress fill or segment against the track | violet-light on track | passes in both themes |
| Pink progress fill or segment against the track | pink on track | passes in both themes |
| Peach progress fill or segment against the track | peach on track | passes in both themes |
| Empty track of a slider, progress bar, segmented bar or scroller against a card | track on surface | accepted at 1.39 in default; passes in high contrast |

Not paired, by design:
- Disabled controls, which WCAG exempts.
- Containers whose text identifies them: the default theme's card edge (`decisions.md`), the dialog
  card against the scrim (1.13:1), the tooltip border (1.58:1) and the dropdown list border (1.39:1).
- Hover borders in `--hh-color-text`, which pass as body text on a card.

Waiting on the track choice: the hovered list row and the hovered dropdown item change only their
background to `--hh-color-track`, 1.39:1 on a card. Whether 1.4.11 reaches hover is arguable;
the owner's track decision in BACKLOG.md settles these too.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Empty track of a slider, progress bar, segmented bar or scroller against a card: track on surface is 1.39:1, needs 3 (palette from 0.1; lightening the track drops the fills under 3:1, so it waits on the owner's choice in BACKLOG.md)
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
58 .meta files ok, 32 tokens match HHTheme, 88 contrast pairs checked across 2 theme(s), 15 controls referenced, 16 types in the component reference
```

## Unverified

That the audit found every state-bearing part: it's a read of the stylesheets, not a machine check.
