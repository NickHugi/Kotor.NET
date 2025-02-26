using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;

namespace Kotor.NET.Patcher.Modifiers.ForGFF.Values;

public class ValueConstant : BaseValue
{
    public required object Value { get; set; }

    public override double? AsDouble() => throw new NotImplementedException();
    public override short? AsInt16() => throw new NotImplementedException();
    public override int? AsInt32() => throw new NotImplementedException();
    public override long? AsInt64() => throw new NotImplementedException();
    public override sbyte? AsInt8() => throw new NotImplementedException();
    public override ResRef? AsResRef() => throw new NotImplementedException();
    public override float? AsSingle() => throw new NotImplementedException();
    public override string? AsString() => throw new NotImplementedException();
    public override ushort? AsUInt16() => throw new NotImplementedException();
    public override uint? AsUInt32() => throw new NotImplementedException();
    public override ulong? AsUInt64() => throw new NotImplementedException();
    public override byte? AsUInt8() => throw new NotImplementedException();
}
