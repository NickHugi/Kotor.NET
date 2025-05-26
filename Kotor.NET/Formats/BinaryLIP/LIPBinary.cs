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
        try
        {
            var reader = new BinaryReader(stream);

            FileHeader = new LIPBinaryFileHeader(reader);

            for (int i = 0; i < FileHeader.KeyFrameCount; i++)
            {
                KeyFrames.Add(new LIPBinaryKeyFrame(reader));
            }
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to read the 2DA data.", ex);
        }
    }

    public void Write(Stream stream)
    {
        try
        {
            var writer = new BinaryWriter(stream);

            FileHeader.Write(writer);

            foreach (var keyFrame in KeyFrames)
            {
                keyFrame.Write(writer);
            }
        }
        catch (Exception ex)
        {
            throw new IOException("Failed to write the 2DA data.", ex);
        }
    }

    public void Recalculate()
    {
        FileHeader.KeyFrameCount = KeyFrames.Count;
    }
}
