using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Formats.Binary2DA;

namespace Kotor.NET.Tests.Formats.Binary2DA;

public class TestTwoDABinary
{
    public static readonly string File1Filepath = "Formats/Binary2DA/file1.2da";

    [Fact]
    public void Read_File1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new TwoDABinary(stream);

        using (var scope = new AssertionScope())
        {
            binary.ColumnHeaders.Should()
                .HaveCount(4)
                .And.Contain(["label", "name_strref", "description_strref", "aistate"]);

            binary.RowHeaders.Should()
                .HaveCount(3)
                .And.Contain(["0", "1", "2"]);

            binary.CellValues.Should()
                .HaveCount(12)
                .And.HaveElementAt(0, "DEFAULT")
                .And.HaveElementAt(3, "0")
                .And.HaveElementAt(4, "GRENADE");
        }
    }
}
