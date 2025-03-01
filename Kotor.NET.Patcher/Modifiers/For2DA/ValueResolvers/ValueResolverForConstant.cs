using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class ValueResolverForConstant : BaseValueResolver
{
    public required string Value { get; init; }

    public override string Resolve(TwoDA twoda, TwoDARow row, PatcherMemory memory)
    {
        return Value;
    }
}
