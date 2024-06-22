using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryBWM;

public class BWMBinaryAdjacency
{
    public static readonly int SIZE = 12;

    public int Index1 { get; set; }
    public int Index2 { get; set; }
    public int Index3 { get; set; }

    public BWMBinaryAdjacency()
    {
    }

    public BWMBinaryAdjacency(BinaryReader reader)
    {
        Index1 = reader.ReadInt32();
        Index2 = reader.ReadInt32();
        Index3 = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Index1);
        writer.Write(Index2);
        writer.Write(Index3);
    }
}
