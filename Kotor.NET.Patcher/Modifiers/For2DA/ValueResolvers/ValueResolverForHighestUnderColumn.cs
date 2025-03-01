using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class ValueResolverForHighestUnderColumn : BaseValueResolver
{
    public required string ColumnHeader { get; init; }

    public override string Resolve(TwoDA twoda, TwoDARow? row, PatcherMemory memory)
    {
        if (row is null)
        {
            throw new PatchingException($"Try to get value for column '{ColumnHeader}' in an invalid context.");
        }

        if (!twoda.GetColumns().Contains(ColumnHeader))
        {
            throw new PatchingException($"Could not find column '{ColumnHeader}'.");
        }

        return twoda.GetRows().Select(x => int.TryParse(x.GetCell(ColumnHeader).AsString(), out int value) ? value : 0).Max().ToString();
    }
}
