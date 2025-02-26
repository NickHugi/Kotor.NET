using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA;

public class EditRow2DAModifier
{
    public required IRowLocator TargetRowLocator { get; init; }
    public required ICellValue? RowHeader { get; init; }
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, Memory2DA memory2DA, MemoryTLK memoryTLK, PatchLogger log)
    {
        var row = TargetRowLocator.Locate(twoda);

        if (RowHeader is not null)
        {
            row.RowHeader = RowHeader.Resolve(twoda, row, memory2DA, memoryTLK);
        }

        Assignments.ForEach(x => x.Assign(twoda, row, memory2DA, memoryTLK));
    }
}
