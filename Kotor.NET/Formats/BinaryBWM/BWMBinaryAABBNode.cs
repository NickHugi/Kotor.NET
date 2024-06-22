using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryBWM;

public class BWMBinaryAABBNode
{
    public static readonly int SIZE = 44;

    public Vector3 BoundingBoxMin { get; set; } = new Vector3();
    public Vector3 BoundingBoxMax { get; set; } = new Vector3();
    public int FaceIndex { get; set; }
    public int UnknownAlways4 { get; set; } = 4;
    public int MostSignificantPlane { get; set; }
    public int LeftChildIndex { get; set; }
    public int RightChildIndex { get; set; }

    public BWMBinaryAABBNode()
    {

    }

    public BWMBinaryAABBNode(BinaryReader reader)
    {
        BoundingBoxMin = reader.ReadVector3();
        BoundingBoxMax = reader.ReadVector3();
        FaceIndex = reader.ReadInt32();
        UnknownAlways4 = reader.ReadInt32();
        MostSignificantPlane = reader.ReadInt32();
        LeftChildIndex = reader.ReadInt32();
        RightChildIndex = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(BoundingBoxMin);
        writer.Write(BoundingBoxMax);
        writer.Write(FaceIndex);
        writer.Write(UnknownAlways4);
        writer.Write(MostSignificantPlane);
        writer.Write(LeftChildIndex);
        writer.Write(RightChildIndex);
    }
}
