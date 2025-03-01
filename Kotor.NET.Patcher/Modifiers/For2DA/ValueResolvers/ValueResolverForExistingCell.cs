using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class ValueResolverForExistingCell : BaseValueResolver
{
    public required string ColumnHeader { get; init; }
    public required IRowLocator RowLocator { get; init; }

    public override string Resolve(TwoDA twoda, TwoDARow? _, PatcherMemory memory)
    {
        var row = RowLocator.Locate(twoda);
        var value = row.GetCell(ColumnHeader).AsString();
        return value;
    }
}
