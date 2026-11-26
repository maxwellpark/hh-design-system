# Acceptance: gallery-disabled-test

Unity 6000.0.85f2's compiler. The test is written but not run, since batch mode needs a licence
(Blocked on owner).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| The test compiles with the editor tests | the compile gate, `HootonHeroics.DesignSystem.Tests.Editor` | ok | verified |
| The page `HHGallery.Build` makes holds a disabled instance of each focusable control: button, icon button, hold button, toggle, slider, dropdown, text field, radio group, stepper, tabs, list and scroll view | `GalleryTests.ThePageHoldsEveryFocusableControlDisabled`, finding each with `page.Query<T>().Where(e => !e.enabledSelf)` | | written, not run |
| The list matches the controls `check.mjs`'s disabled check requires | read against its focusable bases | | verified by reading |

## Coverage and complexity

One test with twelve assertions, each named, so a failure says which control is missing.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (18 files)
ok   HEAD compiles: 18 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
70 .meta files ok, 32 tokens match HHTheme, 126 contrast pairs checked across 2 theme(s), 19 controls referenced, 21 types in the component reference
```

## Unverified

The test hasn't run.
