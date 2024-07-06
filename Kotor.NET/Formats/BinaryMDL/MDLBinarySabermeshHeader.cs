using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinarySabermeshHeader
{
    public static readonly int SIZE = 20;

    public int OffsetToVertexArray { get; set; }
    public int OffsetToTexCoordArray { get; set; }
    public int OffsetToNormalArray { get; set; }
    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }

    public MDLBinarySabermeshHeader()
    {
    }
    public MDLBinarySabermeshHeader(MDLBinaryReader reader)
    {
        OffsetToVertexArray = reader.ReadInt32();
        OffsetToNormalArray = reader.ReadInt32();
        OffsetToTexCoordArray = reader.ReadInt32();
        Unknown1 = reader.ReadInt32();
        Unknown2 = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(OffsetToVertexArray);
        writer.Write(OffsetToNormalArray);
        writer.Write(OffsetToTexCoordArray);
        writer.Write(Unknown1);
        writer.Write(Unknown2);
    }
}
