# Basic UI sample

Two ways to try it. Either set the PanelSettings' Theme Style Sheet to `hh-theme.tss`, or give
the document `hh-tokens.uss` and `hh-components.uss` as below (all in the package's
`Runtime/Styles/`).

**Code (fastest):** add `DemoPanel.cs` to a GameObject that has a `UIDocument`, assign
`hh-tokens.uss` and `hh-components.uss` to the two fields (or leave them empty with the theme),
press Play. Play fills the progress bar; Reset, a ghost button, empties it.

**Themes:** fill `DemoPanel`'s `themes` with `hh-theme.tss` and `hh-theme-high-contrast.tss` and a
Theme button appears that cycles them.

**UXML:** open `demo.uxml` in the UI Builder, or set it as a `UIDocument`'s source asset,
and add the two USS files to the document's stylesheet list.
