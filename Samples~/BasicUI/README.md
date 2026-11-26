# Basic UI sample

First give the panel the styles, either way:

- **Theme (simplest):** set your PanelSettings' **Theme Style Sheet** to `hh-theme.tss`
  (in the package's `Runtime/Styles/`).
- **Per document:** assign `hh-tokens.uss` and `hh-components.uss` to `DemoPanel`'s two fields,
  or add them to the document's stylesheet list.

Then:

**Code (fastest):** add `DemoPanel.cs` to a GameObject that has a `UIDocument` and press Play.
Play fills the progress bar, Reset (a ghost button) empties it.

**UXML:** open `demo.uxml` in the UI Builder, or set it as a `UIDocument`'s source asset.
