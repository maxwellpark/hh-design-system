# Acceptance: toast-action

Node 22 and Unity 6000.0.85f2's compiler. EditMode tests are written but not run, since batch mode
needs a licence (Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHToast`, `HHToastStack`, the tests and the gallery compile, editor and player, and every existing `Show` call still compiles | the compile gate (Settings, HUD and Pause Menu samples call `Show`) | 16 of 16 | verified |
| A toast has no action until `SetAction`, which adds one ghost button and relabels it on a second call | `ToastTests.AToastHasNoActionUntilOneIsSet` | | written, not run |
| The action runs its callback, then dismisses the toast | `ToastTests.AnActionRunsItsCallbackThenDismisses` | | written, not run |
| `Show` without an action label adds none | `ToastTests.ShowWithoutAnActionLeavesNone` | | written, not run |
| The button's text meets AA on the toast card, at rest and hovered, in both themes | two "Toast action text on a toast card" pairs | pass | verified |
| The reference names `.hh-toast__action` and the new parameters | `check.mjs` | pass | verified |
| The toast lays out as a row with its text wrapping beside the button | `.hh-toast`, `.hh-toast__text` | | unverified |

## Coverage and complexity

`SetAction` and `RunAction` are a few lines each, both tested. The action button is a normal
`HHButton`, so the focus check and its focus rule already cover it. A toast that timed out after
its action was used does nothing, since `Dismiss` checks it still has a parent.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem [editor] (27 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (17 files)
ok   HootonHeroics.DesignSystem [player] (27 files)
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
68 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery and press "Toast with action": "Item sold" with an Undo button at its right.
2. Press Undo: that toast goes and "Sale undone" shows. Leave another for six seconds: it goes alone.

## Unverified

Rendering and the three tests.
