using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorGFF;

public readonly struct GFFStructID
{
    private readonly uint _value;

    internal GFFStructID(uint value)
    {
        _value = value;
    }

    public override string ToString()
    {
        return _value.ToString();
    }

    public static implicit operator GFFStructID(uint value)
    {
        return new(value);
    }
    public static implicit operator GFFStructID(int value)
    {
        return new((uint)value);
    }
    public static implicit operator uint(GFFStructID structID)
    {
        return structID._value;
    }
    public static implicit operator int(GFFStructID structID)
    {
        return (int)structID._value;
    }
}
