# Adding a control

`node Tools~/check.mjs` and `bash Tools~/unity-compile.sh` hold every control to the same rules, and
they are the authority: this page lists what they check today so a new control passes first time.
Run both before you push; a failure names the file and what is missing.

## The control

A control is a `[UxmlElement] public partial class` in `Runtime/HH<Name>.cs`, the file named after
the class (the reference check finds its section by the file name), with a `UssClassName` constant,
by convention `"hh-<name>"`, and one constant per extra class it adds:

```csharp
using UnityEngine.UIElements;

namespace GuideExample
{
    /// UXML: <GuideExample.GuideChip text="New" />
    [UxmlElement]
    public partial class GuideChip : Label
    {
        public const string UssClassName = "guide-chip";
        public const string HotUssClassName = UssClassName + "--hot";

        public GuideChip()
        {
            AddToClassList(UssClassName);
        }

        [UxmlAttribute]
        public bool Hot
        {
            get => ClassListContains(HotUssClassName);
            set => EnableInClassList(HotUssClassName, value);
        }
    }
}
```

(That example lives in this guide only, under its own namespace and with a `guide-` class rather
than `hh-`, so the gate compiles it without it becoming a control.)

## What the checks need

- **Files:** every new file and folder has a committed `.meta` with a unique, 32-character lowercase
  hex GUID. Open the package in Unity to generate them, or copy the form beside it: two lines for a
  file, and for a new folder the eight-line folder form, as in `Editor.meta`, with a new GUID.
- **Rules:** each `*UssClassName` constant has a rule in `Runtime/Styles/hh-components.uss`, using
  only properties Unity 6000.0 knows and only `.unity-*` classes it defines, and every
  `var(--hh-*)` naming a defined token. By convention, not by check, colours, sizes and durations
  come from tokens, so themes and the accessibility modifiers reach them.
- **Test:** an EditMode test in `Tests/Editor/` that names the control, outside `GalleryTests.cs`.
- **Gallery:** `Editor/HHGallery.cs` builds it.
- **Reference:** a `## HH<Name>` section in `Documentation~/components.md` whose Classes,
  Attributes and Tokens lists match the code and the rules exactly, with a UXML example whose
  attributes are its own, its Unity base's or VisualElement's.
- **Focusable controls** (a base in `check.mjs`'s focusable table): a `:focus` rule and a single
  compound `:disabled` rule, both starting with its own class, and a disabled instance in the
  gallery: a uniquely named variable, added to the page, with `SetEnabled(false)`. Add it to
  `GalleryTests.ThePageHoldsEveryFocusableControlDisabled` too. A new Unity base must be added to
  the focusable or not-focusable table first.
- **Contrast:** add a pair to `Tools~/contrast.json` for each colour it puts text, an edge, a ring
  or a fill on. The check runs every pair in every theme, but can't know a pair is missing.
- **New tokens:** a name starting `color-`, `space-`, `radius-`, `font-size-`, `duration-` or
  `easing-`, added to every `hh-tokens*.uss`, to `HHTheme`, and to the mirrors `Tools~/check-tokens.mjs`
  checks, `Brand~/tokens.json` and `Godot~/addons/hh_studio/hh_tokens.gd`. A new `duration-*` or
  `font-size-*` token also goes in the `.hh-reduced-motion` or `.hh-large-text` rule.

## Then

Add a CHANGELOG line, an acceptance pack in `Documentation~/acceptance/`, GOAL.md's counts, and the
control to a sample if it serves one.
