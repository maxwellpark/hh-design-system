# Settings sample

A settings screen built from the design system: tabs of toggles, a slider, a dropdown and a text
field, Save showing a toast, and Back asking before it throws changes away.

Put `SettingsMenu.cs` on a GameObject with a `UIDocument` whose PanelSettings' Theme Style Sheet is
`hh-theme.tss` (or `hh-theme-high-contrast.tss`), and press Play. Change something and press Back:
a dialog asks whether to discard it. Press Save: a toast confirms.
