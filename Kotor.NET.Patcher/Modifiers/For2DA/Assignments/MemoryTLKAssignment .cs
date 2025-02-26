using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.Assignments;

public class MemoryTLKAssignment : IAssignment
{
    public required string Key { get; init; }
    public required ICellValue Value { get; init; }

    public void Assign(TwoDA twoda, TwoDARow row, Memory2DA memory2DA, MemoryTLK memoryTLK)
    {
        var value = Value.Resolve(twoda, row, memory2DA, memoryTLK);

        if (int.TryParse(value, out var valueAsInt))
        {
            memoryTLK.Set(Key, valueAsInt);
        }
        else
        {
            throw new PatchingException();
        }
    }
}
