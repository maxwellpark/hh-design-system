# Acceptance: theme-switching

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHThemes`, its tests and the sample's Theme button compile, editor and player | the compile gate | 6 builds | verified |
| `Apply` sets the theme and ignores nulls | `ApplySetsTheThemeAndIgnoresNulls` | compiled, not run | unverified |
| `Next` cycles, wraps, starts at the first for an unlisted theme, skips null entries, and returns null for no usable themes | `NextCyclesAndStartsAtTheFirstForAnUnlistedTheme` | compiled, not run | unverified |
| The sample copies its PanelSettings before building, so cycling in Play mode leaves the asset alone | `DemoPanel.OnEnable` | | unverified |
| Pressing Theme in Play mode restyles the whole panel | `panelSettings.themeStyleSheet` | | unverified |

## Coverage and complexity

Needs a Unity test run. `Next` is 25 lines with four branches.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
56 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced
```

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   123 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, three minutes)

1. In the Basic UI sample, set the PanelSettings' theme to `hh-theme.tss`, set `DemoPanel.themes` to
   `hh-theme.tss` and `hh-theme-high-contrast.tss`, and press Play: a Theme button shows under Reset.
   (The two USS fields are ignored while themes are set.)
2. Press it: the card turns black-edged high contrast; press again: back to the usual look.
3. Stop Play: the PanelSettings asset still names the theme it had before.
4. Run the EditMode tests: the two in `ThemesTests` pass.

## Unverified

The live switch, the asset staying untouched, and the two tests.
