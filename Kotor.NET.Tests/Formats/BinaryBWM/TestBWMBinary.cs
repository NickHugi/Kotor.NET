using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryBWM;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryBWM;

public class TestBWMBinary
{
    public static readonly string File1Filepath = "Formats/BinaryBWM/file1.bwm";

    [SetUp]
    public void Setup()
    {

    }

    private BWMBinary GetBinaryBWM(byte[] data)
    {
        return new BWMBinary(new MemoryStream(data));
    }

    [Test]
    public void Test_ReadFile1()
    {
        var binaryBWM = GetBinaryBWM(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryBWM.FileHeader.FileType, Is.EqualTo("BWM "));
        Assert.That(binaryBWM.FileHeader.FileVersion, Is.EqualTo("V1.0"));
        Assert.That(binaryBWM.FileHeader.VertexCount, Is.EqualTo(6));
        Assert.That(binaryBWM.FileHeader.FaceCount, Is.EqualTo(4));
        Assert.That(binaryBWM.FileHeader.EdgeCount, Is.EqualTo(4));
        Assert.That(binaryBWM.FileHeader.PerimeterCount, Is.EqualTo(1));

        Assert.That(binaryBWM.Vertices.Count, Is.EqualTo(6));
        Assert.That(binaryBWM.FaceIndices.Count, Is.EqualTo(4));
        Assert.That(binaryBWM.FaceMaterials.Count, Is.EqualTo(4));
        Assert.That(binaryBWM.FaceNormals.Count, Is.EqualTo(4));
        Assert.That(binaryBWM.FacePlaneCoefficients.Count, Is.EqualTo(4));
        Assert.That(binaryBWM.Edges.Count, Is.EqualTo(4));
        Assert.That(binaryBWM.Perimeters.Count, Is.EqualTo(1));

        Assert.That(binaryBWM.Vertices[0].X, Is.EqualTo(0));
        Assert.That(binaryBWM.Vertices[0].Y, Is.EqualTo(0));
        Assert.That(binaryBWM.Vertices[0].Z, Is.EqualTo(0));

        Assert.That(binaryBWM.FaceMaterials[0], Is.EqualTo(0));
        Assert.That(binaryBWM.FaceMaterials[3], Is.EqualTo(7));

        Assert.That(binaryBWM.FaceNormals[3].X, Is.EqualTo(0));
        Assert.That(binaryBWM.FaceNormals[3].Y, Is.EqualTo(0));
        Assert.That(binaryBWM.FaceNormals[3].Z, Is.EqualTo(1));

        Assert.That(binaryBWM.Perimeters[0], Is.EqualTo(4));
    }

    [Test]
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

        Assert.That(binaryBWM.FileHeader.VertexCount, Is.EqualTo(6));
        Assert.That(binaryBWM.FileHeader.FaceCount, Is.EqualTo(4));
        Assert.That(binaryBWM.FileHeader.EdgeCount, Is.EqualTo(4));
        Assert.That(binaryBWM.FileHeader.PerimeterCount, Is.EqualTo(1));

        stream.Position = binaryBWM.FileHeader.OffsetToVertices;
        var vertex0 = reader.ReadVector3();
        Assert.That(vertex0.X, Is.EqualTo(0));
        Assert.That(vertex0.Y, Is.EqualTo(0));
        Assert.That(vertex0.Z, Is.EqualTo(0));

        stream.Position = binaryBWM.FileHeader.OffsetToFaceIndices;
        var indices0 = new BWMBinaryFaceIndices(reader);
        Assert.That(indices0.Index1, Is.EqualTo(0));
        Assert.That(indices0.Index2, Is.EqualTo(1));
        Assert.That(indices0.Index3, Is.EqualTo(3));

        stream.Position = binaryBWM.FileHeader.OffsetToFaceMaterials + 4*3;
        var material3 = reader.ReadInt32();
        Assert.That(material3, Is.EqualTo(7));

        stream.Position = binaryBWM.FileHeader.OffsetToFaceNormals;
        var normal0 = reader.ReadVector3();
        Assert.That(normal0.X, Is.EqualTo(0));
        Assert.That(normal0.Y, Is.EqualTo(0));
        Assert.That(normal0.Z, Is.EqualTo(1));

        stream.Position = binaryBWM.FileHeader.OffsetToFaceCoefficients;
        var coefficient0 = reader.ReadSingle();
        Assert.That(coefficient0, Is.EqualTo(0));

        stream.Position = binaryBWM.FileHeader.OffsetToAABBs;
        var aabb0 = new BWMBinaryAABBNode(reader);
        Assert.That(aabb0.FaceIndex, Is.EqualTo(-1));
        Assert.That(aabb0.LeftChildIndex, Is.EqualTo(1));
        Assert.That(aabb0.RightChildIndex, Is.EqualTo(4));

        stream.Position = binaryBWM.FileHeader.OffsetToEdges;
        var edge0 = new BWMBinaryEdge(reader);
        Assert.That(edge0.EdgeIndex, Is.EqualTo(0));
        Assert.That(edge0.Transition, Is.EqualTo(-1));

        stream.Position = binaryBWM.FileHeader.OffsetToPerimeters;
        var perimeter0 = reader.ReadInt32();
        Assert.That(perimeter0, Is.EqualTo(4));
    }
}
