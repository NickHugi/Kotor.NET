using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Values;

public abstract class BaseValue
{
    public abstract byte AsUInt8(PatcherMemory memory);
    public abstract sbyte AsInt8(PatcherMemory memory);
    public abstract ushort AsUInt16(PatcherMemory memory);
    public abstract short AsInt16(PatcherMemory memory);
    public abstract uint AsUInt32(PatcherMemory memory);
    public abstract int AsInt32(PatcherMemory memory);
    public abstract ulong AsUInt64(PatcherMemory memory);
    public abstract long AsInt64(PatcherMemory memory);
    public abstract float AsSingle(PatcherMemory memory);
    public abstract double AsDouble(PatcherMemory memory);
    public abstract string AsString(PatcherMemory memory);
    public abstract ResRef AsResRef(PatcherMemory memory);
    public abstract Vector3 AsVector3(PatcherMemory memory);
    public abstract Vector4 AsVector4(PatcherMemory memory);
}
