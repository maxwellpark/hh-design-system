# Acceptance: brand-kit

The engine-neutral brand kit and the Godot studio splash. Built 26 November 2026 on Linux with Godot 4.7.2 built from source, headless only, so nothing here has been seen on a screen. The owner chose on that date to keep brand assets and per-engine splash code in this package; `hh-tokens.uss` stays the source of the look.

Regenerate: `GODOT=<godot 4.7> bash Tools~/verify.sh` (exit 0 required).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `Brand~/tokens.json` holds every colour in `hh-tokens.uss` with the same RGBA, and the same space, radius and font-size values | `node Tools~/check-tokens.mjs` | exit 0 | verified |
| The Godot constants match `tokens.json`, including the translucent colours | same | exit 0 | verified |
| The check fails on drift | `--hh-color-focus` changed by one digit | exit 1, names the token | verified |
| The splash lasts 2 s: fade in 0.35 s, hold 1.3 s, fade out 0.35 s | `test_the_splash_lasts_two_seconds_and_fades_in_holds_and_fades_out` | pass | verified |
| Reduced motion cuts instead of fading | `test_reduced_motion_cuts_instead_of_fading` | pass | verified |
| It emits `finished` once and frees itself | `test_it_finishes_once_after_its_time_and_frees_itself` | pass | verified |
| A key, click, touch or pad press skips it; a release, a key repeat or mouse motion does not | `test_any_press_skips_but_a_release_or_key_repeat_does_not` | pass | verified |
| Brand field colour, mark height from its token, Russell left of the mark, wordmark text, no host node without a host | `test_layout_puts_the_host_left_of_the_mark_on_the_brand_field` | pass | verified (positions, not pixels) |
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

`skips(event)`: key press, key repeat, mouse press, mouse release, touch press, pad press, mouse motion: all walked.

## Coverage and complexity

Not measured: Godot has no coverage tool here. Every function in `hh_splash.gd` is called by a test except `_process` (a one-line call to `advance`) and `_input` (calls `skips`, then `finish`). The longest function is `_layout` at 20 lines; nothing branches more than twice.

## Behaviour transcript

```
check-tokens: tokens.json and Godot match hh-tokens.uss
ok   test_splash.gd test_the_splash_lasts_two_seconds_and_fades_in_holds_and_fades_out
ok   test_splash.gd test_reduced_motion_cuts_instead_of_fading
ok   test_splash.gd test_it_finishes_once_after_its_time_and_frees_itself
ok   test_splash.gd test_any_press_skips_but_a_release_or_key_repeat_does_not
ok   test_splash.gd test_layout_puts_the_host_left_of_the_mark_on_the_brand_field
5 tests, 0 failed
```

## Hands-on checklist

1. Copy `Godot~/addons/hh_studio` into a Godot 4.7 project, add `HHSplash.create(null)` to the first scene and run it: the dark field, the mark centred, "HOOTON HEROICS" below, about two seconds, then the scene.
2. Press any key during it: it ends at once.
3. Pass a texture as the host: it stands left of the mark, the same height band.

## Unverified

- The splash on screen, in a desktop or web build.
- The Unity splash, which is not built.
- Russell's master as the host: it lives in a private repo and stays out of this public one.
