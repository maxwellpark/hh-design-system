# Acceptance: dialog

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHDialog`, its tests and the internals they reach compile, editor and player | the compile gate | 6 builds | verified |
| The scrim token is mirrored in `HHTheme`, `Brand~/tokens.json` and Godot | `check.mjs`, `check-tokens.mjs` | 29 tokens match | verified |
| Every dialog class has a rule, and the dialog has tests | the reference check | 10 controls | verified |
| Opening adds the open class; closing twice raises `Closed` once | `OpensAndClosesOnceRaisingClosed` | compiled, not run | unverified |
| Cancel and a scrim click close it, unless a choice is required; a pointer on the card doesn't | `CancelAndScrimCloseUnlessAChoiceIsRequired`, `APointerOnTheCardDoesNotClose` | compiled, not run | unverified |
| Actions run their handler, then close unless told not to | `ActionsRunThenCloseUnlessToldNot` | compiled, not run | unverified |
| UXML children land in the body; `title` and `require-choice` apply | `UxmlChildrenGoInTheBodyAndAttributesApply` | compiled, not run | unverified |
| Escape and gamepad cancel reach the dialog from a focused action; opening focuses the first action and closing restores focus | `KeyDownEvent`, `NavigationCancelEvent` and focus calls in `HHDialog` | needs a panel | unverified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `Open` | closed / already open | the open test (not run) |
| `Close` | open / already closed / previous focus gone | the open test; focus: none |
| `HandleCancel` | closed / require choice / closes | the cancel test (not run) |
| `HandleScrimPointer` | the scrim / the card | the two pointer tests (not run) |
| `RunAction` | closes / stays | the action test (not run) |

## Coverage and complexity

Needs a Unity test run; not measured. The constructor is the longest member at 30 lines with no
branches; the rest are under 15.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
39 .meta files ok, 29 tokens match HHTheme, 27 contrast pairs checked, 10 controls referenced
```

`node Tools~/check-tokens.mjs`:

```
check-tokens: tokens.json and Godot match hh-tokens.uss
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem [editor] (15 files)
ok   HootonHeroics.DesignSystem.Editor [editor] (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (4 files)
ok   Sample.BasicUI [editor] (1 files)
ok   HootonHeroics.DesignSystem [player] (15 files)
ok   Sample.BasicUI [player] (1 files)
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. In a panel, add an `HHDialog` with a title and a label, call `AddAction("Keep", null, ghost: true)`
   and `AddAction("Quit")`, then `Open()`. The page dims behind a centred card; "Keep" has focus.
2. Press Escape: it closes and focus returns to whatever had it. Open it again and click the dim
   area: it closes. Click inside the card: it stays.
3. Set `RequireChoice = true`: Escape and outside clicks do nothing; "Quit" closes it.
4. Run the EditMode tests: the five in `DialogTests` pass.

## Unverified

- Rendering, the five tests, and focus and key routing in a live panel.
- Tab can move focus out of the card (a recorded decision).
