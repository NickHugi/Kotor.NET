using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.For2DA;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.Modifiers;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA;
using Kotor.NET.Resources.Kotor2DA.Builder;

namespace Kotor.NET.Patcher.Tests.Modifiers.For2DA;

public class CopyRow2DAModifierTest
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
    public void Apply_FoundBlueprint_FoundOverride()
    {
        var twoda = Generate2DA();

        var modifier = new CopyRow2DAModifier
        {
            BlueprintRowLocator = new RowLocatorByRowIndex { Index = 1 },
            OverrideRowLocator = new RowLocatorByRowIndex { Index = 2 },
            Assignments =
            [
                new RowCellAssignment { ColumnHeader = "Column0", Value = new ValueResolverForConstant { Value = "edited" } },
            ]
        };

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        modifier.Apply(twoda, memory, logger);

        using (var scope = new AssertionScope())
        {
            twoda.GetRow(0).RowHeader.Should().Be("Row0");
            twoda.GetRow(0).GetCell("Column0").AsString().Should().Be("a");
            twoda.GetRow(0).GetCell("Column1").AsString().Should().Be("b");
            twoda.GetRow(0).GetCell("Column2").AsString().Should().Be("c");

            twoda.GetRow(1).RowHeader.Should().Be("Row1");
            twoda.GetRow(1).GetCell("Column0").AsString().Should().Be("d");
            twoda.GetRow(1).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(1).GetCell("Column2").AsString().Should().Be("f");

            twoda.GetRow(2).RowHeader.Should().Be("Row2");
            twoda.GetRow(2).GetCell("Column0").AsString().Should().Be("edited");
            twoda.GetRow(2).GetCell("Column1").AsString().Should().Be("h");
            twoda.GetRow(2).GetCell("Column2").AsString().Should().Be("i");
        }
    }

    [Fact]
    public void Apply_FoundBlueprint_DidNotFindOverride()
    {
        var twoda = Generate2DA();

        var modifier = new CopyRow2DAModifier
        {
            BlueprintRowLocator = new RowLocatorByRowIndex { Index = 1 },
            OverrideRowLocator = new RowLocatorByRowIndex { Index = 33 },
            Assignments =
            [
                new RowHeaderAssignment { Value = new ValueResolverForConstant { Value = "Row3" } },
                new RowCellAssignment { ColumnHeader = "Column0", Value = new ValueResolverForConstant { Value = "edited" } },
            ]
        };

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        modifier.Apply(twoda, memory, logger);

        using (var scope = new AssertionScope())
        {
            twoda.GetRow(0).RowHeader.Should().Be("Row0");
            twoda.GetRow(0).GetCell("Column0").AsString().Should().Be("a");
            twoda.GetRow(0).GetCell("Column1").AsString().Should().Be("b");
            twoda.GetRow(0).GetCell("Column2").AsString().Should().Be("c");

            twoda.GetRow(1).RowHeader.Should().Be("Row1");
            twoda.GetRow(1).GetCell("Column0").AsString().Should().Be("d");
            twoda.GetRow(1).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(1).GetCell("Column2").AsString().Should().Be("f");

            twoda.GetRow(2).RowHeader.Should().Be("Row2");
            twoda.GetRow(2).GetCell("Column0").AsString().Should().Be("g");
            twoda.GetRow(2).GetCell("Column1").AsString().Should().Be("h");
            twoda.GetRow(2).GetCell("Column2").AsString().Should().Be("i");

            twoda.GetRow(3).RowHeader.Should().Be("Row3");
            twoda.GetRow(3).GetCell("Column0").AsString().Should().Be("edited");
            twoda.GetRow(3).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(3).GetCell("Column2").AsString().Should().Be("f");
        }
    }

    [Fact]
    public void Apply_FoundBlueprint_NotLookingForOverride()
    {
        var twoda = Generate2DA();

        var modifier = new CopyRow2DAModifier
        {
            BlueprintRowLocator = new RowLocatorByRowIndex { Index = 1 },
            OverrideRowLocator = null,
            Assignments =
            [
                new RowHeaderAssignment { Value = new ValueResolverForConstant { Value = "Row3" } },
                new RowCellAssignment { ColumnHeader = "Column0", Value = new ValueResolverForConstant { Value = "edited" } },
            ]
        };

        var memory = new PatcherMemory();
        var logger = new PatcherLogger();
        modifier.Apply(twoda, memory, logger);

        using (var scope = new AssertionScope())
        {
            twoda.GetRow(0).RowHeader.Should().Be("Row0");
            twoda.GetRow(0).GetCell("Column0").AsString().Should().Be("a");
            twoda.GetRow(0).GetCell("Column1").AsString().Should().Be("b");
            twoda.GetRow(0).GetCell("Column2").AsString().Should().Be("c");

            twoda.GetRow(1).RowHeader.Should().Be("Row1");
            twoda.GetRow(1).GetCell("Column0").AsString().Should().Be("d");
            twoda.GetRow(1).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(1).GetCell("Column2").AsString().Should().Be("f");

            twoda.GetRow(2).RowHeader.Should().Be("Row2");
            twoda.GetRow(2).GetCell("Column0").AsString().Should().Be("g");
            twoda.GetRow(2).GetCell("Column1").AsString().Should().Be("h");
            twoda.GetRow(2).GetCell("Column2").AsString().Should().Be("i");

            twoda.GetRow(3).RowHeader.Should().Be("Row3");
            twoda.GetRow(3).GetCell("Column0").AsString().Should().Be("edited");
            twoda.GetRow(3).GetCell("Column1").AsString().Should().Be("e");
            twoda.GetRow(3).GetCell("Column2").AsString().Should().Be("f");
        }
    }
}
