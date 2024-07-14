using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryAnimationHeader
{
    public static readonly int SIZE = MDLBinaryGeometryHeader.SIZE + 56;

    public MDLBinaryGeometryHeader GeometryHeader { get; set; } = new MDLBinaryGeometryHeader();
    public float AnimationLength { get; set; }
    public float TransitionTime { get; set; }
    public string AnimationRoot { get; set; } = "";
    public int OffsetToEventArray { get; set; }
    public int EventCount { get; set; }
    public int EventCount2 { get; set; }
    public int Unknown { get; set; }

    public MDLBinaryAnimationHeader()
    {
    }
    public MDLBinaryAnimationHeader(MDLBinaryReader reader)
    {
        GeometryHeader = new MDLBinaryGeometryHeader(reader);
        AnimationLength = reader.ReadSingle();
        TransitionTime = reader.ReadSingle();
        AnimationRoot = reader.ReadString(32);
        OffsetToEventArray = reader.ReadInt32();
        EventCount = reader.ReadInt32();
        EventCount2 = reader.ReadInt32();
        Unknown = reader.ReadInt32();
    }

    public void Write(MDLBinaryWriter writer)
    {
        GeometryHeader.Write(writer);
        writer.Write(AnimationLength);
        writer.Write(TransitionTime);
        writer.Write(AnimationRoot.Resize(32), 0);
        writer.Write(OffsetToEventArray);
        writer.Write(EventCount);
        writer.Write(EventCount2);
        writer.Write(Unknown);
    }
}
