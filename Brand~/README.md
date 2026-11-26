# Hooton Heroics brand kit

Engine-neutral source for every HH game, site and store page. How the studio uses it, with the lore and jam process, is in the studio kit guide (`Docs/STUDIO-KIT.md` in the website repo). `tokens.json` holds the values; each engine's copy is checked against it by `Tools~/check-tokens.mjs`.

## Logo

`hh-logo.png`, 1024 x 1024, the studio mark. It is byte-identical to the website's `public/hh-logo.png` (SHA-256 starting `1bafd37de57ff9d0`); change it there and here together. Keep it on `color.bg` or a darker field, at least 64 px tall, with clear space of a quarter of its height on every side. Never recolour, stretch or put it on a busy image.

## Studio splash

Every HH game opens with the same splash before its own title.

- Field: `color.bg`, full screen. No other colour behind the mark.
- Mark: `hh-logo.png` centred, `splash.logo-height-fraction` of the screen's shorter side.
- Wordmark: `splash.wordmark` below the mark in `color.text`, letter-spaced, the game's own UI font is fine.
- Host: when the game supplies Russell's studio-host master, he stands left of the mark at `splash.host-height-fraction` of the shorter side, facing it. The master stays in the private repo that owns it; the game copies a sized derivative in, and Russell's character bible governs its use.
- Timing: fade in `fade-in-seconds`, hold `hold-seconds`, fade out `fade-out-seconds`, 2 seconds in all. Any key, click, touch or pad button skips it. With reduced motion on, it cuts instead of fading.
- Sound: none yet. A sound logo, when one exists, plays once at the start of the hold and is skipped with the splash.
- Then the game's own title. The splash never shows a loading bar; load behind it or after it.

Engine implementations: Godot in `Godot~/addons/hh_studio`. Unity is planned in `Runtime/` (it needs a compile and play check in Unity before it ships).

## Credits line

In-game credits and store pages close with: "A Hooton Heroics game." and the mark at its minimum size.

## Disclosure

The splash mark is not generated art. Russell's masters are AI-generated (see his provenance record), so a game that shows him on its splash answers yes to in-game AI art on Steam's disclosure and tags itch.io accordingly.
