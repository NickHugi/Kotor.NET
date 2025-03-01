using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class ValueResolverForPatcherMemory : BaseValueResolver
{
    public required string Key { get; init; }

    public override string Resolve(TwoDA twoda, TwoDARow? row, PatcherMemory memory)
    {
        return memory.Get(Key);
    }
}
