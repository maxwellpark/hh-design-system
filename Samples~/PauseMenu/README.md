# Pause menu sample

A pause menu built from the design system: a card of Resume, Settings and a Hold to quit button,
which quits only when held for a second and a half.

Put `PauseMenu.cs` on a GameObject with a `UIDocument` whose PanelSettings' Theme Style Sheet is
`hh-theme.tss` (or `hh-theme-high-contrast.tss`), and press Play. Press Pause, top left, to open
the menu; a game would bind this to Escape or Start. Resume closes it. Settings points to the
Settings sample, and Hold to quit, held with the pointer, Space or Enter, shows where a game would
leave. A pad's submit has no release, so a game binds its own hold to the button's `Begin` and
`Release`, or adds a dialog.
