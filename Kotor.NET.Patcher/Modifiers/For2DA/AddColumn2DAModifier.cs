using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA;

public class AddColumn2DAModifier
{
    public required string ColumnHeader { get; init; }
    public required string DefaultValue { get; init; }
    public required List<IAssignment> Assignments { get; init; }

    public void Apply(TwoDA twoda, Memory2DA memory2DA, MemoryTLK memoryTLK, PatchLogger log)
    {
        twoda.AddColumn(ColumnHeader, DefaultValue);

        Assignments.ForEach(x => x.Assign(twoda, null, memory2DA, memoryTLK));
    }
}
