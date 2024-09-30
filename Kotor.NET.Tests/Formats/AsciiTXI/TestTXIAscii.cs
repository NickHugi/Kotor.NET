using System.Reflection.PortableExecutable;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.AsciiTXI;
using Kotor.NET.Formats.BinaryTLK;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.AsciiTXI;

public class TestTXIAscii
{
    public static readonly string NormalMapFilepath = "Formats/AsciiTXI/font.txi";

    private TXIAscii GetAsciiTXI(byte[] data)
    {
        return new TXIAscii(new MemoryStream(data));
    }
   
    [Fact]
    public void Read_FontFile()
    {
        var ascii = GetAsciiTXI(File.ReadAllBytes(NormalMapFilepath));

        using (new AssertionScope())
        {
            ascii.Fields.Should()
                .Contain(x => x.Instruction == "mipmap")
                .Which.Values.Should().ContainSingle()
                .And.HaveElementAt(0, "0");

            ascii.Fields.Should()
                .Contain(x => x.Instruction == "numchars")
                .Which.Values.Should().ContainSingle()
                .And.HaveElementAt(0, "256");

            ascii.Fields.Should()
                .Contain(x => x.Instruction == "upperleftcoords")
                .Which.Values.Should().ContainSingle()
                .And.HaveElementAt(0, "256");

            ascii.Fields.Should()
                .Contain(x => x.Instruction == "upperleftcoords")
                .Which.SubValues.Should().HaveCount(256)
                .And.Subject.ElementAt(255).Should().BeEquivalentTo(["0.316406", "0.480469", "0"]);
        }
    }
}
