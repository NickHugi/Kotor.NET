using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryERF;

public class ERFBinaryResourceEntry
{
    public static readonly int SIZE = 8;

    public int Offset { get; set; }
    public int Size { get; set; }

    public ERFBinaryResourceEntry()
    {

    }
    public ERFBinaryResourceEntry(BinaryReader reader)
    {
        Offset = reader.ReadInt32();
        Size = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(Offset);
        writer.Write(Size);
    }
}
