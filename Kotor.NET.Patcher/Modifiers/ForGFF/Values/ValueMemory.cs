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
    public override short AsInt16() => throw new NotImplementedException();
    public override int AsInt32() => throw new NotImplementedException();
    public override long AsInt64() => throw new NotImplementedException();
    public override sbyte AsInt8() => throw new NotImplementedException();
    public override ResRef AsResRef() => throw new NotImplementedException();
    public override float AsSingle() => throw new NotImplementedException();
    public override string AsString() => throw new NotImplementedException();
    public override ushort AsUInt16() => throw new NotImplementedException();
    public override uint AsUInt32() => throw new NotImplementedException();
    public override ulong AsUInt64() => throw new NotImplementedException();
    public override byte AsUInt8() => throw new NotImplementedException();
    public override Vector3 AsVector3() => throw new NotImplementedException();
    public override Vector4 AsVector4() => throw new NotImplementedException();
}
