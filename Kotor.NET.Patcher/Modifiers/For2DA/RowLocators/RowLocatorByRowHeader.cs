using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

/// <summary>
/// Locate a row by its Row Header.
/// </summary>
public class RowLocatorByRowHeader : IRowLocator
{
    public required string RowHeader { get; init; }

    public TwoDARow Locate(TwoDA twoda)
    {
        var rows = twoda.GetRows().Where(x => x.RowHeader == RowHeader).ToList();

        if (rows.Count == 0)
        {
            throw new PatchingException($"Could not find a row with the header '{RowHeader}'.");
        }
        else if (rows.Count > 1)
        {
            throw new PatchingException($"Found multiple rows matching the header '{RowHeader}'.");
        }
        else
        {
            return rows.Single();
        }
    }

    public bool TryLocate(TwoDA twoda, out TwoDARow? row)
    {
        try
        {
            row = Locate(twoda);
            return true;
        }
        catch (PatchingException)
        {
            row = null;
            return false;
        }
    }
}
