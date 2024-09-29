using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Tests.Formats.Binary2DA;

public class TestTwoDASerializer
{
    [Fact]
    public void Write()
    {
        var twoda = new TwoDA();
        twoda.AddColumn("ColumnA");
        twoda.AddColumn("ColumnB");
        twoda.AddColumn("ColumnC");
        var row0 = twoda.AddRow("Row0");
        var row1 = twoda.AddRow("Row1");
        row0.GetCell("ColumnA").SetString("cell0");
        row0.GetCell("ColumnB").SetString("cell1");
        row0.GetCell("ColumnC").SetString("cell2");
        row1.GetCell("ColumnA").SetString("cell3");
        row1.GetCell("ColumnB").SetString("cell4");
        row1.GetCell("ColumnC").SetString("cell5");

        var serializer = new TwoDABinarySerializer(twoda);
        var binary = serializer.Serialize();

        using (var scope = new AssertionScope())
        {
            binary.ColumnHeaders.Should()
                .HaveCount(3)
                .And.Contain(["ColumnA", "ColumnB", "ColumnC"]);

            binary.RowHeaders.Should()
                .HaveCount(2)
                .And.HaveElementAt(0, "Row0")
                .And.HaveElementAt(1, "Row1");

            binary.CellValues.Should()
                .HaveCount(6)
                .And.HaveElementAt(0, "cell0")
                .And.HaveElementAt(2, "cell2")
                .And.HaveElementAt(3, "cell3");
        }
    }
}
