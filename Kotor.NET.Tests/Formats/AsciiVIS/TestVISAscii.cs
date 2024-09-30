using System.Reflection.PortableExecutable;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.AsciiVIS;
using Kotor.NET.Formats.BinaryTLK;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.AsciiVIS;

public class TestVISAscii
{
    public static readonly string File1Filepath = "Formats/AsciiVIS/file1.vis";

    private VISAscii GetAsciiVIS(byte[] data)
    {
        return new VISAscii(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var ascii = GetAsciiVIS(File.ReadAllBytes(File1Filepath));

        Assert.Equal(4, ascii.Rooms.Count);

        using (new AssertionScope())
        {
            var room0 = ascii.Rooms.ElementAt(0);
            room0.Model.Should().Be("221tela");
            room0.Visibility.Should().BeEmpty();
        }

        using (new AssertionScope())
        {
            var room1 = ascii.Rooms.ElementAt(1);
            room1.Model.Should().Be("221telb");
            room1.Visibility.Should().HaveCount(2);
            room1.Visibility.Should().Contain("221telc");
            room1.Visibility.Should().Contain("221teld");
        }

        using (new AssertionScope())
        {
            var room3 = ascii.Rooms.ElementAt(3);
            room3.Model.Should().Be("221teld");
            room3.Visibility.Should().HaveCount(1);
            room3.Visibility.Should().Contain("221telb");
        }
    }
}
