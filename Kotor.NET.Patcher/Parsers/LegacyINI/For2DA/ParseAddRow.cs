using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using Kotor.NET.Patcher.Extensions;
using Kotor.NET.Patcher.Modifiers.For2DA;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.For2DA;

public class ParseAddRow
{
    public AddRow2DAModifier Parse(KeyDataCollection section)
    {
        return new()
        {
            OverrideRowLocator = ParseOverrideRowLocator(section),
            Assignments = ParseAssignments(section),
        };
    }

    private IRowLocator? ParseOverrideRowLocator(KeyDataCollection section)
    {
        if (section.TryGetString("ExclusiveColumn", out var rowHeader))
        {
            return new RowLocatorByColumn { ColumnHeader = rowHeader, Value = section.Single(x => x.KeyName == rowHeader).GetValueResolver() };
        }
        else
        {
            return null;
        }
    }

    private List<IAssignment> ParseAssignments(KeyDataCollection section)
    {
        return section
            .Where(x => x.KeyName != "ExclusiveColumn")
            .Select<KeyData, IAssignment>(entry =>
            {
                var value = entry.GetValueResolver();

                if (entry.KeyName.StartsWith("2DAMEMORY"))
                {
                    return new MemoryAssignment() { Key = entry.KeyName, Value = value };
                }
                else if (entry.KeyName.StartsWith("StrRef"))
                {
                    return new MemoryAssignment() { Key = entry.KeyName, Value = value };
                }
                else if (entry.KeyName.StartsWith("RowLabel"))
                {
                    return new RowHeaderAssignment() { Value = value };
                }
                else
                {
                    return new RowCellAssignment() { ColumnHeader = entry.KeyName, Value = value };
                }
            })
            .ToList();
    }

}
