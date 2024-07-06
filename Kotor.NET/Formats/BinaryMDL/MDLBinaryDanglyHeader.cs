using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryDanglyHeader
{
    public static readonly int SIZE = 28;

    public int OffsetToContraintArray { get; set; }
    public int ContraintArrayCount { get; set; }
    public int ContraintArrayCount2 { get; set; }
    public float Displacement { get; set; }
    public float Tightness { get; set; }
    public float Period { get; set; }
    public int OffsetToDataArray { get; set; }

    public MDLBinaryDanglyHeader()
    {
    }
    public MDLBinaryDanglyHeader(MDLBinaryReader reader)
    {
        OffsetToContraintArray = reader.ReadInt32();
        ContraintArrayCount = reader.ReadInt32();
        ContraintArrayCount2 = reader.ReadInt32();
        Displacement = reader.ReadSingle();
        Tightness = reader.ReadSingle();
        Period = reader.ReadSingle();
        OffsetToDataArray = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(OffsetToContraintArray);
        writer.Write(ContraintArrayCount);
        writer.Write(ContraintArrayCount2);
        writer.Write(Displacement);
        writer.Write(Tightness);
        writer.Write(Period);
        writer.Write(OffsetToDataArray);
    }
}
