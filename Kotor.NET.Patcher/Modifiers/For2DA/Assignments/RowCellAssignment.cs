using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Assignments;

public class RowCellAssignment : IAssignment
{
    public required string ColumnHeader { get; init; }
    public required BaseValueResolver Value { get; init; }

    public void Assign(TwoDA twoda, TwoDARow row, PatcherMemory memory)
    {
        var value = Value.Resolve(twoda, row, memory);
        row.GetCell(ColumnHeader).SetString(value);
    }
}
