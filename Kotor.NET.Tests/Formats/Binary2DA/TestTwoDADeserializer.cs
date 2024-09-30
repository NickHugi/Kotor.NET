using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Formats.Binary2DA;
using Kotor.NET.Formats.Binary2DA.Serialisation;
using Kotor.NET.Resources.Kotor2DA;

namespace Kotor.NET.Tests.Formats.Binary2DA;

public class TestTwoDADeserializer
{
    [Fact]
    public void Deserialize()
    {
        using var stream = File.OpenRead(TestTwoDABinary.File1Filepath);
        var binary = new TwoDABinary(stream);
        var deserializer = new TwoDABinaryDeserializer(binary);
        var twoda = deserializer.Deserialize();

        using (var scope = new AssertionScope())
        {
            twoda.GetColumns().Should().HaveCount(4);
            twoda.GetRows().Should().HaveCount(3);
        }

        using (var scope = new AssertionScope())
        {
            twoda.GetRow(0).RowHeader.Should().Be("0");
            twoda.GetRow(1).RowHeader.Should().Be("1");
            twoda.GetRow(2).RowHeader.Should().Be("2");

            twoda.GetRow(0).GetCell("label").AsString().Should().Be("DEFAULT");
            twoda.GetRow(0).GetCell("aistate").AsString().Should().Be("0");
            twoda.GetRow(2).GetCell("name_strref").AsString().Should().Be("1121");
        }
    }
}
