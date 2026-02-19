using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryGeometryHeader
{
    public static readonly int SIZE = 80;

    public static readonly int K1_PC_MODEL_FP1 = 4273776;
    public static readonly int K1_PC_MODEL_FP2 = 4216096;
    public static readonly int K1_PC_ANIM_FP1 = 4273392;
    public static readonly int K1_PC_ANIM_FP2 = 4451552;

    public static readonly int K1_XBOX_MODEL_FP1 = 4254992;
    public static readonly int K1_XBOX_MODEL_FP2 = 4255008;
    public static readonly int K1_XBOX_ANIM_FP1 = 4253536;
    public static readonly int K1_XBOX_ANIM_FP2 = 4573360;

    public static readonly int K2_PC_MODEL_FP1 = 4285200;
    public static readonly int K2_PC_MODEL_FP2 = 4216320;
    public static readonly int K2_PC_ANIM_FP1 = 4284816;
    public static readonly int K2_PC_ANIM_FP2 = 4522928;

    public static readonly int K2_XBOX_MODEL_FP1 = 4285872;
    public static readonly int K2_XBOX_MODEL_FP2 = 4216016;
    public static readonly int K2_XBOX_ANIM_FP1 = 4285488;
    public static readonly int K2_XBOX_ANIM_FP2 = 4523088;

    public Int32 FunctionPointer1 { get; set; }
    public Int32 FunctionPointer2 { get; set; }
    public String Name { get; set; } = "";
    public Int32 RootNodeOffset { get; set; }
    public Int32 NodeCount { get; set; }
    public Int32 Unknown1 { get; set; }
    public Int32 Unknown2 { get; set; }
    public Int32 Unknown3 { get; set; }
    public Int32 Unknown4 { get; set; }
    public Int32 Unknown5 { get; set; }
    public Int32 Unknown6 { get; set; }
    public Int32 Unknown7 { get; set; }
    public Byte GeometryType { get; set; }
    public Byte Padding1 { get; set; }
    public Byte Padding2 { get; set; }
    public Byte Padding3 { get; set; }

    public MDLBinaryGeometryHeader()
    {
    }
    public MDLBinaryGeometryHeader(MDLBinaryReader reader)
    {
        FunctionPointer1 = reader.ReadInt32();
        FunctionPointer2 = reader.ReadInt32();
        Name = reader.ReadString(32);
        RootNodeOffset = reader.ReadInt32();
        NodeCount = reader.ReadInt32();
        Unknown1 = reader.ReadInt32();
        Unknown2 = reader.ReadInt32();
        Unknown3 = reader.ReadInt32();
        Unknown4 = reader.ReadInt32();
        Unknown5 = reader.ReadInt32();
        Unknown6 = reader.ReadInt32();
        Unknown7 = reader.ReadInt32();
        GeometryType = reader.ReadByte();
        Padding1 = reader.ReadByte();
        Padding2 = reader.ReadByte();
        Padding3 = reader.ReadByte();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(FunctionPointer1);
        writer.Write(FunctionPointer2);
        writer.Write(Name.Resize(32), 0);
        writer.Write(RootNodeOffset);
        writer.Write(NodeCount);
        writer.Write(Unknown1);
        writer.Write(Unknown2);
        writer.Write(Unknown3);
        writer.Write(Unknown4);
        writer.Write(Unknown5);
        writer.Write(Unknown6);
        writer.Write(Unknown7);
        writer.Write(GeometryType);
        writer.Write(Padding1);
        writer.Write(Padding2);
        writer.Write(Padding3);
    }

    public void SetFunctionPointers(GameEngine game, Platform platform, bool animated)
    {
        if (animated)
        {
            if (game == GameEngine.K1)
            {
                if (platform == Platform.Xbox)
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K1_XBOX_ANIM_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K1_XBOX_ANIM_FP2;
                }
                else
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K1_PC_ANIM_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K1_PC_ANIM_FP2;
                }
            }
            else
            {
                if (platform == Platform.Xbox)
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K2_XBOX_ANIM_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K2_XBOX_ANIM_FP2;
                }
                else
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K2_PC_ANIM_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K2_PC_ANIM_FP2;
                }
            }
        }
        else
        {
            if (game == GameEngine.K1)
            {
                if (platform == Platform.Xbox)
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K1_XBOX_MODEL_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K1_XBOX_MODEL_FP2;
                }
                else
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K1_PC_MODEL_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K1_PC_MODEL_FP2;
                }
            }
            else
            {
                if (platform == Platform.Xbox)
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K2_XBOX_MODEL_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K2_XBOX_MODEL_FP2;
                }
                else
                {
                    FunctionPointer1 = MDLBinaryGeometryHeader.K2_PC_MODEL_FP1;
                    FunctionPointer2 = MDLBinaryGeometryHeader.K2_PC_MODEL_FP2;
                }
            }
        }
    }
}
