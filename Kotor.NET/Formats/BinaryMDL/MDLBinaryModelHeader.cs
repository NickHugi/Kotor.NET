using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryModelHeader
{
    public static readonly int SIZE = MDLBinaryGeometryHeader.SIZE + 116;

    public MDLBinaryGeometryHeader GeometryHeader { get; set; } = new();
    public Byte ModelType { get; set; }
    public Byte Unknown1 { get; set; }
    public Byte Padding1 { get; set; }
    public Byte Fog { get; set; }
    public Int32 ChildModelCount { get; set; }
    public Int32 AnimationOffsetArrayOffset { get; set; }
    public Int32 AnimationCount { get; set; }
    public Int32 AnimationCount2 { get; set; }
    public Int32 Unknown2 { get; set; }
    public Vector3 BoundingBoxMin { get; set; } = new();
    public Vector3 BoundingBoxMax { get; set; } = new();
    public Single Radius { get; set; }
    public Single AnimationScale { get; set; }
    public String SupermodelName { get; set; } = "";
    public Int32 OffsetToRootNode { get; set; }
    public Int32 Unused1 { get; set; }
    public Int32 MDXSize { get; set; }
    public Int32 MDXOffset { get; set; }
    public Int32 OffsetToNameOffsetArray { get; set; }
    public Int32 NamesArrayCount { get; set; }
    public Int32 NamesArrayCount2 { get; set; }

    public MDLBinaryModelHeader()
    {
    }
    public MDLBinaryModelHeader(MDLBinaryReader reader)
    {
        GeometryHeader = new(reader);
        ModelType = reader.ReadByte();
        Unknown1 = reader.ReadByte();
        Padding1 = reader.ReadByte();
        Fog = reader.ReadByte();
        ChildModelCount = reader.ReadInt32();
        AnimationOffsetArrayOffset = reader.ReadInt32();
        AnimationCount = reader.ReadInt32();
        AnimationCount2 = reader.ReadInt32();
        Unknown2 = reader.ReadInt32();
        BoundingBoxMin = reader.ReadVector3();
        BoundingBoxMax = reader.ReadVector3();
        Radius = reader.ReadSingle();
        AnimationScale = reader.ReadSingle();
        SupermodelName = reader.ReadString(32);
        OffsetToRootNode = reader.ReadInt32();
        Unused1 = reader.ReadInt32();
        MDXSize = reader.ReadInt32();
        MDXOffset = reader.ReadInt32();
        OffsetToNameOffsetArray = reader.ReadInt32();
        NamesArrayCount = reader.ReadInt32();
        NamesArrayCount2 = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        GeometryHeader.Write(writer);
        writer.Write(ModelType);
        writer.Write(Unknown1);
        writer.Write(Padding1);
        writer.Write(Fog);
        writer.Write(ChildModelCount);
        writer.Write(AnimationOffsetArrayOffset);
        writer.Write(AnimationCount);
        writer.Write(AnimationCount2);
        writer.Write(Unknown2);
        writer.Write(BoundingBoxMin);
        writer.Write(BoundingBoxMax);
        writer.Write(Radius);
        writer.Write(AnimationScale);
        writer.Write(SupermodelName.Resize(32), 0);
        writer.Write(OffsetToRootNode);
        writer.Write(Unused1);
        writer.Write(MDXSize);
        writer.Write(MDXOffset);
        writer.Write(OffsetToNameOffsetArray);
        writer.Write(NamesArrayCount);
        writer.Write(NamesArrayCount2);
    }
}
