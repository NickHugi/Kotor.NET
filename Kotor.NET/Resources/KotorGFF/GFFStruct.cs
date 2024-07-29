using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorGFF;

public class GFFStruct
{
    internal Dictionary<string, object> _data = new();

    public uint ID { get; set; }

    public GFFStruct()
    {
    }
    public GFFStruct(uint id)
    {
        ID = id;
    }

    public byte? GetUInt8(string fieldName)
    {
        return GetValue<byte?>(fieldName);
    }
    public sbyte? GetInt8(string fieldName)
    {
        return GetValue<sbyte?>(fieldName);
    }
    public ushort? GetUInt16(string fieldName)
    {
        return GetValue<ushort?>(fieldName);
    }
    public short? GetInt16(string fieldName)
    {
        return GetValue<short?>(fieldName);
    }
    public uint? GetUInt32(string fieldName)
    {
        return GetValue<uint?>(fieldName);
    }
    public int? GetInt32(string fieldName)
    {
        return GetValue<int?>(fieldName);
    }
    public ulong? GetUInt64(string fieldName)
    {
        return GetValue<ulong?>(fieldName);
    }
    public long? GetInt64(string fieldName)
    {
        return GetValue<long?>(fieldName);
    }
    public float? GetSingle(string fieldName)
    {
        return GetValue<float?>(fieldName);
    }
    public double? GetDouble(string fieldName)
    {
        return GetValue<double?>(fieldName);
    }
    public string? GetString(string fieldName)
    {
        return GetValue<string?>(fieldName);
    }
    public ResRef? GetResRef(string fieldName)
    {
        return GetValue<ResRef?>(fieldName);
    }
    public LocalisedString? GetLocalisedString(string fieldName)
    {
        return GetValue<LocalisedString?>(fieldName);
    }
    public byte[]? GetBinary(string fieldName)
    {
        return GetValue<byte[]?>(fieldName);
    }
    public GFFStruct? GetStruct(string fieldName)
    {
        return GetValue<GFFStruct?>(fieldName);
    }
    public GFFList? GetList(string fieldName)
    {
        return GetValue<GFFList?>(fieldName);
    }
    public Vector3? GetVector3(string fieldName)
    {
        return GetValue<Vector3?>(fieldName);
    }
    public Vector4? GetVector4(string fieldName)
    {
        return GetValue<Vector4?>(fieldName);
    }

    public void SetUInt8(string fieldName, byte value)
    {
        SetValue<byte>(fieldName, value);
    }
    public void SetInt8(string fieldName, sbyte value)
    {
        SetValue<sbyte>(fieldName, value);
    }
    public void SetUInt16(string fieldName, ushort value)
    {
        SetValue<ushort>(fieldName, value);
    }
    public void SetInt16(string fieldName, short value)
    {
        SetValue<short>(fieldName, value);
    }
    public void SetUInt32(string fieldName, uint value)
    {
        SetValue<uint>(fieldName, value);
    }
    public void SetInt32(string fieldName, int value)
    {
        SetValue<int>(fieldName, value);
    }
    public void SetUInt64(string fieldName, ulong value)
    {
        SetValue<ulong>(fieldName, value);
    }
    public void SetInt64(string fieldName, long value)
    {
        SetValue<long>(fieldName, value);
    }
    public void SetSingle(string fieldName, float value)
    {
        SetValue<float>(fieldName, value);
    }
    public void SetDouble(string fieldName, double value)
    {
        SetValue<double>(fieldName, value);
    }
    public void SetString(string fieldName, string value)
    {
        SetValue<string>(fieldName, value);
    }
    public void SetResRef(string fieldName, ResRef value)
    {
        SetValue<ResRef>(fieldName, value);
    }
    public void SetLocalisedString(string fieldName, LocalisedString value)
    {
        SetValue<LocalisedString>(fieldName, value);
    }
    public void SetBinary(string fieldName, byte[] value)
    {
        SetValue<byte[]>(fieldName, value);
    }
    public void SetStruct(string fieldName, GFFStruct value)
    {
        SetValue<GFFStruct>(fieldName, value);
    }
    public GFFStruct SetStruct(string fieldName, uint structID = 0)
    {
        var value = new GFFStruct(structID);
        SetValue<GFFStruct>(fieldName, value);
        return value;
    }
    public void SetList(string fieldName, GFFList value)
    {
        SetValue<GFFList>(fieldName, value);
    }
    public GFFList SetList(string fieldName)
    {
        var value = new GFFList();
        SetValue<GFFList>(fieldName, value);
        return value;
    }
    public void SetVector3(string fieldName, Vector3 value)
    {
        SetValue<Vector3>(fieldName, value);
    }
    public void SetVector3(string fieldName, float x, float y, float z)
    {
        SetValue<Vector3>(fieldName, new(x, y, z));
    }
    public void SetVector4(string fieldName, Vector4 value)
    {
        SetValue<Vector4>(fieldName, value);
    }
    public void SetVector4(string fieldName, float x, float y, float z, float w)
    {
        SetValue<Vector4>(fieldName, new(x, y, z, w));
    }

    public int FieldCount()
    {
        return _data.Count();
    }
    public void DeleteField(string fieldName)
    {
        if (_data.ContainsKey(fieldName))
            _data.Remove(fieldName);
    }

    public override string ToString()
    {
        return "StructID=" + ID + ", Count=" + _data.Count();
    }

    internal List<string> GetAllStructs()
    {
        return GetAllFieldNamesOfType<GFFStruct>();
    }
    internal List<string> GetAllLists()
    {
        return GetAllFieldNamesOfType<GFFList>();
    }

    private T? GetValue<T>(string fieldName)
    {
        var hasField = _data.TryGetValue(fieldName, out var value);
        return (hasField && value is T) ? (T)value : default(T);
    }
    private void SetValue<T>(string fieldName, T value)
    {
        _data[fieldName] = value;
    }
    private List<string> GetAllFieldNamesOfType<T>()
    {
        var fieldNames = new List<string>();
        foreach (var item in _data)
        {
            if (item.Value is T)
                fieldNames.Add(item.Key);
        }
        return fieldNames;
    }

    internal void SetUInt16(string v1, int v2) => throw new NotImplementedException();
}
