using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryLIP;

public class LIPBinary
{
    public LIPBinaryFileHeader FileHeader { get; set; } = new();
    public List<LIPBinaryKeyFrame> KeyFrames { get; set; } = new();

    public LIPBinary()
    {

    }
    public LIPBinary(Stream stream)
    {
        var reader = new BinaryReader(stream);

        FileHeader = new LIPBinaryFileHeader(reader);

        for (int i = 0; i < FileHeader.KeyFrameCount; i++)
        {
            KeyFrames.Add(new LIPBinaryKeyFrame(reader));
        }
    }

    public void Write(Stream stream)
    {
        var writer = new BinaryWriter(stream);

        FileHeader.Write(writer);

        foreach (var keyFrame in KeyFrames)
        {
            keyFrame.Write(writer);
        }
    }

    public void Recalculate()
    {
        FileHeader.KeyFrameCount = KeyFrames.Count;
    }
}
