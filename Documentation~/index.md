# Hooton Heroics Design System

See the root README for install + usage. Tokens live in `Runtime/Styles/hh-tokens.uss`;
components in `hh-components.uss`; `hh-theme.tss` bundles both with Unity's default theme. Everything is driven by `var(--hh-*)` so theming is a
one-file change. `HHTheme` mirrors the palette for C#/non-UI-Toolkit use.

[components.md](components.md) is the reference for each control: its UXML, classes, attributes,
states and tokens.

[getting-started.md](getting-started.md) builds a small themed screen step by step.

[theming.md](theming.md) makes a theme of your own and lets players switch to it.
