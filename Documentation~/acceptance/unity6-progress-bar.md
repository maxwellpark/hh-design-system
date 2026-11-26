# Acceptance: unity6-progress-bar

Version 0.2.0. No Unity editor was available, so nothing here has been compiled or run in Unity;
every claim about Unity behaviour is unverified, and the hands-on checklist is the real test.
Commands run from the package root with Node 22.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Installed from git, Unity imports every asset (before, none had a `.meta`, so Unity skipped them all) | `node Tools~/check.mjs` | 19 `.meta` files, all committed, unique GUIDs, exit 0 | verified (files); unverified (import) |
| A new file without a committed `.meta` fails the check | `node Tools~/check.test.mjs`, cases "asset without a .meta", "new top-level folder", ".meta on disk but not committed" | pass | verified |
| `HHTheme`'s defaults match every token, and drift fails the check | `node Tools~/check.test.mjs`, the token cases | 15/15 cases pass | verified |
| Hovering a ghost button keeps its text readable (before, background and text were both violet-light) | `.hh-button.hh-button--ghost` (two classes) comes after `.hh-button:hover`, and its own `:hover` outranks both | by CSS specificity | unverified |
| A focused button shows a teal ring without shifting layout (also after a pointer click: UI Toolkit has no `:focus-visible`); a pressed one keeps its own colour rather than Unity's grey; a disabled one dims | `.hh-button:focus`, `.hh-button:hover:active`, `.hh-button:disabled`, 2px border always present | | unverified |
| `HHButton` registers with Unity 6's `[UxmlElement]` and `ghost="true"` works in UXML | `Runtime/HHButton.cs` | | unverified |
| `HHProgressBar` renders with a teal fill, rounded track and the title on top, and keeps `value` and `title` from UXML | `Runtime/HHProgressBar.cs`, `.hh-progress-bar` rules | | unverified |
| Choosing `hh-theme.tss` as a panel's theme styles every document on it | `Runtime/Styles/hh-theme.tss` imports Unity's default theme, then the tokens and components by relative path | | unverified |
| Theme assets saved under 0.1 keep their background colour | `HHTheme.background` keeps its field name, and fields are only added | | unverified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `check.mjs` walk | asset with `.meta` / without / not committed / folder recursed / hidden or `~` skipped | cases 1 to 3 and 15; skip: none |
| `check.mjs` GUIDs | unique / duplicate / missing guid / missing `.meta` file / CRLF line endings | cases 1, 4, 5, 3, 14 |
| `check.mjs` tokens | match / colour / alpha / px drift / token without field / field without token / unparsable colour / non-px size / commented-out token | cases 1, 6 to 13 |
| `HHButton.Ghost` | set true / set false / read | none (no Unity) |
| `HHButton(Action)` | with a click handler / null from the parameterless constructor | none (no Unity) |

## Coverage

Node's coverage needs `--experimental-test-coverage` under `node --test`; the check test runs
`check.mjs` in child processes, so it isn't measured. The paths table above stands in. No C# runs
here, so the Runtime assembly has no coverage.

## Complexity

`lizard` isn't installed here (installing it waits on the owner). By reading, the largest function
is the token loop in `check.mjs` at about 20 lines with four branches; the C# is all under ten
lines per member.

## Behaviour transcript

`node Tools~/check.test.mjs`, exit 0. Each case breaks a copy of the package one way and asserts the
exit code and the message:

```
ok   clean package passes
ok   asset without a .meta
ok   new top-level folder without a .meta
ok   two .meta files share a guid
ok   .meta without a guid
ok   colour drifts
ok   alpha drifts
ok   size drifts
ok   token with no field
ok   field with no token
ok   unparsable colour
ok   size not in px
ok   commented-out token is ignored
ok   .meta with Windows line endings
ok   .meta on disk but not committed
15/15 passed
```

## Captures

None: no Unity editor here.

## Hands-on checklist (Unity 6, about ten minutes)

1. In a clean Unity 6 project, add the package by git URL with `#max/unity6-progress-bar`, or
   `main` once merged. The console stays free of "has no meta file" warnings and compile errors.
2. Import the Basic UI sample. Put `DemoPanel` on a GameObject with a `UIDocument`, assign the two
   USS files, press Play: a card with a title, a progress bar at 40, Play and Reset buttons.
3. Click Play: the bar fills by 10 each time, up to 100. Click Reset: it empties.
4. Hover Reset (the ghost button): its text stays readable on a slightly stronger tint.
5. Press Tab: the focused button gets a teal ring and nothing moves.
6. Clear the two USS fields, set the PanelSettings' Theme Style Sheet to `hh-theme.tss`, press
   Play: the panel looks the same.
7. Open `demo.uxml` in the UI Builder: `HHButton` and `HHProgressBar` show in the Library, and
   the Reset button's `ghost` attribute is ticked.

Anything missing, unstyled, mis-coloured or throwing counts as broken.

## Unverified

Everything that needs Unity: compile, import, rendering, UXML attributes, the theme sheet's
relative imports. The checklist proves them; an EditMode suite (Ready item 2) would keep them proved.
