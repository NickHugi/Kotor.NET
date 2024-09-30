using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.AsciiLYT;
using Kotor.NET.Formats.AsciiLYT.Serialisation;
using Kotor.NET.Formats.BinaryTLK;

namespace Kotor.NET.Tests.Formats.AsciiLYT;

public class TestLYTDeserializer
{
    public static readonly string File1Filepath = "Formats/AsciiLYT/file1.lyt";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);

        var ascii = new LYTAscii(stream);
        var deserializer = new LYTAsciiDeserializer(ascii);
        var lyt = deserializer.Deserialize();

        Assert.Equal(2, lyt.Rooms.Count());
        var room1 = lyt.Rooms.ElementAt(1);
        Assert.Equal("211TELa", room1.Model);
        Assert.Equal(1385.82, room1.Position.X, 2);
        Assert.Equal(473.243, room1.Position.Y, 3);
        Assert.Equal(0.0, room1.Position.Z);

        Assert.Single(lyt.Tracks);
        var track0 = lyt.Tracks.ElementAt(0);
        Assert.Equal("211TEL_MGT04", track0.Room);
        Assert.Equal(75.1835, track0.Position.X, 4);
        Assert.Equal(730.74, track0.Position.Y, 2);
        Assert.Equal(0.0, track0.Position.Z);

        Assert.Single(lyt.Obstacles);
        var obstacle0 = lyt.Obstacles.ElementAt(0);
        Assert.Equal("211TEL_MGC02", obstacle0.Room);
        Assert.Equal(19.0186, obstacle0.Position.X, 4);
        Assert.Equal(1139.38, obstacle0.Position.Y, 2);
        Assert.Equal(0.0, obstacle0.Position.Z);

        Assert.Single(lyt.DoorHooks);
        var doorhook0 = lyt.DoorHooks.ElementAt(0);
        Assert.Equal("221TELa", doorhook0.Room);
        Assert.Equal("Door_02", doorhook0.Door);
        Assert.Equal(-72.282, doorhook0.Position.X, 3);
        Assert.Equal(-0.692468, doorhook0.Position.Y, 4);
        Assert.Equal(0.0, doorhook0.Position.Z);
        Assert.Equal(1.0, doorhook0.Orientation.X);
        Assert.Equal(0.0, doorhook0.Orientation.Y);
        Assert.Equal(0.0, doorhook0.Orientation.Z);
        Assert.Equal(0.0, doorhook0.Orientation.W);
    }
}
