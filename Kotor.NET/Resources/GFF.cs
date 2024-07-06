using Kotor.NET.Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources;

public class GFF
{
    public GFFStruct Root { get; set; }
}

public class GFFStruct
{
    public byte? GetUInt8(string fieldName)
    {
        throw new NotImplementedException();
    }
    public sbyte? GetInt8(string fieldName)
    {
        throw new NotImplementedException();
    }
    public ushort? GetUInt16(string fieldName)
    {
        throw new NotImplementedException();
    }
    public short? GetInt16(string fieldName)
    {
        throw new NotImplementedException();
    }
    public uint? GetUInt32(string fieldName)
    {
        throw new NotImplementedException();
    }
    public int? GetInt32(string fieldName)
    {
        throw new NotImplementedException();
    }
    public ulong? GetUInt64(string fieldName)
    {
        throw new NotImplementedException();
    }
    public long? GetInt64(string fieldName)
    {
        throw new NotImplementedException();
    }
    public float? GetSingle(string fieldName)
    {
        throw new NotImplementedException();
    }
    public double? GetDouble(string fieldName)
    {
        throw new NotImplementedException();
    }
    public string? GetString(string fieldName)
    {
        throw new NotImplementedException();
    }
    public ResRef? GetResRef(string fieldName)
    {
        throw new NotImplementedException();
    }
    public LocalisedString? GetLocalizedString(string fieldName)
    {
        throw new NotImplementedException();
    }
    public byte[]? GetBinary(string fieldName)
    {
        throw new NotImplementedException();
    }
    public GFFStruct GetStruct(string fieldName)
    {
        throw new NotImplementedException();
    }
    public GFFList GetList(string fieldName)
    {
        throw new NotImplementedException();
    }
    public Vector3 GetVector3(string fieldName)
    {
        throw new NotImplementedException();
    }
    public Vector4 GetVector4(string fieldName)
    {
        throw new NotImplementedException();
    }
}

public class GFFList
{
    private List<GFFStruct> Structs = new();

    public int Count()
    {
        return Structs.Count;
    }

    public GFFStruct StructAt(int index)
    {
        return Structs[index];
    }
}
