using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class CellValueHighest : ICellValue
{
    public required string ColumnHeader { get; init; }

    public string Resolve(TwoDA twoda, TwoDARow row, Memory memory)
    {
        if (row is null)
        {
            throw new PatchingException();
        }

        if (!twoda.GetColumns().Contains(ColumnHeader))
        {
            throw new PatchingException();
        }

        return twoda.GetRows().Select(x => int.TryParse(x.GetCell(ColumnHeader).AsString(), out int value) ? value : 0).Max().ToString();
    }
}
