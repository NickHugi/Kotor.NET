using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Modifiers;

public class EditRow2DAModifier : I2DAModifier
{
    public required IRowLocator TargetRowLocator { get; init; }
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, PatcherMemory memory, PatcherLogger log)
    {
        var row = TargetRowLocator.Locate(twoda, memory);

        Assignments.ForEach(x => x.Assign(twoda, row, memory));
    }
}
