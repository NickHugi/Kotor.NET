using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions;
using Kotor.NET.Formats.AsciiVIS;
using Kotor.NET.Formats.AsciiVIS.Serialisation;
using Kotor.NET.Formats.BinaryTLK;

namespace Kotor.NET.Tests.Formats.AsciiVIS;

public class TestVISDeserializer
{
    public static readonly string File1Filepath = "Formats/AsciiVIS/file1.vis";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);

        var ascii = new VISAscii(stream);
        var deserializer = new VISAsciiDeserializer(ascii);
        var vis = deserializer.Deserialize();

        Assert.Equal(4, ascii.Rooms.Count);

        using (new AssertionScope())
        {
            vis.Count().Should().Be(4);
            vis.Exists("221tela").Should().BeTrue();
            vis.Exists("221telb").Should().BeTrue();
            vis.Exists("221telc").Should().BeTrue();
            vis.Exists("221teld").Should().BeTrue();
        }

        using (new AssertionScope())
        {
            vis.Get("221tela").CanSee().Count().Should().Be(0);
        }

        using (new AssertionScope())
        {
            vis.Get("221telb").CanSee().Count().Should().Be(2);
            vis.Get("221telb").CanSee().Contains("221telc");
            vis.Get("221telb").CanSee().Contains("221teld");
        }

        using (new AssertionScope())
        {
            vis.Get("221teld").CanSee().Count().Should().Be(1);
            vis.Get("221teld").CanSee().Contains("221telb");
        }
    }
}
