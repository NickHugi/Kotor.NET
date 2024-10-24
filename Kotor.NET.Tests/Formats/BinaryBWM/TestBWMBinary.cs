using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryBWM;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryBWM;

public class TestBWMBinary
{
    public static readonly string File1Filepath = "Formats/BinaryBWM/file1.bwm";

    private BWMBinary GetBinaryBWM(byte[] data)
    {
        return new BWMBinary(new MemoryStream(data));
    }

    [Fact]
    public void Test_ReadFile1()
    {
        var binaryBWM = GetBinaryBWM(File.ReadAllBytes(File1Filepath));

        Assert.Equal("BWM ", binaryBWM.FileHeader.FileType);
        Assert.Equal("V1.0", binaryBWM.FileHeader.FileVersion);
        Assert.Equal(6, binaryBWM.FileHeader.VertexCount);
        Assert.Equal(4, binaryBWM.FileHeader.FaceCount);
        Assert.Equal(4, binaryBWM.FileHeader.EdgeCount);
        Assert.Equal(1, binaryBWM.FileHeader.PerimeterCount);

        Assert.Equal(6, binaryBWM.Vertices.Count);
        Assert.Equal(4, binaryBWM.FaceIndices.Count);
        Assert.Equal(4, binaryBWM.FaceMaterials.Count);
        Assert.Equal(4, binaryBWM.FaceNormals.Count);
        Assert.Equal(4, binaryBWM.FacePlaneDistances.Count);
        Assert.Equal(4, binaryBWM.Edges.Count);
        Assert.Single(binaryBWM.Perimeters);

        Assert.Equal(0, binaryBWM.Vertices[0].X);
        Assert.Equal(0, binaryBWM.Vertices[0].Y);
        Assert.Equal(0, binaryBWM.Vertices[0].Z);

        Assert.Equal(0, binaryBWM.FaceMaterials[0]);
        Assert.Equal(7, binaryBWM.FaceMaterials[3]);

        Assert.Equal(0, binaryBWM.FaceNormals[3].X);
        Assert.Equal(0, binaryBWM.FaceNormals[3].Y);
        Assert.Equal(1, binaryBWM.FaceNormals[3].Z);

        Assert.Equal(4, binaryBWM.Perimeters[0]);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryBWM = GetBinaryBWM(File.ReadAllBytes(File1Filepath));
        binaryBWM.FileHeader.VertexCount = Int32.MinValue;
        binaryBWM.FileHeader.FaceCount = Int32.MinValue;
        binaryBWM.FileHeader.EdgeCount = Int32.MinValue;
        binaryBWM.FileHeader.AABBCount = Int32.MinValue;
        binaryBWM.FileHeader.PerimeterCount = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToVertices = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToFaceIndices = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToFaceMaterials = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToFaceNormals = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToFaceCoefficients = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToAABBs = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToEdges = Int32.MinValue;
        binaryBWM.FileHeader.OffsetToPerimeters = Int32.MinValue;
        binaryBWM.Recalculate();


        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryBWM.Write(stream);

        stream.Position = 0;
        var fileHeader = new BWMBinaryFileHeader(reader);
        Assert.Equal(6, fileHeader.VertexCount);
        Assert.Equal(4, fileHeader.FaceCount);
        Assert.Equal(4, fileHeader.EdgeCount);
        Assert.Equal(1, fileHeader.PerimeterCount);

        stream.Position = fileHeader.OffsetToVertices;
        var vertex0 = reader.ReadVector3();
        Assert.Equal(0, vertex0.X);
        Assert.Equal(0, vertex0.Y);
        Assert.Equal(0, vertex0.Z);

        stream.Position = fileHeader.OffsetToFaceIndices;
        var indices0 = new BWMBinaryFaceIndices(reader);
        Assert.Equal(0, indices0.Index1);
        Assert.Equal(1, indices0.Index2);
        Assert.Equal(3, indices0.Index3);

        stream.Position = fileHeader.OffsetToFaceMaterials + 4*3;
        var material3 = reader.ReadInt32();
        Assert.Equal(7, material3);

        stream.Position = fileHeader.OffsetToFaceNormals;
        var normal0 = reader.ReadVector3();
        Assert.Equal(0, normal0.X);
        Assert.Equal(0, normal0.Y);
        Assert.Equal(1, normal0.Z);

        stream.Position = fileHeader.OffsetToFaceCoefficients;
        var coefficient0 = reader.ReadSingle();
        Assert.Equal(0, coefficient0);

        stream.Position = fileHeader.OffsetToAABBs;
        var aabb0 = new BWMBinaryAABBNode(reader);
        Assert.Equal(-1, aabb0.FaceIndex);
        Assert.Equal(1, aabb0.LeftChildIndex);
        Assert.Equal(4, aabb0.RightChildIndex);

        stream.Position = fileHeader.OffsetToEdges;
        var edge0 = new BWMBinaryEdge(reader);
        Assert.Equal(0, edge0.EdgeIndex);
        Assert.Equal(-1, edge0.Transition);

        stream.Position = fileHeader.OffsetToPerimeters;
        var perimeter0 = reader.ReadInt32();
        Assert.Equal(4, perimeter0);
    }
}
