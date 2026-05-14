using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.AsciiLYT;
using Kotor.NET.Formats.AsciiLYT.Serialisation;

namespace Kotor.NET.Resources.KotorLYT;

public class LYT
{
    internal List<LYTRoom> _rooms;
    internal List<LYTDoorHook> _doorHooks;
    internal List<LYTTrack> _tracks;
    internal List<LYTObstacle> _obstacles;

    public LYT()
    {
        _rooms = new();
        _doorHooks = new();
        _tracks = new();
        _obstacles = new();
    }
    public static LYT FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static LYT FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static LYT FromStream(Stream stream)
    {
        var binary = new LYTAscii(stream);
        var deserializer = new LYTAsciiDeserializer(binary);
        return deserializer.Deserialize();
    }

    public static void ToFile(LYT lyt, string filepath)
    {
        using var stream = File.OpenWrite(filepath);
        ToStream(lyt, stream);
    }
    public static byte[] ToBytes(LYT lyt)
    {
        using var stream = new MemoryStream();
        ToStream(lyt, stream);
        return stream.ToArray();
    }
    public static void ToStream(LYT lyt, Stream stream)
    {
        var binary = new LYTAsciiSerializer(lyt).Serialize();
        binary.Write(stream);
    }

    public LYTRoomCollection Rooms => new(this);
    public LYTDoorHookCollection DoorHooks => new(this);
    public LYTTrackCollection Tracks => new(this);
    public LYTObstacleCollection Obstacles => new(this);
}
