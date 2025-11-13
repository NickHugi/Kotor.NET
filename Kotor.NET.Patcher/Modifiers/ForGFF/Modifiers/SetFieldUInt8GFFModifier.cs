using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;

public class SetFieldUInt8GFFModifier : IGFFModifier
{
    public required string Label { get; set; }
    public required BindingPath Path { get; set; }
    public required BaseValue Value { get; set; }
    public required bool MustAlreadyExist { get; set; }

    public void Apply(GFFStruct @struct, PatcherMemory memory)
    {
        @struct = Path.ResolveStruct(@struct);
        var value = Value.AsUInt8();

        if (MustAlreadyExist && !@struct.GetUInt8(Label).HasValue)
            throw new PatchingException($"The field '{Label}' must exist before assigning a value to it.");

        @struct.SetUInt8(Label, value);
    }
}
