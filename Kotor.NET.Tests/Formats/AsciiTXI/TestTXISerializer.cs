using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions;
using Kotor.NET.Formats.AsciiTXI;
using Kotor.NET.Formats.AsciiTXI.Serialisation;
using Kotor.NET.Resources.KotorTXI;

namespace Kotor.NET.Tests.Formats.AsciiTXI;

public class TestTXISerializer
{
    [Fact]
    public void Serialize()
    {
        var txi = new TXI();
        txi.Texture.EnableMipmaps = true;
        txi.Font.BaselineHeight = 1M;
        txi.Font.CaretIndent = 2M;
        txi.Font.Coords =
        [
            new() { LowerRightX = 1M, LowerRightY = 2M, UpperLeftX = 3M, UpperLeftY = 4M },
            new() { LowerRightX = 5M, LowerRightY = 6M, UpperLeftX = 7M, UpperLeftY = 8M },
        ];

        var serializer = new TXIAsciiSerializer(txi);
        var ascii = serializer.Serialize();

        using (var scope = new AssertionScope())
        {
            ascii.Fields.Should().HaveCount(5);

            ascii.Fields.Should().ContainEquivalentOf(new TXIAsciiField(TXIAsciiInstructions.Mipmap, ["1"]));
            ascii.Fields.Should().ContainEquivalentOf(new TXIAsciiField(TXIAsciiInstructions.BaselineHeight, ["1"]));
            ascii.Fields.Should().ContainEquivalentOf(new TXIAsciiField(TXIAsciiInstructions.CaretIndent, ["2"]));
            ascii.Fields.Should().ContainEquivalentOf(new TXIAsciiField(TXIAsciiInstructions.LowerRightCoords, ["2"], [["1", "2", "0"], ["5", "6", "0"]]));
            ascii.Fields.Should().ContainEquivalentOf(new TXIAsciiField(TXIAsciiInstructions.UpperLeftCoords, ["2"], [["3", "4", "0"], ["7", "8", "0"]]));
        }
    }
}
