using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

public class RowLocatorByColumn : IRowLocator
{
    public required string ColumnHeader { get; init; }
    public required string Value { get; init; }

    public TwoDARow Locate(TwoDA twoda)
    {
        if (twoda.GetColumns().Contains(ColumnHeader))
        {
            throw new PatchingException();
        }

        var rows = twoda.GetRows().Where(x => x.GetCell(ColumnHeader).AsString() == Value).ToList();

        if (rows.Count == 0)
        {
            throw new PatchingException();
        }
        else if (rows.Count > 1)
        {
            throw new PatchingException();
        }
        else
        {
            return rows.Single();
        }
    }
}
