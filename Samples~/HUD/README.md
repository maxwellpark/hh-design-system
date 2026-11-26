# HUD sample

A game HUD built from the design system: a Player card holding a health bar and lives as a
segmented bar, a bag button with a badge counting new items, and toasts for events.

Put `HudOverlay.cs` on a GameObject with a `UIDocument` whose PanelSettings' Theme Style Sheet is
`hh-theme.tss` (or `hh-theme-high-contrast.tss`), and press Play. The buttons along the bottom
stand in for gameplay, so the sample needs neither input system: Take a hit drains health and
costs a life at zero, Heal restores some, and Pick up adds to the badge until the bag is opened.
