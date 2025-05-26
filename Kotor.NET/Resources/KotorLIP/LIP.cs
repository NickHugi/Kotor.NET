using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryLIP;
using Kotor.NET.Formats.BinaryLIP.Serialisation;

namespace Kotor.NET.Resources.KotorLIP;

public class LIP : IEnumerable<LIPKeyFrame>
{
    internal List<LIPKeyFrame> _frames;

    public LIP()
    {
        _frames = new();
    }
    public static LIP FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static LIP FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static LIP FromStream(Stream stream)
    {
        var binary = new LIPBinary(stream);
        var deserializer = new LIPBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public IEnumerator<LIPKeyFrame> GetEnumerator() => _frames.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _frames.GetEnumerator();

    public LIPKeyFrame Add(float time, LIPMouthShape shape)
    {
        var frame = new LIPKeyFrame(this, time, shape);
        var index = _frames.FindLastIndex(x => time < x.Time);

        if (index == -1)
            _frames.Add(frame);
        else
            _frames.Insert(index, frame);

        return frame;
    }
}
