using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

// Prints every unity-* value a *ClassName field gets in Unity assemblies' static constructors, folding ldstr, ldsfld and String.Concat.
static class UnityClasses
{
    static void Main(string[] args)
    {
        var known = new Dictionary<string, string>();
        var names = new SortedSet<string>(StringComparer.Ordinal);
        var cctors = args
            .SelectMany(path => ModuleDefinition.ReadModule(path).Types.SelectMany(t => new[] { t }.Concat(t.NestedTypes)))
            .Select(t => t.Methods.FirstOrDefault(m => m.Name == ".cctor"))
            .Where(m => m?.HasBody == true)
            .ToList();
        // Fields can be built from other types' fields, so passes repeat until nothing new resolves
        for (var pass = 0; pass < 5; pass++)
        {
            var before = known.Count(kv => kv.Value != null);
            foreach (var cctor in cctors)
            {
                var stack = new Stack<string>();
                foreach (var ins in cctor.Body.Instructions)
                {
                    if (ins.OpCode == OpCodes.Ldstr)
                    {
                        stack.Push((string)ins.Operand);
                    }
                    else if (ins.OpCode == OpCodes.Ldsfld)
                    {
                        var field = (FieldReference)ins.Operand;
                        stack.Push(known.TryGetValue(field.DeclaringType.FullName + "." + field.Name, out var value) ? value : null);
                    }
                    else if (ins.OpCode == OpCodes.Call && ((MethodReference)ins.Operand).Name == "Concat")
                    {
                        var parameters = ((MethodReference)ins.Operand).Parameters;
                        var parts = new List<string>();
                        for (var i = 0; i < parameters.Count && stack.Count > 0; i++)
                        {
                            parts.Insert(0, stack.Pop());
                        }
                        var foldable = parameters.Count >= 2 && parameters.All(p => p.ParameterType.FullName == "System.String") && parts.Count == parameters.Count;
                        stack.Push(!foldable || parts.Any(p => p == null) ? null : string.Concat(parts));
                    }
                    else if (ins.OpCode == OpCodes.Stsfld)
                    {
                        var field = (FieldReference)ins.Operand;
                        var value = stack.Count > 0 ? stack.Pop() : null;
                        var key = field.DeclaringType.FullName + "." + field.Name;
                        if (value != null || !known.ContainsKey(key))
                        {
                            known[key] = value;
                        }
                        if (value != null && value.StartsWith("unity-") && field.Name.EndsWith("ClassName", StringComparison.OrdinalIgnoreCase))
                        {
                            names.Add(value);
                        }
                    }
                    else
                    {
                        stack.Clear();
                    }
                }
            }
            if (known.Count(kv => kv.Value != null) == before)
            {
                break;
            }
        }
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }
}
