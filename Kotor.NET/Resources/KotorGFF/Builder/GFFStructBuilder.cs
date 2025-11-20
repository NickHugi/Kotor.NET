using System.Collections.Generic;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Resources.KotorGFF.Builder;

public class GFFStructBuilder
{
    private readonly GFFStruct _struct;

    internal GFFStructBuilder(GFFStruct @struct)
    {
        _struct = @struct;
    }

    public GFFStructBuilder AddInt8(string label, sbyte value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetInt8(label, value);
        return this;
    }

    public GFFStructBuilder AddInt16(string label, short value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetInt16(label, value);
        return this;
    }

    public GFFStructBuilder AddInt32(string label, int value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetInt32(label, value);
        return this;
    }

    public GFFStructBuilder AddInt64(string label, long value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetInt64(label, value);
        return this;
    }

    public GFFStructBuilder AddUInt8(string label, byte value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetUInt8(label, value);
        return this;
    }

    public GFFStructBuilder AddUInt16(string label, ushort value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetUInt16(label, value);
        return this;
    }

    public GFFStructBuilder AddUInt32(string label, uint value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetUInt32(label, value);
        return this;
    }

    public GFFStructBuilder AddUInt64(string label, ulong value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetUInt64(label, value);
        return this;
    }

    public GFFStructBuilder AddSingle(string label, float value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetSingle(label, value);
        return this;
    }

    public GFFStructBuilder AddDouble(string label, double value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetDouble(label, value);
        return this;
    }

    public GFFStructBuilder AddString(string label, string value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetString(label, value);
        return this;
    }

    public GFFStructBuilder AddResRef(string label, ResRef value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetResRef(label, value);
        return this;
    }

    public GFFStructBuilder AddBinary(string label, byte[] value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetBinary(label, value);
        return this;
    }

    public GFFStructBuilder AddVector3(string label, float x, float y, float z)
    {
        ThrowIfLabelInUse(label);
        _struct.SetVector3(label, new(x, y, z));
        return this;
    }
    public GFFStructBuilder AddVector3(string label, Vector3 value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetVector3(label, value);
        return this;
    }

    public GFFStructBuilder AddVector4(string label, float x, float y, float z, float w)
    {
        ThrowIfLabelInUse(label);
        _struct.SetVector4(label, new(x, y, z, w));
        return this;
    }
    public GFFStructBuilder AddVector4(string label, Vector4 value)
    {
        ThrowIfLabelInUse(label);
        _struct.SetVector4(label, value);
        return this;
    }

    public GFFStructBuilder AddList(string label, GFFStructID id = default, Action<GFFListBuilder>? builder = null)
    {
        ThrowIfLabelInUse(label);
        var list = _struct.SetList(label);
        if (builder is not null)
            builder(new(list));
        return this;
    }

    public GFFStructBuilder AddStruct(string label, GFFStructID id = default, Action<GFFStructBuilder>? builder = null)
    {
        ThrowIfLabelInUse(label);
        var @struct = _struct.SetStruct(label, id);
        if (builder is not null)
            builder(new(@struct));
        return this;
    }

    private void ThrowIfLabelInUse(string label)
    {
        if (_struct.GetFields().Any(x => x.Label == label))
            throw new Exception($"A field with label '{label}' has already been assigned.");
    }
}
