using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA;

public class CopyRow2DAModifier
{
    public required IRowLocator BlueprintRowLocator { get; init; }
    public required IRowLocator? OverrideRowLocator { get; init; }
    public required ICellValue RowHeader { get; init; }
    public required Dictionary<string, ICellValue> Values { get; init; }
    public required Dictionary<string, ICellValue> AssignTo2DAMemory { get; init; }
    public required Dictionary<string, ICellValue> AssignToTLKMemory { get; init; }

    public void Apply(TwoDA twoda, Memory memory2DA, PatchLogger log)
    {
        var blueprintRow = BlueprintRowLocator.Locate(twoda);
        var row = OverrideRowLocator?.TryLocate(twoda);

        if (row is null)
        {
            var header = RowHeader.Resolve(twoda, null, memory2DA);
            row = twoda.AddRow(header, blueprintRow);
        }
        else
        {
            var header = RowHeader.Resolve(twoda, row, memory2DA);
            row.RowHeader = header;
        }

        Values.ToList().ForEach(x =>
        {
            var header = x.Key;
            var value = x.Value.Resolve(twoda, row, memory2DA);
            row.GetCell(header).SetString(value);
        });

        AssignTo2DAMemory.ToList().ForEach(x =>
        {
            string key = x.Key;
            string value = x.Value.Resolve(twoda, row, memory2DA);
            memory2DA.Set(key, value);
        });
    }
}
