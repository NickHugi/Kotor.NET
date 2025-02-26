using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public class CellValueRowIndex : ICellValue
{
    public string Resolve(TwoDA twoda, TwoDARow row, Memory2DA memory2DA, MemoryTLK memoryTLK)
    {
        return row.Index.ToString();
    }
}
