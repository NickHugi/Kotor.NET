using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Values;

public abstract class BaseValue
{
    public abstract byte AsUInt8();
    public abstract sbyte AsInt8();
    public abstract ushort AsUInt16();
    public abstract short AsInt16();
    public abstract uint AsUInt32();
    public abstract int AsInt32();
    public abstract ulong AsUInt64();
    public abstract long AsInt64();
    public abstract float AsSingle();
    public abstract double AsDouble(PatcherMemory memory);
    public abstract string AsString();
    public abstract ResRef AsResRef();
    public abstract Vector3 AsVector3();
    public abstract Vector4 AsVector4();
}
