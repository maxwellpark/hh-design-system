# Acceptance: sample-readmes

Node 22. No Unity code changed.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Every sample `package.json` lists has a `README.md` naming every `*.cs` in its folder | the sample check in `check.mjs`, exit 0, over Basic UI, Settings, HUD and Pause Menu | pass | verified |
| The check fails on a listed sample without a README, and on a script its README doesn't name | two planted cases | 2 of 2 | verified |

## Coverage and complexity

About 15 lines in `check.mjs`. It reads only the sample's top folder, which is where every sample
keeps its scripts, and matches the file name anywhere in the README.

## Behaviour transcript

`node Tools~/check.mjs` (last line), `node Tools~/check.test.mjs` (sample cases and total):

```
66 .meta files ok, 32 tokens match HHTheme, 112 contrast pairs checked across 2 theme(s), 17 controls referenced, 19 types in the component reference
ok   the settings sample dropping a control fails
ok   the HUD sample dropping a control fails
ok   the pause menu sample dropping a control fails
ok   a listed sample that's been deleted fails
ok   a listed sample without a README fails
ok   a sample script its README doesn't name fails
96/96 passed
```

## Unverified

Nothing: this is a Node check.
