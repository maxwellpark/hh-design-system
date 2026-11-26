# Acceptance: runtime-tokens

Node 22 and Unity 6000.0.85f2's compiler (the compile gate).

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| `HHTokens` and its tests compile, editor and player | the compile gate | 6 builds | verified |
| Short and full token names name the same custom property | `ShortAndFullTokenNamesMeanTheSameProperty` | compiled, not run | unverified |
| Nothing resolves before styles do, and a null element is safe | `NothingResolvesBeforeStylesDo` | compiled, not run | unverified |
| On a styled element in a panel, `TryGetColor(element, "color-teal", out c)` returns the token's colour, on the document root and on a deep child, and `WhenResolved` runs each time styles resolve | `customStyle` reads in `HHTokens` | needs a live panel | unverified |
| `HHTheme`'s mirror is unchanged | `node Tools~/check.mjs` | 29 tokens match | verified |

## Coverage and complexity

Needs a Unity test run. Each member is one or two lines.

## Behaviour transcript

`node Tools~/check.mjs`:

```
accepted: Button text: text on violet is 4.00:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
accepted: Button text, hovered: text on violet-light is 2.47:1, needs 4.5 (palette from 0.1, waits on the owner's choice in BACKLOG.md)
52 .meta files ok, 29 tokens match HHTheme, 40 contrast pairs checked, 15 controls referenced
```

`node Tools~/unity-compile.mjs HEAD` (last lines):

```
ok   123 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 6 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

## Hands-on checklist (Unity 6, three minutes)

1. On a `UIDocument` with the HH theme, call
   `HHTokens.WhenResolved(root, e => Debug.Log(HHTokens.TryGetColor(e, "color-teal", out var c) + " " + c))`
   in `OnEnable`, and press Play: it logs `True` and teal (0.27, 0.84, 0.77, 1). Repeat with a
   label nested two levels down: if it logs `False`, custom properties don't inherit and the docs
   must say to read from the root.
2. Run the EditMode tests: the two in `TokensTests` pass.

## Unverified

Reading resolved values in a live panel, and the two tests.
