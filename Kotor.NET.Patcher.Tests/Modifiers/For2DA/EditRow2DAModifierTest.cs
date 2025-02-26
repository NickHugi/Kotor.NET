using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Patcher.Modifiers;
using Kotor.NET.Patcher.Modifiers.For2DA;
using Kotor.NET.Patcher.Modifiers.For2DA.Assignments;
using Kotor.NET.Patcher.Modifiers.For2DA.CellValues;
using Kotor.NET.Patcher.Modifiers.For2DA.RowLocators;
using Kotor.NET.Resources.Kotor2DA.Builder;

namespace Kotor.NET.Patcher.Tests.Modifiers.For2DA;

public class EditRow2DAModifierTest
{
    [Fact]
    public void Apply()
    {
        var twoda = new TwoDABuilder()
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

        var modifier = new EditRow2DAModifier
        {
            RowHeader = new CellValueConstant { Value = "EditedHeader " },
            TargetRowLocator = new RowLocatorByRowIndex { Index = 1 },
            Assignments =
            [
                new RowCellAssignment { ColumnHeader = "Column0", Value = new CellValueConstant { Value = "edited cell 1" } },
                new RowCellAssignment { ColumnHeader = "Column2", Value = new CellValueConstant { Value = "edited cell 2" } },
            ]
        };

        var memory = new Memory2DA();

        modifier.Apply(twoda, memory, null, null);
    }
}
