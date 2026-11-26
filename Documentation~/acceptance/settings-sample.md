# Acceptance: settings-sample

Node 22 and Unity 6000.0.85f2's compiler, which builds each sample as a project's own scripts would,
in the editor and for a player.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The Settings sample compiles against the package, editor and player | the compile gate: `Sample.Settings [editor]` and `[player]` | ok | verified |
| It uses the tabs, toggle, slider, dropdown, text field, dialog and toast stack, and the check fails if it stops | the settings check in `check.mjs`; "the settings sample dropping a control fails" | pass | verified |
| The Package Manager lists it | `package.json` `samples` entry | 2 samples | verified |
| Save shows a toast; Back with unsaved changes opens the dialog, whose Discard restores the saved values | `SettingsMenu.Save`, `Back`, `Revert` | | unverified |

## Coverage and complexity

Sample code, not under test. `OnEnable` builds the screen in three helpers; every method is under 25 lines.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
58 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced
```

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   Sample.Settings [player] (1 files)
ok   123 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 8 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. Import the Settings sample, put `SettingsMenu` on a GameObject with a `UIDocument` using
   `hh-theme.tss`, and press Play: a Settings card with Audio, Video and Player tabs.
2. Turn Music off and press Back: a "Discard changes?" dialog. Discard: Music is back on.
3. Change the name and press Save: a "Settings saved" toast; Back now does nothing.

## Unverified

The screen's behaviour in Play mode.
