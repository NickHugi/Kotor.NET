using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA;

public class AddColumn2DAModifier : I2DAModifier
{
    /// <summary>
    /// The header to be assigned to the added column.
    /// </summary>
    public required string ColumnHeader { get; init; }
    /// <summary>
    /// The default value to assign new cells under the added column.
    /// </summary>
    public required string DefaultValue { get; init; }
    /// <summary>
    /// List of actions to perform after the column has been added.
    /// </summary>
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, PatcherMemory memory, PatcherLogger log)
    {
        twoda.AddColumn(ColumnHeader, DefaultValue);

        Assignments.ForEach(x => x.Assign(twoda, null, memory));
    }
}
