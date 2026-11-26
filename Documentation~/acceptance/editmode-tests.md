# Acceptance: editmode-tests

No Unity editor was available, and Unity's batch mode refuses tests without a licence, so none of
these tests has run. This pack lists what each would prove once it does.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHButton` carries `.hh-button` and starts as not ghost | `ControlTests.ButtonCarriesItsStyleClass` | not run | unverified |
| Setting `Ghost` adds and removes `.hh-button--ghost` without touching `.hh-button` | `GhostTogglesItsClass` | not run | unverified |
| `HHProgressBar` carries `.hh-progress-bar` | `ProgressBarCarriesItsStyleClass` | not run | unverified |
| UXML `value`, `title` and `ghost` reach the controls through `[UxmlElement]` | `UxmlAttributesReachTheControls`, loading `Tests/Editor/Fixtures/controls.uxml` | not run | unverified |
| `HHStyles.Apply` adds each sheet once, however often it's called | `ApplyAddsEachStyleSheetOnce` | not run | unverified |
| The test folder, fixture and asmdef import from a git install | `node Tools~/check.mjs` (every new file has a committed `.meta`) | exit 0 | verified (files) |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `HHButton.Ghost` | set true / set false / read | `GhostTogglesItsClass`, `UxmlAttributesReachTheControls` (not run) |
| `HHStyles.Apply` | first call adds / second call skips / null root | first two by `ApplyAddsEachStyleSheetOnce` (not run); null root: none |

## Coverage and complexity

Coverage needs the Code Coverage package on a Unity test run, so it's not measured. The tests are
under 15 lines each, with no branches.

## Hands-on checklist (Unity 6, five minutes)

1. In a Unity 6 project with the package from this branch, add
   `"testables": ["com.hootonheroics.designsystem"]` to `Packages/manifest.json`.
2. Window > General > Test Runner > EditMode: `HootonHeroics.DesignSystem.Tests.Editor` lists five
   tests. Run All: all pass.

A compile error or any red test counts as broken.

## Unverified

All five tests, until they run in Unity.
