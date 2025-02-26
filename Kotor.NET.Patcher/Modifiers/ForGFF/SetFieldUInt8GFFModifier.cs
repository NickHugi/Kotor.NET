using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.ForGFF.Values;
using Kotor.NET.Resources.KotorGFF;

namespace Kotor.NET.Patcher.Modifiers.ForGFF;

public class SetFieldUInt8GFFModifier
{
    public required string Label { get; set; }
    public required Path Path { get; set; }
    public required BaseValue Value { get; set; }
    public required bool MustAlreadyExist { get; set; }

    public void Apply(GFF gff)
    {
        var @struct = Path.ResolveStruct();
        var value = Value.AsUInt8();

        if (@struct is null)
            throw new NotImplementedException(); // TODO
        if (value is null)
            throw new NotImplementedException(); // TODO
        if (MustAlreadyExist && !@struct.GetUInt8(Label).HasValue)
            throw new NotImplementedException(); // TODO

        @struct.SetUInt8(Label, value.Value);
    }
}
