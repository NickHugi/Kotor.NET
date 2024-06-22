using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryLIP;


public class LIPBinaryKeyFrame
{
    public static readonly int SIZE = 8;

    public float Time { get; set; }
    public byte Shape { get; set; }

    public LIPBinaryKeyFrame()
    {

    }
    public LIPBinaryKeyFrame(BinaryReader reader)
    {
        Time = reader.ReadSingle();
        Shape = reader.ReadByte();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Time);
        writer.Write(Shape);
    }
}
