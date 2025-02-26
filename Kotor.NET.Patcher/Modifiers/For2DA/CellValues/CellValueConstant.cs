using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class CellValueConstant : ICellValue
{
    public required string Value { get; init; }

    public string Resolve(TwoDA twoda, TwoDARow row, Memory2DA memory2DA, MemoryTLK memoryTLK)
    {
        return Value;
    }
}
