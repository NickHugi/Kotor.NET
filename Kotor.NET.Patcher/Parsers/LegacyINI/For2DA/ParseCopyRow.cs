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

public class ParseCopyRow
{
    public CopyRow2DAModifier Parse(KeyDataCollection section)
    {
        return new()
        {
            OverrideRowLocator = ParseOverrideRowLocator(section),
            BlueprintRowLocator = ParseBlueprintRowLocator(section),
            Assignments = ParseAssignments(section),
        };
    }

    private IRowLocator ParseBlueprintRowLocator(KeyDataCollection section)
    {
        if (section.TryGetInt32("RowIndex", out var rowIndex))
        {
            return new RowLocatorByRowIndex { Index = rowIndex };
        }
        else if (section.TryGetString("RowLabel", out var rowHeader))
        {
            return new RowLocatorByRowHeader { RowHeader = rowHeader };
        }
        else if (section.TryGetString("LabelIndex", out var cellValue))
        {
            return new RowLocatorByColumn { ColumnHeader = "label", Value = cellValue };
        }
        else
        {
            throw new ParsingException($"Did not specify a row to copy.");
        }
    }

    public IRowLocator? ParseOverrideRowLocator(KeyDataCollection section)
    {
        if (section.TryGetString("ExclusiveColumn", out var columnHeader))
        {
            if (section.TryGetString(columnHeader, out var value))
            {
                return new RowLocatorByColumn { ColumnHeader = columnHeader, Value = value };
            }
            else
            {
                throw new ParsingException($"The specified unqiue column '{columnHeader}' did not have a value specified for the row.");
            }
        }
        else
        {
            return null;
        }
    }

    private List<IAssignment> ParseAssignments(KeyDataCollection section)
    {
        return section
            .Where(x => x.KeyName != "RowIndex")
            .Where(x => x.KeyName != "RowLabel")
            .Where(x => x.KeyName != "LabelIndex")
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
                else if (entry.KeyName == "NewRowLabel")
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
