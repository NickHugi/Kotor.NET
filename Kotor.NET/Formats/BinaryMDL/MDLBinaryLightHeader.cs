using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryLightHeader
{
    public static readonly int SIZE = 92;

    public int OffsetToUnknownArray { get; set; }
    public int UnknownArrayCount { get; set; }
    public int UnknownArrayCount2 { get; set; }
    public int OffsetToFlareSizeArray { get; set; }
    public int FlareSizeArrayCount { get; set; }
    public int FlareSizeArrayCount2 { get; set; }
    public int OffsetToFlarePositionArray { get; set; }
    public int FlarePositionArrayCount { get; set; }
    public int FlarePositionArrayCount2 { get; set; }
    public int OffsetToFlareColorShiftArray { get; set; }
    public int FlareColorShiftArrayCount { get; set; }
    public int FlareColorShiftArrayCount2 { get; set; }
    public int OffsetToFlareTextureNameOffsetsArray { get; set; }
    public int FlareTextureNameCount { get; set; }
    public int FlareTextureNameCount2 { get; set; }
    public float FlareRadius { get; set; }
    public uint LightPriority { get; set; }
    public uint AmbientOnly { get; set; }
    public uint DynamicType { get; set; }
    public uint AffectDynamic { get; set; }
    public uint Shadow { get; set; }
    public uint Flare { get; set; }
    public uint FadingLight { get; set; }

    public MDLBinaryLightHeader()
    {
    }
    public MDLBinaryLightHeader(MDLBinaryReader reader)
    {
        FlareRadius = reader.ReadSingle();
        OffsetToUnknownArray = reader.ReadInt32();
        UnknownArrayCount = reader.ReadInt32();
        UnknownArrayCount2 = reader.ReadInt32();
        OffsetToFlareSizeArray = reader.ReadInt32();
        FlareSizeArrayCount = reader.ReadInt32();
        FlareSizeArrayCount2 = reader.ReadInt32();
        OffsetToFlarePositionArray = reader.ReadInt32();
        FlarePositionArrayCount = reader.ReadInt32();
        FlarePositionArrayCount2 = reader.ReadInt32();
        OffsetToFlareColorShiftArray = reader.ReadInt32();
        FlareColorShiftArrayCount = reader.ReadInt32();
        FlareColorShiftArrayCount2 = reader.ReadInt32();
        OffsetToFlareTextureNameOffsetsArray = reader.ReadInt32();
        FlareTextureNameCount = reader.ReadInt32();
        FlareTextureNameCount2 = reader.ReadInt32();
        LightPriority = reader.ReadUInt32();
        AmbientOnly = reader.ReadUInt32();
        DynamicType = reader.ReadUInt32();
        AffectDynamic = reader.ReadUInt32();
        Shadow = reader.ReadUInt32();
        Flare = reader.ReadUInt32();
        FadingLight = reader.ReadUInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(FlareRadius);
        writer.Write(OffsetToUnknownArray);
        writer.Write(UnknownArrayCount);
        writer.Write(UnknownArrayCount2);
        writer.Write(OffsetToFlareSizeArray);
        writer.Write(FlareSizeArrayCount);
        writer.Write(FlareSizeArrayCount2);
        writer.Write(OffsetToFlarePositionArray);
        writer.Write(FlarePositionArrayCount);
        writer.Write(FlarePositionArrayCount2);
        writer.Write(OffsetToFlareColorShiftArray);
        writer.Write(FlareColorShiftArrayCount);
        writer.Write(FlareColorShiftArrayCount2);
        writer.Write(OffsetToFlareTextureNameOffsetsArray);
        writer.Write(FlareTextureNameCount);
        writer.Write(FlareTextureNameCount2);
        writer.Write(LightPriority);
        writer.Write(AmbientOnly);
        writer.Write(DynamicType);
        writer.Write(AffectDynamic);
        writer.Write(Shadow);
        writer.Write(Flare);
        writer.Write(FadingLight);
    }
}
