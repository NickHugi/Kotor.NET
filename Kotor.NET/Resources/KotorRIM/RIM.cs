using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryRIM;
using Kotor.NET.Formats.BinaryRIM.Serialisation;

namespace Kotor.NET.Resources.KotorRIM;

public class RIM : IEnumerable<RIMResource>
{
    internal List<RIMResource> _resources;

    public RIM()
    {
        _resources = new();
    }
    public static RIM FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static RIM FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static RIM FromStream(Stream stream)
    {
        var binary = new RIMBinary(stream);
        var deserializer = new RIMBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public IEnumerator<RIMResource> GetEnumerator() => _resources.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _resources.GetEnumerator();

    public RIMResource Add(ResRef resref, ResourceType type, byte[] data)
    {
        var resource = new RIMResource(this, resref, type, data);
        _resources.Add(resource);
        return resource;
    }
}
