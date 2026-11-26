# Goal

Status: draft, not confirmed

## Outcome

A themeable UI Toolkit design system for Unity 6 that a game installs from its git URL and restyles
from one tokens file, with the controls a game's menus, settings screens and HUD need.

## Done when

- [ ] It installs from the git URL into a clean Unity 6 project with no import warnings, and the
  Basic UI sample runs. Check: a clean project in Unity 6, console empty after import.
- [x] 2026-11-26, `Tools~/check.mjs`: every imported asset has a `.meta`, and `HHTheme` matches
  the tokens. Check: `node Tools~/check.mjs` exits 0.
- [x] 2026-11-26, `Runtime/` (17 UXML controls): menus, settings and HUD are covered: button, toggle, slider, dropdown, text field, dialog,
  tabs, tooltip, toast and progress bar each have a file in `Runtime/` and rules in
  `hh-components.uss`. Check: the controls count in Numbers reaches 10.
- [ ] Every text colour meets WCAG AA contrast on the surfaces it sits on. Check:
  `node Tools~/check.mjs`.
- [ ] Switching between dark, light and high-contrast themes at runtime restyles every control.
  Check: a capture of the sample in each theme.
- [ ] EditMode tests pass through unity-packages' `unity-verify`. Check: its exit code (it runs on
  the owner's machine; not available in a cloud session).

## Numbers

| What | Command | Latest |
|---|---|---|
| Controls with a UXML element | `grep -l "\[UxmlElement\]" Runtime/*.cs \| wc -l` | 18 |
| Assembly builds with Unity's compiler (editor and player) | `bash Tools~/unity-compile.sh` | 16 of 16 |
| Tokens | `node Tools~/check.mjs` | 32 |
| Check cases | `node Tools~/check.test.mjs` | 99/99 |
| Contrast pairs below AA | `node Tools~/check.mjs` (accepted lines) | 3 of 122, all owner decisions |
| EditMode tests | unity-packages' `unity-verify` | 72 written, 0 run |

## Out of scope

- uGUI and IMGUI controls.
- Paid Pro components, until the open-core split is decided.
- The planned sibling packages (`com.hootonheroics.ai`, `...procgen`).

## Last checked

2026-11-26, 51a7f47.
