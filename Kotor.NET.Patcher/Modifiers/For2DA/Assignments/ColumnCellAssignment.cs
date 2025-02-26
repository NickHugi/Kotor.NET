using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Assignments;

public class ColumnCellAssignment : IAssignment
{
    public required IRowLocator RowLocator { get; init; }
    public required string ColumnHeader { get; init; }
    public required ICellValue Value { get; init; }

    public void Assign(TwoDA twoda, TwoDARow _, Memory2DA memory2DA, MemoryTLK memoryTLK)
    {
        var row = RowLocator.Locate(twoda);
        var value = Value.Resolve(twoda, row, memory2DA, memoryTLK);
        row.GetCell(ColumnHeader).SetString(value);
    }
}
