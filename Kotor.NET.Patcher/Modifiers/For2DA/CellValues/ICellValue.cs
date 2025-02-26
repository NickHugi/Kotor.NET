using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.CellValues;

public interface ICellValue
{
    public string Resolve(TwoDA twoda, TwoDARow? row, Memory2DA memory2DA, MemoryTLK memoryTLK);
    public string? TryResolve(TwoDA twoda, TwoDARow row, Memory2DA memory2DA, MemoryTLK memoryTLK)
    {
        try
        {
            return Resolve(twoda, row, memory2DA, memoryTLK);
        }
        catch (PatchingException)
        {
            return null;
        }
    }
}
