using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions.Execution;
using FluentAssertions;
using Kotor.NET.Formats.AsciiVIS;
using Kotor.NET.Formats.AsciiVIS.Serialisation;
using Kotor.NET.Resources.KotorVIS;

namespace Kotor.NET.Tests.Formats.AsciiVIS;

public class TestVISSerializer
{
    public static readonly string File1Filepath = "Formats/AsciiVIS/file1.vis";

    [Fact]
    public void Test_ReadFile1()
    {
        var vis = new VIS();
        vis.Add("Room1");
        vis.Add("Room2");
        vis.Add("Room3");
        vis.Get("Room1").SetCanBeSeenBy("Room2", true);
        vis.Get("Room2").SetCanBeSeenBy("Room1", true);

        using var stream = new MemoryStream();

        var serializer = new VISAsciiSerializer(vis);
        var ascii = serializer.Serialize();
        ascii.Write(stream);

        stream.Position = 0;
        var serialized = new VISAscii(stream);
        var deserializer = new VISAsciiDeserializer(serialized);
        var read = deserializer.Deserialize();

        using (new AssertionScope())
        {
            vis.Count().Should().Be(3);
            vis.Exists("Room1").Should().BeTrue();
            vis.Exists("Room2").Should().BeTrue();
            vis.Exists("Room3").Should().BeTrue();
        }

        using (new AssertionScope())
        {
            vis.Get("Room1").CanSee().Should().HaveCount(1);
            vis.Get("Room1").CanSee("Room2").Should().BeTrue();
        }

        using (new AssertionScope())
        {
            vis.Get("Room2").CanSee().Should().HaveCount(1);
            vis.Get("Room2").CanSee("Room1").Should().BeTrue();
        }

        using (new AssertionScope())
        {
            vis.Get("Room3").CanSee().Should().HaveCount(0);
        }
    }
}
