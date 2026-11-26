# Acceptance: contrast-check

Commands run from the package root with Node 22. No Unity editor was available.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every text and focus pair in `Tools~/contrast.json` meets WCAG AA (4.5:1 text, 3:1 focus ring), or is an accepted exception with its reason | `node Tools~/check.mjs` | exit 0, 10 pairs, 2 accepted | verified |
| A pair below its minimum fails the check | `check.test.mjs` "contrast below AA fails" | pass | verified |
| A translucent colour is blended onto what it sits over before measuring, and a pair that doesn't say what that is fails | "translucent background without over fails"; the ghost pairs blend `violet-ghost` over `surface` | pass | verified |
| A pair naming a missing token fails | "pair naming an unknown token fails" | pass | verified |
| Ghost button text stays at AA when hovered (was 4.42:1 at a 28% tint) | `--hh-color-violet-ghost-hover` at 22%: 4.78:1 by the check | pass | verified (ratio); unverified (render) |
| The progress title reads clearly (white on teal fill was 1.63:1) | Title moved above the bar onto the card: text on `surface`, 15.16:1 | pass | verified (ratio); unverified (render) |
| The 0.1 button colours still fail AA and are reported, not hidden | "accepted pair is reported, not failed"; BACKLOG.md Blocked on owner | 4.00:1 and 2.47:1 | verified |
| An accepted pair can't quietly get worse, or stay accepted once it passes | "accepted pair getting worse fails", "accepted pair that now passes fails" | pass | verified |
| The blended ratio and the focus ring's 3:1 minimum are what's measured | "blended ratio is measured", "3:1 minimum applies to the focus ring" | pass | verified |

## Paths

| Code | Path | Evidence |
|---|---|---|
| `contrast` and its loop | pass / fail / accepted / accepted worse / accepted now passing / missing key / unknown token / translucent without `over` / translucent `over` / translucent with `over` | clean run and the contrast cases; translucent `over`: none |
| `min` | default 4.5 / explicit 3 (focus ring) | clean run (both kinds present) |

## Coverage

Not measured: the test runs `check.mjs` in child processes. The paths table stands in.

## Complexity

By reading, the contrast loop is about 30 lines with five branches; under 10 and 60.

## Behaviour transcript

`node Tools~/check.mjs`, exit 0:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
19 .meta files ok, 24 tokens match HHTheme, 10 contrast pairs checked
```

`node Tools~/check.test.mjs`, exit 0:

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
ok   contrast below AA fails
ok   accepted pair is reported, not failed
ok   pair naming an unknown token fails
ok   blended ratio is measured
ok   3:1 minimum applies to the focus ring
ok   accepted pair getting worse fails
ok   accepted pair that now passes fails
ok   pair missing a key fails readably
ok   translucent background without over fails
24/24 passed
```

## Captures

None: no Unity editor here.

## Hands-on checklist (Unity 6, five minutes)

1. Run the Basic UI sample. The progress bar's title "Loading" sits above the bar, left-aligned,
   white on the card, and nothing overlaps the card's subtitle.
2. Hover the Reset (ghost) button: the tint strengthens a little and the text stays easy to read.

Title clipped, overlapping or missing counts as broken.

## Unverified

- How the moved title renders: `top: auto` and `bottom: 100%` on the title container are
  expected to lift it above the bar, left-aligned by `flex-direction: row`, but haven't been seen
  in Unity. If Unity warns on `top: auto`, `top: initial` does the same.
- The 0.1 button contrast, which waits on the owner's palette choice.
