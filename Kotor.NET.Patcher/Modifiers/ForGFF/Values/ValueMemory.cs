using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Values;

public class ValueMemory : BaseValue
{
    public required string Key { get; init; }

    public override double AsDouble(PatcherMemory memory)
    {
        return memory.GetDouble(Key);
    }
    public override short AsInt16(PatcherMemory memory)
    {
        return memory.GetShort(Key);
    }
    public override int AsInt32(PatcherMemory memory)
    {
        return memory.GetInt(Key);
    }
    public override long AsInt64(PatcherMemory memory)
    {
        return memory.GetLong(Key);
    }
    public override sbyte AsInt8(PatcherMemory memory)
    {
        return memory.GetSByte(Key);
    }
    public override ResRef AsResRef(PatcherMemory memory)
    {
        return memory.GetResRef(Key);
    }
    public override float AsSingle(PatcherMemory memory)
    {
        return memory.GetFloat(Key);
    }
    public override string AsString(PatcherMemory memory)
    {
        return memory.GetString(Key);
    }
    public override ushort AsUInt16(PatcherMemory memory)
    {
        return memory.GetUShort(Key);
    }
    public override uint AsUInt32(PatcherMemory memory)
    {
        return memory.GetUInt(Key);
    }
    public override ulong AsUInt64(PatcherMemory memory)
    {
        return memory.GetULong(Key);
    }
    public override byte AsUInt8(PatcherMemory memory)
    {
        return memory.GetByte(Key);
    }
    public override Vector3 AsVector3(PatcherMemory memory)
    {
        var tokens = memory.GetString(Key).Split("|");
        var x = float.Parse(tokens.ElementAt(0));
        var y = float.Parse(tokens.ElementAt(1));
        var z = float.Parse(tokens.ElementAt(2));
        return new(x, y, z);
    }
    public override Vector4 AsVector4(PatcherMemory memory)
    {
        var tokens = memory.GetString(Key).Split("|");
        var x = float.Parse(tokens.ElementAt(0));
        var y = float.Parse(tokens.ElementAt(1));
        var z = float.Parse(tokens.ElementAt(2));
        var w = float.Parse(tokens.ElementAt(3));
        return new(x, y, z, w);
    }
}
