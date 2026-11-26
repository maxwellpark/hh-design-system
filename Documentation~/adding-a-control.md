# Adding a control

`node Tools~/check.mjs` and `bash Tools~/unity-compile.sh` hold every control to the same rules, and
they are the authority: this page lists what they check today so a new control passes first time.
Run both before you push; a failure names the file and what is missing.

## The control

A control is a `[UxmlElement] public partial class` in `Runtime/`, named `HH<Name>`, with a
`UssClassName` constant `"hh-<name>"` and one constant per extra class it adds:

```csharp
using UnityEngine.UIElements;

namespace GuideExample
{
    /// UXML: <hh:GuideChip text="New" />
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

(That example lives in this guide only, under its own namespace, so the gate compiles it without
it becoming a control.)

## What the checks need

- **Files:** every new file has a committed `.meta` with a unique GUID. Open the package in Unity
  to generate them, or write the two-line form the other metas use.
- **Rules:** each `*UssClassName` constant has a rule in `Runtime/Styles/hh-components.uss`, using
  only properties Unity 6000.0 knows and only `.unity-*` classes it defines. Colours, sizes and
  durations come from `var(--hh-*)` tokens, so themes and the accessibility modifiers reach them.
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
- **New tokens:** add them to every `hh-tokens*.uss`, to `HHTheme`, and to the brand mirror that
  `Tools~/check-tokens.mjs` checks.

## Then

Add a CHANGELOG line, an acceptance pack in `Documentation~/acceptance/`, and the control to a
sample if it serves one.
