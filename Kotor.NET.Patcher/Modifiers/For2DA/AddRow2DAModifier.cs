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

public class AddRow2DAModifier : I2DAModifier
{
    /// <summary>
    /// If not null, if the locator finds a row, that row will be edited rather than a new row being added.
    /// </summary>
    public required IRowLocator? OverrideRowLocator { get; init; }
    /// <summary>
    /// A list of actions to apply after the row has been found or added.
    /// </summary>
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, PatcherMemory memory, PatcherLogger log)
    {
        TwoDARow? row = null;

        if (OverrideRowLocator is not null)
        {
            OverrideRowLocator.TryLocate(twoda, out row, memory);
        }

        if (row is null)
        {
            row = twoda.AddRow("");
        }

        Assignments.ForEach(x => x.Assign(twoda, row, memory));
    }
}
