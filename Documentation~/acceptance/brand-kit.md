# Acceptance: brand-kit

The engine-neutral brand kit and the Godot studio splash. Built 26 November 2026 on Linux with Godot 4.7.2 built from source, headless only, so nothing here has been seen on a screen. The owner chose on that date to keep brand assets and per-engine splash code in this package; `hh-tokens.uss` stays the source of the look.

Regenerate: `GODOT=<godot 4.7> bash Tools~/verify.sh` (exit 0 required).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `Brand~/tokens.json` holds every token in `hh-tokens.uss` with the same value, in both directions, and a token in a group nobody mirrors fails | `node Tools~/check-tokens.mjs`; `Tools~/check-tokens.test.mjs` | exit 0; 12 of 12 | verified |
| The Godot constants match `tokens.json`, translucent alpha compared unrounded; a commented-out line, an expression, an extra colour or a differing logo copy fails | same | exit 0; the test's ten failing cases each exit 1 | verified |
| The check fails on drift | `--hh-color-focus` changed by one digit | exit 1, names the token | verified |
| The splash lasts 2 s: fade in 0.35 s, hold 1.3 s, fade out 0.35 s | `test_the_splash_lasts_two_seconds_and_fades_in_holds_and_fades_out` | pass | verified |
| Reduced motion cuts instead of fading | `test_reduced_motion_cuts_instead_of_fading` | pass | verified |
| It emits `finished` once and frees itself | `test_it_finishes_once_after_its_time_and_frees_itself` | pass | verified |
| A 2.2 s first frame neither skips the splash nor its fade (steps capped at 1/30 s) | `test_a_long_first_frame_neither_skips_the_fade_nor_the_splash` | pass | verified |
| Under a Node2D root it covers the viewport, on canvas layer 128 | `test_it_fills_the_screen_under_a_2d_scene_and_draws_above_game_layers` | pass | verified |
| A key, click, touch or pad press skips it; a release, a key repeat, the wheel or mouse motion does not | `test_any_press_skips_but_a_release_repeat_wheel_or_motion_does_not` | pass | verified |
| Landscape: Russell left of the mark with a quarter of its height clear, on screen | `test_landscape_puts_the_host_beside_the_mark_with_clear_space` | pass | verified (positions, not pixels) |
| Portrait 720x1280 and 390x844, square 1000x1000: Russell above the mark, the column on screen | `test_portrait_and_square_screens_stack_the_host_above_the_mark_on_screen` | pass | verified (positions) |
| The mark sits on the brand field with no visible square | `hh-logo.png` is opaque on about `#040308` | | known gap, owner item in `BACKLOG.md` |
| How the splash looks | none: the cloud Godot build has no renderer | | unverified |
| Unity projects see no change: Unity ignores folders ending in `~` | `node Tools~/check.mjs`: 29 `.meta` files, no new imported asset | exit 0 | verified (files) |
| `Brand~/hh-logo.png` is the website's mark | SHA-256 starts `1bafd37de57ff9d0` in both | match | verified |

## Paths

`hh_splash.gd` `alpha_at(t, cut)`:

| Input | Output | Test |
|---|---|---|
| t < 0 or t >= 2.0 | 0 | yes |
| cut, 0 <= t < 2.0 | 1 | yes |
| 0 <= t < 0.35 | t / 0.35 | yes (half way) |
| 0.35 <= t <= 1.65 | 1 | yes |
| 1.65 < t < 2.0 | falling to 0 | yes |

`skips(event)`: key press, key repeat, mouse press, mouse release, wheel, touch press, pad press, mouse motion: all walked.

`layout_for(screen, with_host)`: beside (landscape), stacked (portrait), stacked and shrunk (square), no host: all walked.

## Coverage and complexity

Not measured: Godot has no coverage tool here. Every function in `hh_splash.gd` is called by a test except `_process` (a one-line call to `advance`) and `_input` (calls `skips`, then `finish`). The longest is `_ready` at 40 lines of node set-up with no branches but one; `layout_for` branches three ways.

## Behaviour transcript

```
check-tokens: tokens.json and Godot match hh-tokens.uss
ok   test_splash.gd test_the_splash_lasts_two_seconds_and_fades_in_holds_and_fades_out
ok   test_splash.gd test_reduced_motion_cuts_instead_of_fading
ok   test_splash.gd test_it_finishes_once_after_its_time_and_frees_itself
ok   test_splash.gd test_a_long_first_frame_neither_skips_the_fade_nor_the_splash
ok   test_splash.gd test_any_press_skips_but_a_release_repeat_wheel_or_motion_does_not
ok   test_splash.gd test_it_fills_the_screen_under_a_2d_scene_and_draws_above_game_layers
ok   test_splash.gd test_landscape_puts_the_host_beside_the_mark_with_clear_space
ok   test_splash.gd test_portrait_and_square_screens_stack_the_host_above_the_mark_on_screen
8 tests, 0 failed
```

## Hands-on checklist

1. Copy `Godot~/addons/hh_studio` into a Godot 4.7 project, add `HHSplash.create(null)` to the first scene (a Node2D root is fine) and run it: the dark field, the mark centred, "HOOTON HEROICS" below, about two seconds, then the scene.
2. Press any key during it: it ends at once.
3. Pass a texture as the host: it stands left of the mark in landscape and above it in portrait.
4. Look for a faint square around the mark: expected until the transparent mark lands.

## Unverified

- The splash on screen, in a desktop or web build.
- The Unity splash, which is not built.
- Russell's master as the host: it lives in a private repo and stays out of this public one.
