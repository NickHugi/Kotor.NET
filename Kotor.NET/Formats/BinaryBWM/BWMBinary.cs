using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;

namespace Kotor.NET.Formats.BinaryBWM;

public class BWMBinary
{
    public BWMBinaryFileHeader FileHeader { get; set; } = new();
    public List<Vector3> Vertices { get; set; } = new();
    public List<BWMBinaryFaceIndices> FaceIndices { get; set; } = new();
    public List<int> FaceMaterials { get; set;} = new();
    public List<Vector3> FaceNormals { get; set;} = new();
    public List<float> FacePlaneCoefficients { get; set; } = new();
    public List<BWMBinaryAABBNode> AABBs { get; set; } = new();
    public List<BWMBinaryAdjacency> Adjacencies { get; set; } = new();
    public List<BWMBinaryEdge> Edges { get; set; } = new();
    public List<int> Perimeters { get; set; } = new();

    public BWMBinary()
    {

    }

    public BWMBinary(BinaryReader reader)
    {
        FileHeader = new BWMBinaryFileHeader(reader);

        reader.BaseStream.Position = FileHeader.OffsetToVertices;
        for (int i = 0; i < FileHeader.VertexCount; i++)
        {
            Vertices.Add(reader.ReadVector3());
        }

        reader.BaseStream.Position = FileHeader.OffsetToFaceIndices;
        for (int i = 0; i < FileHeader.FaceCount; i++)
        {
            FaceIndices.Add(new BWMBinaryFaceIndices(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToFaceMaterials;
        for (int i = 0; i < FileHeader.FaceCount; i++)
        {
            FaceMaterials.Add(reader.ReadInt32());
        }

        reader.BaseStream.Position = FileHeader.OffsetToFaceNormals;
        for (int i = 0; i < FileHeader.FaceCount; i++)
        {
            FaceNormals.Add(reader.ReadVector3());
        }

        reader.BaseStream.Position = FileHeader.OffsetToFaceCoefficients;
        for (int i = 0; i < FileHeader.FaceCount; i++)
        {
            FacePlaneCoefficients.Add(reader.ReadSingle());
        }

        reader.BaseStream.Position = FileHeader.OffsetToAABBs;
        for (int i = 0; i < FileHeader.AABBCount; i++)
        {
            AABBs.Add(new BWMBinaryAABBNode(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToAdjacencies;
        for (int i = 0; i < FileHeader.AdjacencyCount; i++)
        {
            Adjacencies.Add(new BWMBinaryAdjacency(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToEdges;
        for (int i = 0; i < FileHeader.EdgeCount; i++)
        {
            Edges.Add(new BWMBinaryEdge(reader));
        }

        reader.BaseStream.Position = FileHeader.OffsetToPerimeters;
        for (int i = 0; i < FileHeader.PerimeterCount; i++)
        {
            Perimeters.Add(reader.ReadInt32());
        }
    }

    public void Write(BinaryWriter writer)
    {
        FileHeader.Write(writer);

        writer.BaseStream.Position = FileHeader.OffsetToVertices;
        foreach (var vertex in Vertices)
        {
            writer.Write(vertex);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFaceIndices;
        foreach (var indices in FaceIndices)
        {
            indices.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFaceMaterials;
        foreach (var material in FaceMaterials)
        {
            writer.Write(material);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFaceNormals;
        foreach (var normal in FaceNormals)
        {
            writer.Write(normal);
        }

        writer.BaseStream.Position = FileHeader.OffsetToFaceCoefficients;
        foreach (var coeff in FacePlaneCoefficients)
        {
            writer.Write(coeff);
        }

        writer.BaseStream.Position = FileHeader.OffsetToAABBs;
        foreach (var aabb in AABBs)
        {
            aabb.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToAdjacencies;
        foreach (var adjacency in Adjacencies)
        {
            adjacency.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToEdges;
        foreach (var edge in Edges)
        {
            edge.Write(writer);
        }

        writer.BaseStream.Position = FileHeader.OffsetToPerimeters;
        foreach (var perimeter in Perimeters)
        {
            writer.Write(perimeter);
        }
    }

    public void Recalculate()
    {
        FileHeader.VertexCount = Vertices.Count;
        FileHeader.FaceCount = FaceIndices.Count;
        FileHeader.AABBCount = AABBs.Count;
        FileHeader.AdjacencyCount = Adjacencies.Count;
        FileHeader.PerimeterCount = Perimeters.Count;
        FileHeader.EdgeCount = Edges.Count;

        var offset = BWMBinaryFileHeader.SIZE;
        FileHeader.OffsetToVertices = offset;

        offset += Vertices.Count * 12;
        FileHeader.OffsetToFaceIndices = offset;

        offset += FaceIndices.Count * 12;
        FileHeader.OffsetToFaceMaterials = offset;

        offset += FaceMaterials.Count * 4;
        FileHeader.OffsetToFaceNormals = offset;

        offset += FaceNormals.Count * 12;
        FileHeader.OffsetToFaceCoefficients = offset;

        offset += FacePlaneCoefficients.Count * 4;
        FileHeader.OffsetToAABBs = offset;

        offset += AABBs.Count * BWMBinaryAABBNode.SIZE;
        FileHeader.OffsetToAdjacencies = offset;

        offset += Adjacencies.Count * BWMBinaryAdjacency.SIZE;
        FileHeader.OffsetToEdges = offset;

        offset += Edges.Count * BWMBinaryEdge.SIZE;
        FileHeader.OffsetToPerimeters = offset;
    }
}
