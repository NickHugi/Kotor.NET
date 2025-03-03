using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

/// <summary>
/// Locate a row by its index into the table, starting from 0.
/// </summary>
public class RowLocatorByRowIndex : IRowLocator
{
    public required int Index { get; init; }

    public TwoDARow Locate(TwoDA twoda, PatcherMemory memory)
    {
        var row = twoda.GetRows().ElementAtOrDefault(Index);

        if (row is null)
        {
            throw new PatchingException($"No row exists at index {Index}.");
        }
        else
        {
            return row;
        }
    }

    public bool TryLocate(TwoDA twoda, out TwoDARow? row, PatcherMemory memory)
    {
        try
        {
            row = Locate(twoda, memory);
            return true;
        }
        catch (PatchingException)
        {
            row = null;
            return false;
        }
    }
}
