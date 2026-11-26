# Acceptance: gallery

Node 22 and Unity 6000.0.85f2's compiler.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The gallery window and its tests compile in the editor assembly | the compile gate | 6 builds | verified |
| Every control appears in the gallery | the reference check, which now fails on a control the gallery doesn't name; "a control never in the gallery fails" | 15 controls; 59/59 | verified |
| `Build` puts every control on a page and the dialog starts closed | `TheGalleryShowsEveryControl`, `TheDialogStartsClosed` | compiled, not run | unverified |
| The window opens from the menu, lists `hh-tokens` and `hh-tokens-high-contrast`, and restyles on a pick | `HHGallery.CreateGUI`, `TokenFiles`, `Style` | | unverified |

## Coverage and complexity

Needs a Unity test run. `Build` is about 75 lines with no branches; it stays one function as a
straight list of sections.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on button is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on button-hover is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
58 .meta files ok, 32 tokens match HHTheme, 82 contrast pairs checked across 2 theme(s), 15 controls referenced
```

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   123 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. Hooton Heroics > Design System > Gallery: a window with a Theme dropdown and cards for buttons,
   badges, progress, form controls, tabs, list and overlays.
2. Pick `hh-tokens-high-contrast`: the cards turn black with light edges.
3. Press Open dialog: the dialog covers the page; Escape closes it. Press Show toast: a toast
   appears bottom-right and goes after four seconds.
4. Run the EditMode tests: the two in `GalleryTests` pass.

## Unverified

The window itself, theme switching in it, and the two tests.
