using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.For2DA;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.Kotor2DA.Builder;

namespace Kotor.NET.Patcher.Tests.Modifiers.For2DA;

public class AddColumn2DAModifierTest
{
    private static TwoDA Generate2DA()
    {
        return new TwoDABuilder()
            .AddColumns("Column0", "Column1", "Column2")
            .AddRow("Row0")
                .SetCell("Column0", "a")
                .SetCell("Column1", "b")
                .SetCell("Column2", "c")
                .Finish()
            .AddRow("Row1")
                .SetCell("Column0", "d")
                .SetCell("Column1", "e")
                .SetCell("Column2", "f")
                .Finish()
            .AddRow("Row2")
                .SetCell("Column0", "g")
                .SetCell("Column1", "h")
                .SetCell("Column2", "i")
                .Finish()
            .Build();
    } 

    [Fact]
    public void Apply()
    {
        var twoda = Generate2DA();

        var modifier = new AddColumn2DAModifier
        {
            ColumnHeader = "Column3",
            DefaultValue = "default",
            Assignments =
            [
                new ColumnCellAssignment { ColumnHeader = "Column3", RowLocator = new RowLocatorByRowIndex { Index = 1 }, Value = new ValueResolverForConstant { Value = "constant" } },
                new MemoryAssignment { Key = "2DAMEMORY0", Value = new ValueResolverForExistingCell { ColumnHeader = "Column3", RowLocator = new RowLocatorByRowIndex { Index = 1 } } },
            ]
        };

        var logger = new PatcherLogger();
        var memory = new PatcherMemory();
        memory.Set("2DAMEMORY0", "memory");

        modifier.Apply(twoda, memory, logger);

        using (var scope = new AssertionScope())
        {
            twoda.GetRow(0).GetCell("Column0").AsString().Should().Be("a");
            twoda.GetRow(0).GetCell("Column1").AsString().Should().Be("b");
            twoda.GetRow(0).GetCell("Column2").AsString().Should().Be("c");
            twoda.GetRow(0).GetCell("Column3").AsString().Should().Be("default");

            twoda.GetRow(1).GetCell("Column0").AsString().Should().Be("d");
            twoda.GetRow(1).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(1).GetCell("Column2").AsString().Should().Be("f");
            twoda.GetRow(1).GetCell("Column3").AsString().Should().Be("constant");

            twoda.GetRow(2).GetCell("Column0").AsString().Should().Be("g");
            twoda.GetRow(2).GetCell("Column1").AsString().Should().Be("h");
            twoda.GetRow(2).GetCell("Column2").AsString().Should().Be("i");
            twoda.GetRow(2).GetCell("Column3").AsString().Should().Be("default");

            memory.Get("2DAMEMORY0").Should().Be("constant");
        }
    }
}
