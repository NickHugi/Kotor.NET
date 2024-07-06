using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Formats.BinaryMDL;

public class MDLBinaryTrimeshFace
{
    public static int SIZE = 32;

    public Vector3 Normal { get; set; } = new();
    public float PlaneCoefficient { get; set; }
    public int Material { get; set; }
    public short FaceAdjacency1 { get; set; }
    public short FaceAdjacency2 { get; set; }
    public short FaceAdjacency3 { get; set; }
    public short Vertex1 { get; set; }
    public short Vertex2 { get; set; }
    public short Vertex3 { get; set; }

    public MDLBinaryTrimeshFace()
    {
    }
    public MDLBinaryTrimeshFace(MDLBinaryReader reader)
    {
        Normal = reader.ReadVector3();
        PlaneCoefficient = reader.ReadSingle();
        Material = reader.ReadInt32();
        FaceAdjacency1 = reader.ReadInt16();
        FaceAdjacency2 = reader.ReadInt16();
        FaceAdjacency3 = reader.ReadInt16();
        Vertex1 = reader.ReadInt16();
        Vertex2 = reader.ReadInt16();
        Vertex3 = reader.ReadInt16();
    }

    public void Write(MDLBinaryWriter writer)
    {
        writer.Write(Normal);
        writer.Write(PlaneCoefficient);
        writer.Write(Material);
        writer.Write(FaceAdjacency1);
        writer.Write(FaceAdjacency2);
        writer.Write(FaceAdjacency3);
        writer.Write(Vertex1);
        writer.Write(Vertex2);
        writer.Write(Vertex3);
    }
}
