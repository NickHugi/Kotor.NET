using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Localization;

public readonly struct StringRef
{
    private readonly int _value;

    public StringRef(int stringref)
    {
        _value = stringref;
    }

    public override string ToString() => _value.ToString();

    public static implicit operator int(StringRef stringref) => stringref._value;
    public static implicit operator StringRef(int stringref) => new(stringref);
}
