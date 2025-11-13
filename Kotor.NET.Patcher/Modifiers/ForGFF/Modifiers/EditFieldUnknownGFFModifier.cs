using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Modifiers;

public class EditFieldUnknownGFFModifier : IGFFModifier
{
    public required string Label { get; set; }
    public required BindingPath Path { get; set; }
    public required BaseValue Value { get; set; }

    public void Apply(GFFStruct @struct, PatcherMemory memory)
    {
        @struct = Path.ResolveStruct(@struct);

        if (@struct.HasUInt8(Label))
        {
            @struct.SetUInt8(Label, Value.AsUInt8());
        }
        else if (@struct.HasInt8(Label))
        {
            @struct.SetInt8(Label, Value.AsInt8());
        }
        else if (@struct.HasUInt16(Label))
        {
            @struct.SetUInt16(Label, Value.AsUInt16());
        }
        else if (@struct.HasInt16(Label))
        {
            @struct.SetInt16(Label, Value.AsInt16());
        }
        else if (@struct.HasUInt32(Label))
        {
            @struct.SetUInt32(Label, Value.AsUInt32());
        }
        else if (@struct.HasInt32(Label))
        {
            @struct.SetInt32(Label, Value.AsInt32());
        }
        else if (@struct.HasUInt64(Label))
        {
            @struct.SetUInt64(Label, Value.AsUInt64());
        }
        else if (@struct.HasInt64(Label))
        {
            @struct.SetInt64(Label, Value.AsInt64());
        }
        else if (@struct.HasSingle(Label))
        {
            @struct.SetSingle(Label, Value.AsSingle());
        }
        else if (@struct.HasDouble(Label))
        {
            @struct.SetDouble(Label, Value.AsDouble(memory));
        }
        else if (@struct.HasString(Label))
        {
            @struct.SetString(Label, Value.AsString());
        }
        else if (@struct.HasResRef(Label))
        {
            @struct.SetResRef(Label, Value.AsResRef());
        }
        else if (@struct.HasLocalizedString(Label))
        {
            throw new NotImplementedException();
        }
        else if (@struct.HasBinary(Label))
        {
            throw new NotImplementedException();
        }
        else if (@struct.HasStruct(Label))
        {
            throw new NotImplementedException();
        }
        else if (@struct.HasList(Label))
        {
            throw new NotImplementedException();
        }
        else if (@struct.HasVector3(Label))
        {
            @struct.SetVector3(Label, Value.AsVector3());
        }
        else if (@struct.HasVector4(Label))
        {
            @struct.SetVector4(Label, Value.AsVector4());
        }
        else
        {
            throw new PatchingException($"Trying to assign a value to '{Label}' but it does not exist.");
        }
    }
}
