using IniParser.Model;
using Kotor.NET.Patcher.Extensions;
using Kotor.NET.Patcher.Modifiers.For2DA;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;

namespace Kotor.NET.Patcher.Parsers.LegacyINI.For2DA;

public class ParseAddColumn
{
    public AddColumn2DAModifier Parse(KeyDataCollection section)
    {
        return new()
        {
            ColumnHeader = ParseColumnHeader(section),
            DefaultValue = ParseDefaultValue(section),
            Assignments = ParseAssignments(section),
        };
    }

    private string ParseColumnHeader(KeyDataCollection section)
    {
        if (section.TryGetString("ColumnLabel", out var columnHeader))
        {
            return columnHeader;
        }
        else
        {
            throw new ParsingException("Trying to add a new column without specifying a header for it.");
        }
    }

    private string ParseDefaultValue(KeyDataCollection section)
    {
        if (section.TryGetString("DefaultValue", out var defaultValue))
        {
            return defaultValue;
        }
        else
        {
            return "";
        }
    }

    private List<IAssignment> ParseAssignments(KeyDataCollection section)
    {
        return section
            .Where(x => x.KeyName != "ColumnLabel")
            .Where(x => x.KeyName != "DefaultValue")
            .Select<KeyData, IAssignment>(entry =>
            {
                var columnHeader = ParseColumnHeader(section);

                if (entry.KeyName.StartsWith("2DAMEMORY"))
                {
                    var value = GetValueResolverForMemory(entry.Value, columnHeader);
                    return new MemoryAssignment() { Key = entry.KeyName, Value = value };
                }
                else
                {
                    var rowLocator = GetRowLocator(entry.KeyName);
                    var value = entry.GetValueResolver();
                    return new ColumnCellAssignment() { ColumnHeader = columnHeader, RowLocator = rowLocator, Value = value };
                }
            })
            .ToList();
    }

    private IRowLocator GetRowLocator(string text)
    {
        if (text.StartsWith("I") && int.TryParse(text.Substring(1), out var rowIndex))
        {
            return new RowLocatorByRowIndex { Index = rowIndex };
        }
        if (text.StartsWith("L"))
        {
            var rowHeader = text.Substring(1);
            return new RowLocatorByRowHeader { RowHeader = rowHeader };
        }
        else
        {
            throw new ParsingException($"An invalid identifier ");
        }
    }

    private BaseValueResolver GetValueResolverForMemory(string text, string columnHeader)
    {
        if (text.StartsWith("I") && int.TryParse(text.Substring(1), out var rowIndex))
        {
            var rowLocator = new RowLocatorByRowIndex { Index = rowIndex };
            return new ValueResolverForExistingCell { ColumnHeader = columnHeader, RowLocator = rowLocator };
        }
        if (text.StartsWith("L"))
        {
            var rowHeader = text.Substring(1);
            var rowLocator = new RowLocatorByRowHeader { RowHeader = rowHeader };
            return new ValueResolverForExistingCell { ColumnHeader = columnHeader, RowLocator = rowLocator };
        }
        else
        {
            throw new ParsingException($"An invalid identifier was listed to be assigned for column '{columnHeader}'.");
        }
    }
}
