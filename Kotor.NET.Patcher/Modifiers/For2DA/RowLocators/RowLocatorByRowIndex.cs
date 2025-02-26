using System;
using System.Collections.Generic;
using System.Linq;
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

    public TwoDARow Locate(TwoDA twoda)
    {
        return twoda.GetRows().ElementAtOrDefault(Index) ?? throw new PatchingException();
    }
}
