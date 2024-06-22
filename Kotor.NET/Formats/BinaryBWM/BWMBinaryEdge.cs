using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryBWM;

public class BWMBinaryEdge
{
    public static readonly int SIZE = 8;

    public int EdgeIndex { get; set; }
    public int Transition { get; set; }

    public BWMBinaryEdge()
    {
    }

    public BWMBinaryEdge(BinaryReader reader)
    {
        EdgeIndex = reader.ReadInt32();
        Transition = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(EdgeIndex);
        writer.Write(Transition);
    }
}
