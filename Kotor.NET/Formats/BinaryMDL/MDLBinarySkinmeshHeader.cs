using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinarySkinmeshHeader
{
    public static readonly int SIZE = 96;

    public int Unknown0 { get; set; }
    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }
    public int MDXWeightValueStride { get; set; }
    public int MDXWeightIndexStride { get; set; }
    public int BonemapOffset { get; set; }
    public int BonemapCount { get; set; }
    public int QBonesOffset { get; set; }
    public int QBonesCount { get; set; }
    public int QBonesCount2 { get; set; }
    public int TBonesOffset { get; set; }
    public int TBonesCount { get; set; }
    public int TBonesCount2 { get; set; }
    public int Array8Offset { get; set; }
    public int Array8Count { get; set; }
    public int Array8Count2 { get; set; }
    public ushort BoneIndex1 { get; set; }
    public ushort BoneIndex2 { get; set; }
    public ushort BoneIndex3 { get; set; }
    public ushort BoneIndex4 { get; set; }
    public ushort BoneIndex5 { get; set; }
    public ushort BoneIndex6 { get; set; }
    public ushort BoneIndex7 { get; set; }
    public ushort BoneIndex8 { get; set; }
    public ushort BoneIndex9 { get; set; }
    public ushort BoneIndex10 { get; set; }
    public ushort BoneIndex11 { get; set; }
    public ushort BoneIndex12 { get; set; }
    public ushort BoneIndex13 { get; set; }
    public ushort BoneIndex14 { get; set; }
    public ushort BoneIndex15 { get; set; }
    public ushort BoneIndex16 { get; set; }

    public MDLBinarySkinmeshHeader()
    {
    }
    public MDLBinarySkinmeshHeader(MDLBinaryReader reader)
    {
        Unknown0 = reader.ReadInt32();
        Unknown1 = reader.ReadInt32();
        Unknown2 = reader.ReadInt32();
        MDXWeightValueStride = reader.ReadInt32();
        MDXWeightIndexStride = reader.ReadInt32();
        BonemapOffset = reader.ReadInt32();
        BonemapCount = reader.ReadInt32();
        QBonesOffset = reader.ReadInt32();
        QBonesCount = reader.ReadInt32();
        QBonesCount2 = reader.ReadInt32();
        TBonesOffset = reader.ReadInt32();
        TBonesCount = reader.ReadInt32();
        TBonesCount2 = reader.ReadInt32();
        Array8Offset = reader.ReadInt32();
        Array8Count = reader.ReadInt32();
        Array8Count2 = reader.ReadInt32();
        BoneIndex1 = reader.ReadUInt16();
        BoneIndex2 = reader.ReadUInt16();
        BoneIndex3 = reader.ReadUInt16();
        BoneIndex4 = reader.ReadUInt16();
        BoneIndex5 = reader.ReadUInt16();
        BoneIndex6 = reader.ReadUInt16();
        BoneIndex7 = reader.ReadUInt16();
        BoneIndex8 = reader.ReadUInt16();
        BoneIndex9 = reader.ReadUInt16();
        BoneIndex10 = reader.ReadUInt16();
        BoneIndex11 = reader.ReadUInt16();
        BoneIndex12 = reader.ReadUInt16();
        BoneIndex13 = reader.ReadUInt16();
        BoneIndex14 = reader.ReadUInt16();
        BoneIndex15 = reader.ReadUInt16();
        BoneIndex16 = reader.ReadUInt16();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(Unknown0);
        writer.Write(Unknown1);
        writer.Write(Unknown2);
        writer.Write(MDXWeightValueStride);
        writer.Write(MDXWeightIndexStride);
        writer.Write(BonemapOffset);
        writer.Write(BonemapCount);
        writer.Write(QBonesOffset);
        writer.Write(QBonesCount);
        writer.Write(QBonesCount2);
        writer.Write(TBonesOffset);
        writer.Write(TBonesCount);
        writer.Write(TBonesCount2);
        writer.Write(Array8Offset);
        writer.Write(Array8Count);
        writer.Write(Array8Count2);
        writer.Write(BoneIndex1);
        writer.Write(BoneIndex2);
        writer.Write(BoneIndex3);
        writer.Write(BoneIndex4);
        writer.Write(BoneIndex5);
        writer.Write(BoneIndex6);
        writer.Write(BoneIndex7);
        writer.Write(BoneIndex8);
        writer.Write(BoneIndex9);
        writer.Write(BoneIndex10);
        writer.Write(BoneIndex11);
        writer.Write(BoneIndex12);
        writer.Write(BoneIndex13);
        writer.Write(BoneIndex14);
        writer.Write(BoneIndex15);
        writer.Write(BoneIndex16);
    }
}
