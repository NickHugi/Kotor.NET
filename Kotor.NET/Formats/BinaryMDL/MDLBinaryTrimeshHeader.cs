using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryTrimeshHeader
{
    public static readonly int K1_SIZE = 332;
    public static readonly int K2_SIZE = 340;

    public static readonly uint K1_NORMAL_FP1 = 4216656;
    public static readonly uint K1_NORMAL_FP2 = 4216672;
    public static readonly uint K1_SKIN_FP1 = 4216592;
    public static readonly uint K1_SKIN_FP2 = 4216608;
    public static readonly uint K1_DANGLY_FP1 = 4216640;
    public static readonly uint K1_DANGLY_FP2 = 4216624;

    public static readonly uint K2_NORMAL_FP1 = 4216880;
    public static readonly uint K2_NORMAL_FP2 = 4216896;
    public static readonly uint K2_SKIN_FP1 = 4216816;
    public static readonly uint K2_SKIN_FP2 = 4216832;
    public static readonly uint K2_DANGLY_FP1 = 4216848;
    public static readonly uint K2_DANGLY_FP2 = 4216864;

    public bool IsTSL => new List<uint> { K2_NORMAL_FP1, K2_SKIN_FP1, K2_DANGLY_FP1 }.Contains(FunctionPointer1);

    public uint FunctionPointer1 { get; set; }
    public uint FunctionPointer2 { get; set; }
    public int OffsetToFaceArray { get; set; }
    public int FaceArrayCount { get; set; }
    public int FaceArrayCount2 { get; set; }
    public Vector3 BoundingBoxMin { get; set; } = new();
    public Vector3 BoundingBoxMax { get; set; } = new();
    public float Radius { get; set; }
    public Vector3 AveragePoint { get; set; } = new();
    public Vector3 Diffuse { get; set; } = new();
    public Vector3 Ambient { get; set; } = new();
    public uint TransparencyHint { get; set; }
    public string Texture { get; set; } = "";
    public string Lightmap { get; set; } = "";
    public string Unused1 { get; set; } = "";
    public string Unused2 { get; set; } = "";
    public int OffsetToVertexIndicesCountArray { get; set; }
    public int VertexIndicesCountArrayCount { get; set; }
    public int VertexIndicesCountArrayCount2 { get; set; }
    public int OffsetToVertexIndicesOffsetArray { get; set; }
    public int VertexIndicesOffsetArrayCount { get; set; }
    public int VertexIndicesOffsetArrayCount2 { get; set; }
    public int OffsetToInvertedCounterArray { get; set; }
    public int InvertedCounterArrayCount { get; set; }
    public int InvertedCounterArrayCount2 { get; set; }
    public int Unknown1 { get; set; } = -1;
    public int Unknown2 { get; set; } = -1;
    public int Unknown3 { get; set; } = 0;
    public int Unknown4 { get; set; }
    public int Unknown8 { get; set; }
    public int AnimateUV { get; set; }
    public Vector2 UVDirection { get; set; } = new();
    public float UVSpeed { get; set; }
    public float UVJitterSpeed { get; set; }
    public int MDXDataSize { get; set; }
    public uint MDXDataBitmap { get; set; }
    public int MDXPositionStride { get; set; }
    public int MDXNormalStride { get; set; }
    public int MDXColourStride { get; set; }
    public int MDXTexture1Stride { get; set; }
    public int MDXTexture2Stride { get; set; }
    public int MDXTexture3Stride { get; set; }
    public int MDXTexture4Stride { get; set; }
    public int MDXTangent1Stride { get; set; }
    public int MDXTangent2Stride { get; set; }
    public int MDXTangent3Stride { get; set; }
    public int MDXTangent4Stride { get; set; }
    public ushort VertexCount { get; set; }
    public ushort TextureCount { get; set; }
    public byte HasLightmap { get; set; }
    public byte RotateTexture { get; set; }
    public byte BackgroundGeometry { get; set; }
    public byte HasShadow { get; set; }
    public byte Beaming { get; set; }
    public byte DoesRender { get; set; }
    public byte Unknown5 { get; set; }
    public byte Unknown6 { get; set; }
    public float TotalArea { get; set; }
    public uint Unknown7 { get; set; }

    public byte DirtEnabled { get; set; }
    public byte Padding0 { get; set; }
    public short DirtTexture { get; set; }
    public short DirtCoordinateSpace { get; set; }
    public byte HideInHolograms { get; set; }
    public byte Padding1 { get; set; }

    public int MDXOffsetToData { get; set; }
    public int OffsetToVertexArray { get; set; }

    public MDLBinaryTrimeshHeader()
    {

    }
    public MDLBinaryTrimeshHeader(MDLBinaryReader reader)
    {
        FunctionPointer1 = reader.ReadUInt32();
        FunctionPointer2 = reader.ReadUInt32();
        OffsetToFaceArray = reader.ReadInt32();
        FaceArrayCount = reader.ReadInt32();
        FaceArrayCount2 = reader.ReadInt32();
        BoundingBoxMin = reader.ReadVector3();
        BoundingBoxMax = reader.ReadVector3();
        Radius = reader.ReadSingle();
        AveragePoint = reader.ReadVector3();
        Diffuse = reader.ReadVector3();
        Ambient = reader.ReadVector3();
        TransparencyHint = reader.ReadUInt32();
        Texture = reader.ReadString(32);
        Lightmap = reader.ReadString(32);
        Unused1 = reader.ReadString(12);
        Unused2 = reader.ReadString(12);
        OffsetToVertexIndicesCountArray = reader.ReadInt32();
        VertexIndicesCountArrayCount = reader.ReadInt32();
        VertexIndicesCountArrayCount2 = reader.ReadInt32();
        OffsetToVertexIndicesOffsetArray = reader.ReadInt32();
        VertexIndicesOffsetArrayCount = reader.ReadInt32();
        VertexIndicesOffsetArrayCount2 = reader.ReadInt32();
        OffsetToInvertedCounterArray = reader.ReadInt32();
        InvertedCounterArrayCount = reader.ReadInt32();
        InvertedCounterArrayCount2 = reader.ReadInt32();
        Unknown1 = reader.ReadInt32();
        Unknown2 = reader.ReadInt32();
        Unknown3 = reader.ReadInt32();
        Unknown4 = reader.ReadInt32();
        Unknown8 = reader.ReadInt32();
        AnimateUV = reader.ReadInt32();
        UVDirection = reader.ReadVector2();
        UVSpeed = reader.ReadSingle();
        UVJitterSpeed = reader.ReadSingle();
        MDXDataSize = reader.ReadInt32();
        MDXDataBitmap = reader.ReadUInt32();
        MDXPositionStride = reader.ReadInt32();
        MDXNormalStride = reader.ReadInt32();
        MDXColourStride = reader.ReadInt32();
        MDXTexture1Stride = reader.ReadInt32();
        MDXTexture2Stride = reader.ReadInt32();
        MDXTexture3Stride = reader.ReadInt32();
        MDXTexture4Stride = reader.ReadInt32();
        MDXTangent1Stride = reader.ReadInt32();
        MDXTangent2Stride = reader.ReadInt32();
        MDXTangent3Stride = reader.ReadInt32();
        MDXTangent4Stride = reader.ReadInt32();
        VertexCount = reader.ReadUInt16();
        TextureCount = reader.ReadUInt16();
        HasLightmap = reader.ReadByte();
        RotateTexture = reader.ReadByte();
        BackgroundGeometry = reader.ReadByte();
        HasShadow = reader.ReadByte();
        Beaming = reader.ReadByte();
        DoesRender = reader.ReadByte();
        Unknown5 = reader.ReadByte();
        Unknown6 = reader.ReadByte();
        TotalArea = reader.ReadSingle();
        Unknown7 = reader.ReadUInt32();
        if (IsTSL)
        {
            DirtEnabled = reader.ReadByte();
            Padding0 = reader.ReadByte();
            DirtTexture = reader.ReadInt16();
            DirtCoordinateSpace = reader.ReadInt16();
            HideInHolograms = reader.ReadByte();
            Padding1 = reader.ReadByte();
        }
        MDXOffsetToData = reader.ReadInt32();
        OffsetToVertexArray = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(FunctionPointer1);
        writer.Write(FunctionPointer2);
        writer.Write(OffsetToFaceArray);
        writer.Write(FaceArrayCount);
        writer.Write(FaceArrayCount2);
        writer.Write(BoundingBoxMin);
        writer.Write(BoundingBoxMax);
        writer.Write(Radius);
        writer.Write(AveragePoint);
        writer.Write(Diffuse);
        writer.Write(Ambient);
        writer.Write(TransparencyHint);
        writer.Write(Texture.Resize(32), 0);
        writer.Write(Lightmap.Resize(32), 0);
        writer.Write(Unused1.Resize(12), 0);
        writer.Write(Unused2.Resize(12), 0);
        writer.Write(OffsetToVertexIndicesCountArray);
        writer.Write(VertexIndicesCountArrayCount);
        writer.Write(VertexIndicesCountArrayCount2);
        writer.Write(OffsetToVertexIndicesOffsetArray);
        writer.Write(VertexIndicesOffsetArrayCount);
        writer.Write(VertexIndicesOffsetArrayCount2);
        writer.Write(OffsetToInvertedCounterArray);
        writer.Write(InvertedCounterArrayCount);
        writer.Write(InvertedCounterArrayCount2);
        writer.Write(Unknown1);
        writer.Write(Unknown2);
        writer.Write(Unknown3);
        writer.Write(Unknown4);
        writer.Write(Unknown8);
        writer.Write(AnimateUV);
        writer.Write(UVDirection);
        writer.Write(UVSpeed);
        writer.Write(UVJitterSpeed);
        writer.Write(MDXDataSize);
        writer.Write(MDXDataBitmap);
        writer.Write(MDXPositionStride);
        writer.Write(MDXNormalStride);
        writer.Write(MDXColourStride);
        writer.Write(MDXTexture1Stride);
        writer.Write(MDXTexture2Stride);
        writer.Write(MDXTexture3Stride);
        writer.Write(MDXTexture4Stride);
        writer.Write(MDXTangent1Stride);
        writer.Write(MDXTangent2Stride);
        writer.Write(MDXTangent3Stride);
        writer.Write(MDXTangent4Stride);
        writer.Write(VertexCount);
        writer.Write(TextureCount);
        writer.Write(HasLightmap);
        writer.Write(RotateTexture);
        writer.Write(BackgroundGeometry);
        writer.Write(HasShadow);
        writer.Write(Beaming);
        writer.Write(DoesRender);
        writer.Write(Unknown5);
        writer.Write(Unknown6);
        writer.Write(TotalArea);
        writer.Write(Unknown7);
        if (IsTSL)
        {
            writer.Write(DirtEnabled);
            writer.Write(Padding0);
            writer.Write(DirtTexture);
            writer.Write(DirtCoordinateSpace);
            writer.Write(HideInHolograms);
            writer.Write(Padding1);
        }
        writer.Write(MDXOffsetToData);
        writer.Write(OffsetToVertexArray);
    }
}
