using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.AsciiLYT;
using Kotor.NET.Formats.BinaryTLK;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.AsciiLYT;

public class TestLYTAscii
{
    public static readonly string File1Filepath = "Formats/AsciiLYT/file1.lyt";

    private LYTAscii GetAsciiLYT(byte[] data)
    {
        return new LYTAscii(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var ascii = GetAsciiLYT(File.ReadAllBytes(File1Filepath));

        Assert.Equal(2, ascii.Layout.Rooms.Count);
        var room1 = ascii.Layout.Rooms.ElementAt(1);
        Assert.Equal("211TELa", room1.Model);
        Assert.Equal("1385.82", room1.PositionX);
        Assert.Equal("473.243", room1.PositionY);
        Assert.Equal("0.0", room1.PositionZ);

        Assert.Single(ascii.Layout.Tracks);
        var track0 = ascii.Layout.Tracks.ElementAt(0);
        Assert.Equal("211TEL_MGT04", track0.Node);
        Assert.Equal("75.1835", track0.PositionX);
        Assert.Equal("730.74", track0.PositionY);
        Assert.Equal("0.0", track0.PositionZ);

        Assert.Single(ascii.Layout.Obstacles);
        var obstacle0 = ascii.Layout.Obstacles.ElementAt(0);
        Assert.Equal("211TEL_MGC02", obstacle0.Node);
        Assert.Equal("19.0186", obstacle0.PositionX);
        Assert.Equal("1139.38", obstacle0.PositionY);
        Assert.Equal("0.0", obstacle0.PositionZ);

        Assert.Single(ascii.Layout.DoorHooks);
        var doorhook0 = ascii.Layout.DoorHooks.ElementAt(0);
        Assert.Equal("221TELa", doorhook0.Room);
        Assert.Equal("Door_02", doorhook0.Door);
        Assert.Equal("0", doorhook0.Unknown);
        Assert.Equal("-72.282", doorhook0.PositionX);
        Assert.Equal("-0.692468", doorhook0.PositionY);
        Assert.Equal("0.0", doorhook0.PositionZ);
        Assert.Equal("1.0", doorhook0.OrientationX);
        Assert.Equal("0.0", doorhook0.OrientationY);
        Assert.Equal("0.0", doorhook0.OrientationZ);
        Assert.Equal("0.0", doorhook0.OrientationW);
    }
}
