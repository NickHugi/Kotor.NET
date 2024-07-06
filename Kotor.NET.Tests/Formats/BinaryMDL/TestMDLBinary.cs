using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryMDL;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryMDL;

public class TestMDLBinary
{
    public static readonly string File1Filepath = "Formats/BinaryMDL/file1";
    public static readonly string File2Filepath = "Formats/BinaryMDL/file2";
    public static readonly string File3Filepath = "Formats/BinaryMDL/file3";
    public static readonly string File4Filepath = "Formats/BinaryMDL/file4";

    [SetUp]
    public void Setup()
    {
        
    }

    private MDLBinary GetBinaryMDL(string extensionlessPath)
    {
        var mdlData = File.ReadAllBytes(extensionlessPath + ".mdl");
        var mdlReader = new BinaryReader(new MemoryStream(mdlData));
        var mdxData = File.ReadAllBytes(extensionlessPath + ".mdx");
        var mdxReader = new BinaryReader(new MemoryStream(mdxData));
        return new MDLBinary(mdlReader, mdxReader);
    }

    [Test]
    public void Test_ReadFile1()
    {
        var binaryMDL = GetBinaryMDL(File1Filepath);

        Assert.That(binaryMDL.ModelHeader.GeometryHeader.NodeCount, Is.EqualTo(134));
        Assert.That(binaryMDL.ModelHeader.GeometryHeader.Name, Is.EqualTo("m14aa_01c"));
        Assert.That(binaryMDL.ModelHeader.GeometryHeader.GeometryType, Is.EqualTo(2));
        Assert.That(binaryMDL.ModelHeader.ModelType, Is.EqualTo(0));
        Assert.That(binaryMDL.ModelHeader.Radius, Is.EqualTo(7));
        Assert.That(binaryMDL.ModelHeader.AnimationScale, Is.EqualTo(1));
        Assert.That(binaryMDL.ModelHeader.SupermodelName, Is.EqualTo("NULL"));
        Assert.That(binaryMDL.Names.First(), Is.EqualTo("m14aa_01c"));
        Assert.That(binaryMDL.Names.Last(), Is.EqualTo("brglight04"));
        Assert.That(binaryMDL.RootNode.NodeHeader.ChildArrayCount, Is.EqualTo(48));
        Assert.That(binaryMDL.RootNode.Children.First().ControllerHeaders.Count, Is.EqualTo(2));
        Assert.That(binaryMDL.RootNode.Children.First().ControllerData.Count, Is.EqualTo(9));
        Assert.That(binaryMDL.RootNode.Children.First().NodeHeader.ControllerArrayCount, Is.EqualTo(2));
    }

    [Test]
    public void Test_ReadFile2()
    {
        var binaryMDL = GetBinaryMDL(File2Filepath);

        Assert.That(binaryMDL.ModelHeader.GeometryHeader.Name, Is.EqualTo("m14aa_01f"));
    }

    [Test]
    public void Test_ReadFile3()
    {
        var binaryMDL = GetBinaryMDL(File3Filepath);

        Assert.That(binaryMDL.ModelHeader.GeometryHeader.Name, Is.EqualTo("P_Zaalbar"));
        Assert.That(binaryMDL.Animations.Count, Is.EqualTo(1));
        Assert.That(binaryMDL.Animations.First().AnimationHeader.AnimationLength, Is.EqualTo(1.43333).Within(0.1));
        Assert.That(binaryMDL.Animations.First().AnimationHeader.TransitionTime, Is.EqualTo(0.25));
        Assert.That(binaryMDL.Animations.First().AnimationHeader.AnimationRoot, Is.EqualTo("P_Zaalbar"));
        Assert.That(binaryMDL.Animations.First().AnimationHeader.EventCount, Is.EqualTo(0));
        Assert.That(binaryMDL.Animations.First().Events.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Test_ReadFile4()
    {
        var binaryMDL = GetBinaryMDL(File4Filepath);

        Assert.That(binaryMDL.ModelHeader.GeometryHeader.Name, Is.EqualTo("w_ShortSbr_001"));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryMDL = GetBinaryMDL(File1Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(new BinaryWriter(stream), new BinaryWriter(mdxStream));

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(reader, mdxReader);
        Assert.That(newBinaryMDL.RootNode.NodeHeader.ChildArrayCount, Is.EqualTo(48));
        Assert.That(newBinaryMDL.RootNode.Children.First().ControllerHeaders.Count, Is.EqualTo(2));
        Assert.That(newBinaryMDL.RootNode.Children.First().ControllerData.Count, Is.EqualTo(9));
        Assert.That(newBinaryMDL.RootNode.Children.First().NodeHeader.ControllerArrayCount, Is.EqualTo(2));
    }

    [Test]
    public void Test_RecalculateFile2()
    {
        var binaryMDL = GetBinaryMDL(File2Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(new BinaryWriter(stream), new BinaryWriter(mdxStream));

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(reader, mdxReader);
    }

    [Test]
    public void Test_RecalculateFile3()
    {
        var binaryMDL = GetBinaryMDL(File3Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(new BinaryWriter(stream), new BinaryWriter(mdxStream));

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(reader, mdxReader);
    }

    [Test]
    public void Test_RecalculateFile4()
    {
        var binaryMDL = GetBinaryMDL(File4Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(new BinaryWriter(stream), new BinaryWriter(mdxStream));

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(reader, mdxReader);
    }
}
