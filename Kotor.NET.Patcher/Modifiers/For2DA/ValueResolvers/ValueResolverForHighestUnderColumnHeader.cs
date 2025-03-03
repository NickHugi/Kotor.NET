using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class ValueResolverForHighestUnderColumnHeader : BaseValueResolver
{
    public override string Resolve(TwoDA twoda, TwoDARow? row, PatcherMemory memory)
    {
        return twoda.GetRows().Select(x => int.TryParse(x.RowHeader, out var value) ? value : 0).Max().ToString();
    }
}
