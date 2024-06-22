using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryBWM;

public class BWMBinaryFileHeader
{
    public static readonly int SIZE = 134;
    public static readonly IReadOnlyList<string> FILE_TYPES = new List<string>()
    {
        "BWM ",
    };
    public static readonly string FILE_VERSION = "V1.0";

    public string FileType { get; set; } = "BWM ";
    public string FileVersion { get; set; } = "V1.0";
    public int WalkmeshType { get; set; }
    public byte[] Reserved { get; set; } = new byte[48];
    public Vector3 Position { get; set; } = new();
    public int VertexCount { get; set; }
    public int OffsetToVertices { get; set; }
    public int FaceCount { get; set; }
    public int OffsetToFaceIndices { get; set; }
    public int OffsetToFaceMaterials { get; set; }
    public int OffsetToFaceNormals { get; set; }
    public int OffsetToFaceCoefficients { get; set; }
    public int AABBCount { get; set; }
    public int OffsetToAABBs { get; set; }
    public int Unknown { get; set; }
    public int AdjacencyCount { get; set; }
    public int OffsetToAdjacencies { get; set; }
    public int EdgeCount { get; set; }
    public int OffsetToEdges { get; set; }
    public int PerimeterCount { get; set; }
    public int OffsetToPerimeters { get; set; }

    public BWMBinaryFileHeader()
    {
    }

    public BWMBinaryFileHeader(BinaryReader reader)
    {
        FileType = reader.ReadString(4);
        FileVersion = reader.ReadString(4);
        WalkmeshType = reader.ReadInt32();
        Reserved = reader.ReadBytes(48);
        Position = reader.ReadVector3();
        VertexCount = reader.ReadInt32();
        OffsetToVertices = reader.ReadInt32();
        FaceCount = reader.ReadInt32();
        OffsetToFaceIndices = reader.ReadInt32();
        OffsetToFaceMaterials = reader.ReadInt32();
        OffsetToFaceNormals = reader.ReadInt32();
        OffsetToFaceCoefficients = reader.ReadInt32();
        AABBCount = reader.ReadInt32();
        OffsetToAABBs = reader.ReadInt32();
        Unknown = reader.ReadInt32();
        AdjacencyCount = reader.ReadInt32();
        OffsetToAdjacencies = reader.ReadInt32();
        EdgeCount = reader.ReadInt32();
        OffsetToEdges = reader.ReadInt32();
        PerimeterCount = reader.ReadInt32();
        OffsetToPerimeters = reader.ReadInt32();
    }

    public void Write(BinaryWriter writer)
    {
        writer.Write(FileType);
        writer.Write(FileVersion);
        writer.Write(WalkmeshType);
        writer.Write(Reserved, 0, 48);
        writer.Write(Position);
        writer.Write(VertexCount);
        writer.Write(OffsetToVertices);
        writer.Write(FaceCount);
        writer.Write(OffsetToFaceIndices);
        writer.Write(OffsetToFaceMaterials);
        writer.Write(OffsetToFaceNormals);
        writer.Write(OffsetToFaceCoefficients);
        writer.Write(AABBCount);
        writer.Write(OffsetToAABBs);
        writer.Write(Unknown);
        writer.Write(AdjacencyCount);
        writer.Write(OffsetToAdjacencies);
        writer.Write(EdgeCount);
        writer.Write(OffsetToEdges);
        writer.Write(PerimeterCount);
        writer.Write(OffsetToPerimeters);
    }
}
