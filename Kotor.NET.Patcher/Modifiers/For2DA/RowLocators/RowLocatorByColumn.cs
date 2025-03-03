using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

public class RowLocatorByColumn : IRowLocator
{
    public required string ColumnHeader { get; init; }
    public required BaseValueResolver Value { get; init; }

    public TwoDARow Locate(TwoDA twoda, PatcherMemory memory)
    {
        if (twoda.GetColumns().Contains(ColumnHeader))
        {
            throw new PatchingException($"Could not find column '{ColumnHeader}'.");
        }

        var value = Value.Resolve(twoda, null, memory);
        var rows = twoda.GetRows().Where(x => x.GetCell(ColumnHeader).AsString() == value).ToList();

        if (rows.Count == 0)
        {
            throw new PatchingException($"Could not find row with column '{ColumnHeader}' matching value '{Value}'.");
        }
        else if (rows.Count > 1)
        {
            throw new PatchingException($"Found more than 1 more with column '{ColumnHeader}' matching value '{Value}.");
        }
        else
        {
            return rows.Single();
        }
    }
    public bool TryLocate(TwoDA twoda, out TwoDARow? row, PatcherMemory memory)
    {
        try
        {
            row = Locate(twoda, memory);
            return true;
        }
        catch (PatchingException)
        {
            row = null;
            return false;
        }
    }
}
