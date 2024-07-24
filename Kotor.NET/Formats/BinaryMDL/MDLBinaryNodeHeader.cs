using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryNodeHeader
{
    public static readonly int SIZE = 80;

    public ushort NodeType { get; set; }
    public ushort NameIndex { get; set; }
    public ushort NodeIndex { get; set; }
    public ushort Padding { get; set; }
    public int OffsetToRootNode { get; set; }
    public int OffsetToParentNode { get; set; }
    public Vector3 Position { get; set; } = new();
    public Vector4 Rotation { get; set; } = new();
    public int OffsetToChildOffsetArray { get; set; }
    public int ChildArrayCount { get; set; }
    public int ChildArrayCount2 { get; set; }
    public int OffsetToControllerArray { get; set; }
    public int ControllerArrayCount { get; set; }
    public int ControllerArrayCount2 { get; set; }
    public int OffsetToControllerData { get; set; }
    public int ControllerDataCount { get; set; }
    public int ControllerDataCount2 { get; set; }

    public MDLBinaryNodeHeader()
    {
    }
    public MDLBinaryNodeHeader(MDLBinaryReader reader)
    {
        NodeType = reader.ReadUInt16();
        NameIndex = reader.ReadUInt16();
        NodeIndex = reader.ReadUInt16();
        Padding = reader.ReadUInt16();
        OffsetToRootNode = reader.ReadInt32();
        OffsetToParentNode = reader.ReadInt32();
        Position = reader.ReadVector3();
        Rotation = reader.ReadVector4();
        OffsetToChildOffsetArray = reader.ReadInt32();
        ChildArrayCount = reader.ReadInt32();
        ChildArrayCount2 = reader.ReadInt32();
        OffsetToControllerArray = reader.ReadInt32();
        ControllerArrayCount = reader.ReadInt32();
        ControllerArrayCount2 = reader.ReadInt32();
        OffsetToControllerData = reader.ReadInt32();
        ControllerDataCount = reader.ReadInt32();
        ControllerDataCount2 = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(NodeType);
        writer.Write(NameIndex);
        writer.Write(NodeIndex);
        writer.Write(Padding);
        writer.Write(OffsetToRootNode);
        writer.Write(OffsetToParentNode);
        writer.Write(Position);
        writer.Write(Rotation);
        writer.Write(OffsetToChildOffsetArray);
        writer.Write(ChildArrayCount);
        writer.Write(ChildArrayCount2);
        writer.Write(OffsetToControllerArray);
        writer.Write(ControllerArrayCount);
        writer.Write(ControllerArrayCount2);
        writer.Write(OffsetToControllerData);
        writer.Write(ControllerDataCount);
        writer.Write(ControllerDataCount2);
    }
}
