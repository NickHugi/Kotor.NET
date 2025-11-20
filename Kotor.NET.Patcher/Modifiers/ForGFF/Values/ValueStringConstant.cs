using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Values;

public class ValueStringConstant : BaseValue
{
    public required string Value { get; init; }

    public override byte AsUInt8(PatcherMemory memory) => byte.Parse(Value);
    public override sbyte AsInt8(PatcherMemory memory) => sbyte.Parse(Value);
    public override ushort AsUInt16(PatcherMemory memory) => ushort.Parse(Value);
    public override short AsInt16(PatcherMemory memory) => short.Parse(Value);
    public override uint AsUInt32(PatcherMemory memory) => uint.Parse(Value);
    public override int AsInt32(PatcherMemory memory) => int.Parse(Value);
    public override ulong AsUInt64(PatcherMemory memory) => ulong.Parse(Value);
    public override long AsInt64(PatcherMemory memory) => long.Parse(Value);
    public override float AsSingle(PatcherMemory memory) => float.Parse(Value);
    public override double AsDouble(PatcherMemory memory) => double.Parse(Value);
    public override ResRef AsResRef(PatcherMemory memory) => new(Value);
    public override string AsString(PatcherMemory memory) => Value;
    public override Vector3 AsVector3(PatcherMemory memory)
    {
        var tokens = Value.Split("|");
        var x = float.Parse(tokens.ElementAt(0));
        var y = float.Parse(tokens.ElementAt(1));
        var z = float.Parse(tokens.ElementAt(2));
        return new(x, y, z);
    }
    public override Vector4 AsVector4(PatcherMemory memory)
    {
        var tokens = Value.Split("|");
        var x = float.Parse(tokens.ElementAt(0));
        var y = float.Parse(tokens.ElementAt(1));
        var z = float.Parse(tokens.ElementAt(2));
        var w = float.Parse(tokens.ElementAt(3));
        return new(x, y, z, w);
    }
}
