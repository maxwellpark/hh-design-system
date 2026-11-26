# Acceptance: getting-started

Node 22 and Unity 6000.0.85f2's compiler, through the doc snippet gate.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Both C# blocks in the guide compile against the package, editor and player, each as its own assembly | the compile gate: `Docs.getting_started.1` and `.2`, `[editor]` and `[player]` | ok | verified |
| Its UXML uses only `hh:` elements and attributes the component reference has | `check.mjs` guide UXML check, exit 0 | pass | verified |
| The steps produce the screen described: a dark card titled Northwind, a Music toggle and a violet Play button that logs to the Console, then three buttons that switch theme and accessibility | | | unverified |
| The menu paths (Panel Settings asset, Package Manager) match Unity 6000.0 | | | unverified |

## Coverage and complexity

Two C# blocks and one UXML block, all held by machine; the prose and menu paths aren't.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   Docs.getting_started.1 [editor] (1 files)
ok   Docs.getting_started.2 [editor] (1 files)
ok   Docs.getting_started.1 [player] (1 files)
ok   Docs.getting_started.2 [player] (1 files)
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
65 .meta files ok, 32 tokens match HHTheme, 102 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
```

## Hands-on checklist (Unity 6, ten minutes)

1. In a new Unity 6 project, follow the guide from step 1 to step 4 and press Play: the card,
   toggle and button show as described, and Play logs "Playing with music".
2. Untick Music and press Play: it logs "Playing in silence".
3. Add `MenuOptions` as step 5 says and press Play: three ghost buttons under Play. High contrast
   restyles the card; Large text grows it; Reduce motion stops the hover fades.

## Unverified

Following the guide in Unity, end to end.
