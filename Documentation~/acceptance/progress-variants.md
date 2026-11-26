# Acceptance: progress-variants

Node 22 and Unity 6000.0.85f2's compiler (`Tools~/unity-compile.sh`). Stacked on the icon button
and badge PR, which adds `HHTone`.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Progress tones, `HHSegmentedBar` and their tests compile with Unity's compiler and generator | the compile gate | 4 of 4 assemblies | verified |
| Every tone's fill reads against the track at 3:1 or more | `node Tools~/check.mjs`, three new pairs | 27 pairs pass | verified |
| A progress bar is teal until given another tone, and returns to no tone class for teal | `ProgressBarIsTealUntilGivenAnotherTone` | compiled, not run | unverified |
| A segmented bar keeps one child per segment, clamped to 1 to 50, and fills from the start, clamping `filled` when segments shrink | `SegmentedBarBuildsOneChildPerSegment`, `SegmentedBarFillsFromTheStartAndClamps` | compiled, not run | unverified |
| `tone`, `segments` and `filled` work from UXML | `UxmlAttributesReachTheVariants` with the fixture | compiled, not run | unverified |
| Both have rules and tests | the reference check | 9 controls | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `HHToneClasses.Apply` | each tone / the default tone | the tone tests (not run) |
| `HHSegmentedBar.Segments` | grow / shrink / below 1 / above 50 | the segment test (not run) |
| `HHSegmentedBar.Filled` | normal / above segments / negative / set before segments | the fill tests (not run) |

## Coverage and complexity

Needs a Unity test run; not measured. The longest member, `Segments`, is 16 lines with two loops.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
36 .meta files ok, 28 tokens match HHTheme, 27 contrast pairs checked, 9 controls referenced
```

`bash Tools~/unity-compile.sh HEAD`:

```
ok   HootonHeroics.DesignSystem (13 files)
ok   HootonHeroics.DesignSystem.Editor (1 files)
ok   HootonHeroics.DesignSystem.Tests.Editor (3 files)
ok   BasicUISample (1 files)
ok   HEAD compiles with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, five minutes)

1. Add `<hh:HHProgressBar value="60" tone="Pink" />`: a pink fill on the dark track.
2. Add `<hh:HHSegmentedBar segments="5" filled="3" tone="Peach" />`: five rounded segments with
   small gaps, the first three peach.
3. Run the EditMode tests: the four in `ProgressVariantTests` pass.

## Unverified

Rendering, and the four tests, which need a licensed editor.
