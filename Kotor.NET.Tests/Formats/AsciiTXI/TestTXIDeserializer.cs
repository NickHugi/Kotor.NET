using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions;
using Kotor.NET.Formats.AsciiTXI;
using Kotor.NET.Formats.AsciiTXI.Serialisation;
using Kotor.NET.Formats.BinaryTLK;
using Kotor.NET.Resources.KotorTXI;

namespace Kotor.NET.Tests.Formats.AsciiTXI;

public class TestTXIDeserializer
{
    public static readonly string File1Filepath = "Formats/AsciiTXI/font.txi";

    [Fact]
    public void Deserialize_FontFile()
    {
        using var stream = File.OpenRead(File1Filepath);

        var ascii = new TXIAscii(stream);
        var deserializer = new TXIAsciiDeserializer(ascii);
        var txi = deserializer.Deserialize();

        using (var scope = new AssertionScope())
        {
            txi.Texture.EnableMipmaps.Should().BeFalse();
            txi.Texture.UseLinearFiltering.Should().BeFalse();
            txi.Texture.CompressTexture.Should().BeFalse();
            txi.Texture.DownsampleMin.Should().Be(0);
            txi.Texture.DownsampleMax.Should().Be(0);

            txi.Font.NumberOfCharacters.Should().Be(256);
            txi.Font.FontHeight.Should().Be(0.17M);
            txi.Font.BaselineHeight.Should().Be(0.14M);
            txi.Font.TextureWidth.Should().Be(2.56M);
            txi.Font.SpacingBottom.Should().Be(0);
            txi.Font.SpacingRight.Should().Be(0);

            txi.Font.Coords.Should().HaveCount(256);
            txi.Font.Coords.ElementAt(0).Should().BeEquivalentTo(new TXIFontCoords
            {
                UpperLeftX = 0.000000M,
                UpperLeftY = 0.183594M,
                LowerRightX = 0.031250M,
                LowerRightY = 0.117188M,
            });
            txi.Font.Coords.ElementAt(255).Should().BeEquivalentTo(new TXIFontCoords
            {
                UpperLeftX = 0.316406M,
                UpperLeftY = 0.480469M,
                LowerRightX = 0.347656M,
                LowerRightY = 0.414063M,
            });
        }
    }
}
