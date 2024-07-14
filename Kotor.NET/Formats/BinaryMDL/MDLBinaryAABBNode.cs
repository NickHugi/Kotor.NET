using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryAABBNode
{
    public static readonly int SIZE = 40;

    public BoundingBox BoundingBox { get; set; } = new();
    public int LeftChildOffset { get; set; }
    public int RightChildOffset { get; set; }
    public int FaceIndex { get; set; }
    public int MostSiginificantPlane { get; set; }

    public MDLBinaryAABBNode? LeftNode { get; set; }
    public MDLBinaryAABBNode? RightNode { get; set; }

    public MDLBinaryAABBNode()
    {
    }
    public MDLBinaryAABBNode(MDLBinaryReader reader)
    {
        BoundingBox = reader.ReadBoundingBox();
        LeftChildOffset = reader.ReadInt32();
        RightChildOffset = reader.ReadInt32();
        FaceIndex = reader.ReadInt32();
        MostSiginificantPlane = reader.ReadInt32();

        if (LeftChildOffset > 0)
        {
            reader.SetStreamPosition(LeftChildOffset);
            LeftNode = new(reader);
        }
        if (RightChildOffset > 0)
        {
            reader.SetStreamPosition(RightChildOffset);
            RightNode = new(reader);
        }
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(BoundingBox);
        writer.Write(LeftChildOffset);
        writer.Write(RightChildOffset);
        writer.Write(FaceIndex);
        writer.Write(MostSiginificantPlane);

        if (LeftNode is not null)
        {
            writer.SetStreamPosition(LeftChildOffset);
            LeftNode.Write(writer);
        }
        if (RightNode is not null)
        {
            writer.SetStreamPosition(RightChildOffset);
            RightNode.Write(writer);
        }
    }
}
