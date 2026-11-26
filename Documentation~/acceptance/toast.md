# Acceptance: toast

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHToast`, `HHToastStack` and their tests compile, editor and player | the compile gate | 6 builds | verified |
| Every tone's edge reads against the card at 3:1 or more | four new contrast pairs | 31 pairs pass | verified |
| Both have rules and tests | the reference check | 11 controls | verified |
| `Show` adds a toast with its text and exactly its tone's class | `ShowAddsAToastWithItsTextAndTone` | compiled, not run | unverified |
| The newest goes last, and past the cap the oldest is dismissed with `Dismissed` raised | `NewestGoesLastAndTheOldestIsDismissedPastTheCap` | compiled, not run | unverified |
| Dismissing twice raises `Dismissed` once; lowering the cap trims, never below 1; other children in the stack neither count nor get removed | `DismissIsSafeToCallTwice`, `LoweringTheCapTrimsAndItNeverDropsBelowOne`, `OtherChildrenDontCountOrBreakTheCap` | compiled, not run | unverified |
| `max-visible`, `text` and `tone` work from UXML | `UxmlAttributesReachTheStackAndToast` | compiled, not run | unverified |
| A toast dismisses itself after its seconds | `schedule.Execute(...).StartingIn` in `Show` | needs a live panel | unverified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `Show` | timed / untimed (0) / past the cap | the stack tests (not run); timed: none |
| `Dismiss` | in a stack / already gone | `DismissIsSafeToCallTwice` (not run) |
| `MaxVisible` | raise / lower / below 1 | the cap test (not run) |

## Coverage and complexity

Needs a Unity test run; not measured. Every member is under 15 lines with at most two branches.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
39 .meta files ok, 28 tokens match HHTheme, 31 contrast pairs checked, 11 controls referenced
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

1. Add an `HHToastStack` to a full-screen panel and call `Show("Saved", HHToastTone.Success)` four
   times quickly, then once with `HHToastTone.Danger`: cards stack in the bottom-right, newest at the
   bottom, with teal and pink edges; the oldest goes when a fifth arrives.
2. Wait four seconds: each card goes on its own. Clicks beside the cards reach the game.
3. Run the EditMode tests: the six in `ToastTests` pass.

## Unverified

Rendering, the timed dismissal, and the six tests, which need a licensed editor.
