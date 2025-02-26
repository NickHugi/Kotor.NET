using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA;

public class AddRow2DAModifier
{
    public required IRowLocator? OverrideRowLocator { get; init; }
    public required ICellValue RowHeader { get; init; }
    public required Dictionary<string, ICellValue> Values { get; init; }
    public required Dictionary<string, ICellValue> AssignTo2DAMemory { get; init; }
    public required Dictionary<string, ICellValue> AssignToTLKMemory { get; init; }

    public void Apply(TwoDA twoda, Memory2DA memory2DA, MemoryTLK memoryTLK, PatchLogger log)
    {
        var row = OverrideRowLocator?.TryLocate(twoda);

        if (row is null)
        {
            var header = RowHeader.Resolve(twoda, null, memory2DA, memoryTLK);
            row = twoda.AddRow(header);
        }
        else
        {
            var header = RowHeader.Resolve(twoda, row, memory2DA, memoryTLK);
            row.RowHeader = header;
        }

        Values.ToList().ForEach(x =>
        {
            var header = x.Key;
            var value = x.Value.Resolve(twoda, row, memory2DA, memoryTLK);
            row.GetCell(header).SetString(value);
        });

        AssignTo2DAMemory.ToList().ForEach(x =>
        {
            string key = x.Key;
            string value = x.Value.Resolve(twoda, row, memory2DA, memoryTLK);
            memory2DA.Set(key, value);
        });
    }
}
