using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

// Prints every *UssClassName a Unity assembly's static constructors assign, folding ldstr, ldsfld and String.Concat.
static class UnityClasses
{
    static void Main(string[] args)
    {
        var known = new Dictionary<string, string>();
        var names = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var path in args)
        {
            var types = ModuleDefinition.ReadModule(path).Types.SelectMany(t => new[] { t }.Concat(t.NestedTypes));
            foreach (var cctor in types.Select(t => t.Methods.FirstOrDefault(m => m.Name == ".cctor")).Where(m => m?.HasBody == true))
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
                        var parts = new List<string>();
                        for (var i = 0; i < ((MethodReference)ins.Operand).Parameters.Count && stack.Count > 0; i++)
                        {
                            parts.Insert(0, stack.Pop());
                        }
                        stack.Push(parts.Any(p => p == null) ? null : string.Concat(parts));
                    }
                    else if (ins.OpCode == OpCodes.Stsfld)
                    {
                        var field = (FieldReference)ins.Operand;
                        var value = stack.Count > 0 ? stack.Pop() : null;
                        known[field.DeclaringType.FullName + "." + field.Name] = value;
                        if (value != null && field.Name.EndsWith("UssClassName", StringComparison.OrdinalIgnoreCase))
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
        }
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }
}
