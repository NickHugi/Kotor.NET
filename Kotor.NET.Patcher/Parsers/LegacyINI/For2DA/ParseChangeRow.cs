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

public class ParseChangeRow
{
    public EditRow2DAModifier Parse(KeyDataCollection section)
    {
        return new()
        {
            TargetRowLocator = ParseTargetRowLocator(section),
            Assignments = ParseAssignments(section),
        };
    }

    private IRowLocator ParseTargetRowLocator(KeyDataCollection section)
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
            return new RowLocatorByColumn { ColumnHeader = "label", Value = new ValueResolverForConstant { Value = cellValue } };
        }
        else
        {
            throw new ParsingException($"Did not specify which column to edit.");
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
                else
                {
                    return new RowCellAssignment() { ColumnHeader = entry.KeyName, Value = value };
                }
            })
            .ToList();
    }

}
