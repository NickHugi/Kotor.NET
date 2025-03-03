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

public class CopyRow2DAModifier : I2DAModifier
{
    public required IRowLocator BlueprintRowLocator { get; init; }
    public required IRowLocator? OverrideRowLocator { get; init; }
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, PatcherMemory memory, PatcherLogger log)
    {
        var source = BlueprintRowLocator.Locate(twoda, memory);
        TwoDARow? target = null;

        if (OverrideRowLocator is not null)
        {
            OverrideRowLocator.TryLocate(twoda, out target, memory);
        }
        if (target is null)
        {
            target = twoda.AddRow("", source);
        }

        Assignments.ForEach(x => x.Assign(twoda, target, memory));
    }
}
