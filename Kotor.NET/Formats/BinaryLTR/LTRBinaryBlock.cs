using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryLTR;

public class LTRBinaryBlock
{
    public float[] Start { get; set; }
    public float[] Middle { get; set; }
    public float[] End { get; set; }

    public LTRBinaryBlock(byte size)
    {
        Start = new float[size];
        Middle = new float[size];
        End = new float[size];
    }
    public LTRBinaryBlock(BinaryReader reader, byte size)
    {
        Start = Enumerable.Range(0, size).Select(x => reader.ReadSingle()).ToArray();
        Middle = Enumerable.Range(0, size).Select(x => reader.ReadSingle()).ToArray();
        End = Enumerable.Range(0, size).Select(x => reader.ReadSingle()).ToArray();
    }

    public void Write(BinaryWriter writer)
    {
        Start.ToList().ForEach(x => writer.Write(x));
        Middle.ToList().ForEach(x => writer.Write(x));
        End.ToList().ForEach(x => writer.Write(x));
    }
}
