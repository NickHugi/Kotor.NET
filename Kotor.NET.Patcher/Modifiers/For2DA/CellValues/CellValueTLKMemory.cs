using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class CellValueTLKMemory : ICellValue
{
    public required string Key { get; init; }

    public string Resolve(TwoDA twoda, TwoDARow row, Memory memory)
    {
        return memoryTLK.Get(Key).ToString();
    }
}
