using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;

namespace Kotor.NET.Resources.KotorERF;

public class ERF : IEnumerable<ERFResource>
{
    internal List<ERFResource> _resources;

    public ERF()
    {
        _resources = new();
    }

    public static ERF FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static ERF FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static ERF FromStream(Stream stream)
    {
        var binary = new ERFBinary(stream);
        var deserializer = new ERFBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public IEnumerator<ERFResource> GetEnumerator() => _resources.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _resources.GetEnumerator();

    public ERFResource Add(ResRef resref, ResourceType type, byte[] data)
    {
        var resource = new ERFResource(this, resref, type, data);
        _resources.Add(resource);
        return resource;
    }
}
