using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryWalkmeshHeader
{
    public static readonly int SIZE = 4;

    public int OffsetToRootAABBNode { get; set; }

    public MDLBinaryWalkmeshHeader()
    {
    }
    public MDLBinaryWalkmeshHeader(MDLBinaryReader reader)
    {
        OffsetToRootAABBNode = reader.ReadInt32();
    }

    public void Writer(MDLBinaryWriter writer)
    {
        writer.Write(OffsetToRootAABBNode);
    }
}
