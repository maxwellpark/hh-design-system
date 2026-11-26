# Hooton Heroics brand kit

Engine-neutral brand kit for every HH game, site and store page. `Runtime/Styles/hh-tokens.uss` is the source of the look; `tokens.json` mirrors it for other engines and adds the splash values. How the studio uses it, with the lore and jam process, is in the studio kit guide (`Docs/STUDIO-KIT.md` in the website repo). `Tools~/check-tokens.mjs` checks the mirror and each engine's copy.

## Logo

`hh-logo.png`, 1024 x 1024, the studio mark. It is byte-identical to the website's `public/hh-logo.png` (SHA-256 starting `1bafd37de57ff9d0`); change it there and here together. Keep it on `color.bg` or a darker field, at least 64 px tall, with clear space of a quarter of its height on every side. Never recolour, stretch or put it on a busy image.

## Studio splash

Every HH game opens with the same splash before its own title.

- Field: `color.bg`, full screen. No other colour behind the mark. The current `hh-logo.png` is opaque with a near-black ground (about `#040308`), a shade darker than `color.bg`, so a faint square shows around it until a transparent mark replaces it (an owner item in `BACKLOG.md`).
- Mark: `hh-logo.png` centred, `splash.logo-height-fraction` of the screen's shorter side, with a quarter of its height clear on every side.
- Wordmark: `splash.wordmark` below the mark in `color.text`, letter-spaced, the game's own UI font is fine.
- Host: when the game supplies Russell's studio-host master, he stands left of the mark at `splash.host-height-fraction` of the shorter side, facing it, with a quarter of the mark's height between them. On screens too narrow for that (portrait, square) he stands above the mark, shrunk if needed to keep the column on screen. The master stays in the private repo that owns it; the game copies a sized derivative in, and Russell's character bible governs its use.
- Timing: fade in `fade-in-seconds`, hold `hold-seconds`, fade out `fade-out-seconds`, 2 seconds in all, in real time, with a single hitch over a quarter of a second counted as one frame so a slow first frame cannot skip it. Any key, click, touch or pad button skips it; the mouse wheel does not. With reduced motion on, it cuts instead of fading.
- Layer: it draws on its own canvas layer above the game's, so it works under any root node.
- Sound: none yet. A sound logo, when one exists, plays once at the start of the hold and is skipped with the splash.
- Then the game's own title. The splash never shows a loading bar; load behind it or after it.

Engine implementations: Godot in `Godot~/addons/hh_studio`. Unity is planned in `Runtime/` (it needs a compile and play check in Unity before it ships).

## Credits line

In-game credits and store pages close with: "A Hooton Heroics game." and the mark at its minimum size.

## Disclosure

The splash mark is not generated art. Russell's masters are AI-generated (see his provenance record), so a game that shows him on its splash answers yes to in-game AI art on Steam's disclosure and tags itch.io accordingly.
