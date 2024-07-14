using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryFileHeader
{
    public int Always0 { get; set; }
    public int MDLSize { get; set; }
    public int MDXSize { get; set; }

    public MDLBinaryFileHeader()
    {
    }
    public MDLBinaryFileHeader(MDLBinaryReader reader)
    {
        Always0 = reader.ReadInt32();
        MDLSize = reader.ReadInt32();
        MDXSize = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(Always0);
        writer.Write(MDLSize);
        writer.Write(MDXSize);
    }
}
