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

public class ParseAddRowTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [add_row_2da]
                ExclusiveColumn=Column0
                RowLabel=rowheader
                Column0=2DAMEMORY1
                2DAMEMORY2=RowIndex
            """);

        var parser = new ParseAddRow();
        var modifier = parser.Parse(ini["add_row_2da"]);

        using (new AssertionScope())
        {
            modifier.OverrideRowLocator.Should().BeEquivalentTo(new RowLocatorByColumn
            {
                ColumnHeader = "Column0",
                Value = new ValueResolverForPatcherMemory { Key = "2DAMEMORY1" },
            });

            modifier.Assignments.Should().HaveCount(3);

            modifier.Assignments.Should().ContainEquivalentOf(new RowHeaderAssignment
            {
                Value = new ValueResolverForConstant { Value = "rowheader" },
            }, config => config.IncludingAllRuntimeProperties());

            modifier.Assignments.Should().ContainEquivalentOf(new RowCellAssignment
            {
                ColumnHeader = "Column0",
                Value = new ValueResolverForPatcherMemory { Key = "2DAMEMORY1" },
            }, config => config.IncludingAllRuntimeProperties());

            modifier.Assignments.Should().ContainEquivalentOf(new MemoryAssignment
            {
                Key = "2DAMEMORY2",
                Value = new ValueResolverForTargetRowIndex()
            }, config => config.IncludingAllRuntimeProperties());
        }
    }
}
