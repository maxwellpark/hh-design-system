# Acceptance: placeholder

Node 22 and Unity 6000.0.85f2's compiler and class reader.

## Claims

| Claim | Evidence | Result | Status |
|---|---|---|---|
| Unity 6000.0 marks a text field's placeholder with `.unity-base-text-field__input--placeholder` | the gate's class reader lists it among the classes `UnityEngine.UIElementsModule.dll` defines, and the class check passes | ok | verified |
| `HHTextField`'s placeholder is drawn in `--hh-color-text-dim` | `.hh-text-field.unity-base-field .unity-base-text-field__input--placeholder`, after the input rule it ties | | unverified in Unity |
| That colour meets AA on the field's `bg` in both themes | "Placeholder text in a text field" | pass | verified |
| The gallery shows a field with a placeholder, and compiles | `Editor/HHGallery.cs` "Callsign"; the compile gate | ok | verified |
| The reference documents it, and its example's `placeholder-text` is a known base attribute | `check.mjs` | pass | verified |

## Coverage and complexity

One USS rule and one pair. The rule ties the input's own colour rule on specificity and wins on
order, if Unity puts the class on the input; if it puts it on the inner text element, the rule
sets that element's colour directly.

## Behaviour transcript

`node Tools~/unity-compile.mjs HEAD` (excerpt):

```
ok   HootonHeroics.DesignSystem.Editor [editor] (2 files)
ok   HootonHeroics.DesignSystem.Tests.Editor [editor] (17 files)
ok   179 .unity-* class uses checked against 319 Unity class names
ok   HEAD compiles: 16 assembly builds with the compiler and generators of Unity 6000.0.85f2
```

`node Tools~/check.mjs` (last line):

```
68 .meta files ok, 32 tokens match HHTheme, 124 contrast pairs checked across 2 theme(s), 18 controls referenced, 20 types in the component reference
```

## Hands-on checklist (Unity 6, two minutes)

1. Open the gallery: Callsign's field reads "Pick a callsign" in dim text. Type: the text is white.

## Unverified

The colour in Unity.
