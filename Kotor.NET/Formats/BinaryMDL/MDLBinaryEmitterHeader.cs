using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryEmitterHeader
{
    public static readonly int SIZE = 236;

    public float DeadSpace { get; set; }
    public float BlastRadius { get; set; }
    public float BlastLength { get; set; }
    public uint BranchCount { get; set; }
    public float ControlPointSmoothing { get; set; }
    public int XGrid { get; set; }
    public int YGrid { get; set; }
    public string Update { get; set; } = "";
    public string Render { get; set; } = "";
    public string Blend { get; set; } = "";
    public string Texture { get; set; } = "";
    public string ChunkName { get; set; } = "";
    public uint TwoSidedTexture { get; set; }
    public uint Loop { get; set; }
    public uint RenderOrder { get; set; }
    public string DepthTextureName { get; set; } = "";
    public uint Flags { get; set; }

    public MDLBinaryEmitterHeader()
    {
    }
    public MDLBinaryEmitterHeader(MDLBinaryReader reader)
    {
        DeadSpace = reader.ReadSingle();
        BlastRadius = reader.ReadSingle();
        BlastLength = reader.ReadSingle();
        BranchCount = reader.ReadUInt32();
        ControlPointSmoothing = reader.ReadSingle();
        XGrid = reader.ReadInt32();
        YGrid = reader.ReadInt32();
        Update = reader.ReadString(32);
        Render = reader.ReadString(32);
        Blend = reader.ReadString(32);
        Texture = reader.ReadString(32);
        ChunkName = reader.ReadString(32);
        TwoSidedTexture = reader.ReadUInt32();
        Loop = reader.ReadUInt32();
        RenderOrder = reader.ReadUInt32();
        DepthTextureName = reader.ReadString(32);
        Flags = reader.ReadUInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(DeadSpace);
        writer.Write(BlastRadius);
        writer.Write(BlastLength);
        writer.Write(BranchCount);
        writer.Write(ControlPointSmoothing);
        writer.Write(XGrid);
        writer.Write(YGrid);
        writer.Write(Update.Resize(32), 0);
        writer.Write(Render.Resize(32), 0);
        writer.Write(Blend.Resize(32), 0);
        writer.Write(Texture.Resize(32), 0);
        writer.Write(ChunkName.Resize(32), 0);
        writer.Write(TwoSidedTexture);
        writer.Write(Loop);
        writer.Write(RenderOrder);
        writer.Write(DepthTextureName.Resize(32), 0);
        writer.Write(Flags);
    }
}
