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

public class ParseChangeRowTest
{
    [Fact]
    public void Parse()
    {
        var ini = new IniDataParser().Parse(
            """
                [change_row_2da]
                RowIndex=1
                Column1=hello
                Column2=world!
                2DAMEMORY1=RowIndex
            """);

        var parser = new ParseChangeRow();
        var modifier = parser.Parse(ini["change_row_2da"]);

        using (new AssertionScope())
        {
            modifier.RowHeader.Should().BeNull();

            modifier.TargetRowLocator
                .Should().BeOfType<RowLocatorByRowIndex>()
                .And.Subject.Should().Match<RowLocatorByRowIndex>(x => x.Index == 1);

            modifier.Assignments.Should().HaveCount(3);

            modifier.Assignments.Should().ContainEquivalentOf(new
            {
                ColumnHeader = "Column1",
                Value = new CellValueConstant() { Value = "hello" }
            }).Subject.Should().BeOfType<RowCellAssignment>();

            modifier.Assignments.Should().ContainEquivalentOf(new
            {
                ColumnHeader = "Column2",
                Value = new CellValueConstant() { Value = "world!" }
            }).Subject.Should().BeOfType<RowCellAssignment>();

            modifier.Assignments.Should().ContainEquivalentOf(new
            {
                Key = "2DAMEMORY1",
                Value = new CellValueRowIndex()
            }).Subject.Should().BeOfType<MemoryAssignment>();
        }
    }
}
