# Pause menu sample

A pause menu built from the design system: a card of Resume, Settings and Quit, with Quit asking
through a dialog that only its buttons can close.

Put `PauseMenu.cs` on a GameObject with a `UIDocument` whose PanelSettings' Theme Style Sheet is
`hh-theme.tss` (or `hh-theme-high-contrast.tss`), and press Play. Press Pause, top left, to open
the menu; a game would bind this to Escape or Start. Resume closes it. Settings points to the
Settings sample, and Quit asks before it would leave.
