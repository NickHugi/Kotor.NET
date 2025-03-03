using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Parsers.LegacyINI.For2DA;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.Kotor2DA.Builder;
using IniParser;
using IniParser.Parser;
using FluentAssertions.Execution;
using FluentAssertions;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using FluentAssertions.Equivalency;

namespace Kotor.NET.Patcher.Tests.Parsers.LegacyINI.For2DA;

public class ParseAddColumnTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [add_column_2da]
                ColumnLabel=NewColumn
                DefaultValue=default
                I1=val1
                L2=val2
                2DAMEMORY1=I1
                2DAMEMORY2=L2
            """);

        var parser = new ParseAddColumn();
        var modifier = parser.Parse(ini["add_column_2da"]);

        using (new AssertionScope())
        {
            modifier.ColumnHeader.Should().Be("NewColumn");
            modifier.DefaultValue.Should().Be("default");

            modifier.Assignments.Should().HaveCount(4);

            modifier.Assignments.Should().ContainEquivalentOf(new ColumnCellAssignment
            {
                RowLocator = new RowLocatorByRowIndex { Index = 1 },
                ColumnHeader = "NewColumn",
                Value = new ValueResolverForConstant { Value = "val1" },
            }, config => config.IncludingAllRuntimeProperties());

            modifier.Assignments.Should().ContainEquivalentOf(new ColumnCellAssignment
            {
                RowLocator = new RowLocatorByRowHeader { RowHeader = "2" },
                ColumnHeader = "NewColumn",
                Value = new ValueResolverForConstant { Value = "val2" },
            }, config => config.IncludingAllRuntimeProperties());

            modifier.Assignments.Should().ContainEquivalentOf(new MemoryAssignment
            {
                Key = "2DAMEMORY1",
                Value = new ValueResolverForExistingCell { ColumnHeader = "NewColumn", RowLocator = new RowLocatorByRowIndex { Index = 1 } },
            }, config => config.IncludingAllRuntimeProperties());

            modifier.Assignments.Should().ContainEquivalentOf(new MemoryAssignment
            {
                Key = "2DAMEMORY2",
                Value = new ValueResolverForExistingCell { ColumnHeader = "NewColumn", RowLocator = new RowLocatorByRowHeader { RowHeader = "2" } },
            }, config => config.IncludingAllRuntimeProperties());
        }
    }
}
