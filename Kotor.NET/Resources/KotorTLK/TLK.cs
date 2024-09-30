using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryTLK;
using Kotor.NET.Formats.BinaryTLK.Serialisation;

namespace Kotor.NET.Resources.KotorTLK;

public class TLK : IEnumerable<TLKEntry>
{
    public int Language { get; set; }

    internal List<TLKEntry> _entries;

    public TLK()
    {
        _entries = new();
    }
    public static TLK FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static TLK FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static TLK FromStream(Stream stream)
    {
        var binary = new TLKBinary(stream);
        var deserializer = new TLKBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public IEnumerator<TLKEntry> GetEnumerator() => _entries.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _entries.GetEnumerator();

    public void Add(string text, ResRef sound)
    {
        _entries.Add(new TLKEntry(this, text, sound));
    }
}
