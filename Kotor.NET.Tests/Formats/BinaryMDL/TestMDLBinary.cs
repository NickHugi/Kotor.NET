using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.NET.Tests.Formats.BinaryMDL;

public class TestMDLBinary
{
    public static readonly string File1Filepath = "Formats/BinaryMDL/file1";
    public static readonly string File2Filepath = "Formats/BinaryMDL/file2";
    public static readonly string File3Filepath = "Formats/BinaryMDL/file3";
    public static readonly string File4Filepath = "Formats/BinaryMDL/file4";

    private MDLBinary GetBinaryMDL(string extensionlessPath)
    {
        var mdlData = File.ReadAllBytes(extensionlessPath + ".mdl");
        var mdxData = File.ReadAllBytes(extensionlessPath + ".mdx");
        return new MDLBinary(new MemoryStream(mdlData), new MemoryStream(mdxData));
    }

    [Fact]
    public void Test_ReadFile1()
    {
        var binaryMDL = GetBinaryMDL(File1Filepath);

        Assert.Equal(134, binaryMDL.ModelHeader.GeometryHeader.NodeCount);
        Assert.Equal("m14aa_01c", binaryMDL.ModelHeader.GeometryHeader.Name);
        Assert.Equal(2, binaryMDL.ModelHeader.GeometryHeader.GeometryType);
        Assert.Equal(0, binaryMDL.ModelHeader.ModelType);
        Assert.Equal(7, binaryMDL.ModelHeader.Radius);
        Assert.Equal(1, binaryMDL.ModelHeader.AnimationScale);
        Assert.Equal("NULL", binaryMDL.ModelHeader.SupermodelName);
        Assert.Equal("m14aa_01c", binaryMDL.Names.First());
        Assert.Equal("brglight04", binaryMDL.Names.Last());
        Assert.Equal(48, binaryMDL.RootNode.NodeHeader.ChildArrayCount);
        Assert.Equal(2, binaryMDL.RootNode.Children.First().ControllerHeaders.Count);
        Assert.Equal(9, binaryMDL.RootNode.Children.First().ControllerData.Count);
        Assert.Equal(2, binaryMDL.RootNode.Children.First().NodeHeader.ControllerArrayCount);
    }

    [Fact]
    public void Test_ReadFile2()
    {
        var binaryMDL = GetBinaryMDL(File2Filepath);

        Assert.Equal("m14aa_01f", binaryMDL.ModelHeader.GeometryHeader.Name);
    }

    [Fact]
    public void Test_ReadFile3()
    {
        var binaryMDL = GetBinaryMDL(File3Filepath);

        Assert.Equal("P_Zaalbar", binaryMDL.ModelHeader.GeometryHeader.Name);
        Assert.Single(binaryMDL.Animations);
        Assert.Equal(1.43333, binaryMDL.Animations.First().AnimationHeader.AnimationLength, 0.1);
        Assert.Equal(0.25, binaryMDL.Animations.First().AnimationHeader.TransitionTime);
        Assert.Equal("P_Zaalbar", binaryMDL.Animations.First().AnimationHeader.AnimationRoot);
        Assert.Equal(0, binaryMDL.Animations.First().AnimationHeader.EventCount);
        Assert.Empty(binaryMDL.Animations.First().Events);
    }

    [Fact]
    public void Test_ReadFile4()
    {
        var binaryMDL = GetBinaryMDL(File4Filepath);

        Assert.Equal("w_ShortSbr_001", binaryMDL.ModelHeader.GeometryHeader.Name);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryMDL = GetBinaryMDL(File1Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(stream, mdxStream);

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(stream, mdxStream);
        Assert.Equal(48, newBinaryMDL.RootNode.NodeHeader.ChildArrayCount);
        Assert.Equal(2, newBinaryMDL.RootNode.Children.First().ControllerHeaders.Count);
        Assert.Equal(9, newBinaryMDL.RootNode.Children.First().ControllerData.Count);
        Assert.Equal(2, newBinaryMDL.RootNode.Children.First().NodeHeader.ControllerArrayCount);
    }

    [Fact]
    public void Test_RecalculateFile2()
    {
        var binaryMDL = GetBinaryMDL(File2Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(stream, mdxStream);

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(stream, mdxStream);
    }

    [Fact]
    public void Test_RecalculateFile3()
    {
        var binaryMDL = GetBinaryMDL(File3Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(stream, mdxStream);

        var reader = new BinaryReader(stream);
        var mdxReader = new BinaryReader(mdxStream);
        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(stream, mdxStream);
    }

    [Fact]
    public void Test_RecalculateFile4()
    {
        var binaryMDL = GetBinaryMDL(File4Filepath);
        binaryMDL.Recalculate();

        var stream = new MemoryStream();
        var mdxStream = new MemoryStream();
        binaryMDL.Write(stream, mdxStream);

        stream.Position = 0;
        mdxStream.Position = 0;

        var newBinaryMDL = new MDLBinary(stream, mdxStream);
    }

    [Fact]
    public void Test_ParseFile1()
    {
        var binaryMDL = GetBinaryMDL(File1Filepath);
        var mdl = binaryMDL.Parse();
        mdl.Root.Children.RemoveRange(10, mdl.Root.Children.Count-10);
        mdl.Root.Children = mdl.Root.Children.Where(x => x is not MDLWalkmeshNode).ToList();
        mdl.Root.Children = mdl.Root.Children.Where(x => x is not MDLLightNode).ToList();
        foreach (var item in mdl.Root.GetAllAncestors())
        {
            if (mdl.Root.Children.Contains(item))
                item.Children.Clear();
            //item.Children = item.Children.Where(x => x is not MDLDanglyNode).ToList();
            //item.Children = item.Children.Where(x => x is not MDLWalkmeshNode).ToList();
            //item.Children = item.Children.Where(x => x is not MDLLightNode).ToList();
        }

        var b2 = new MDLBinary();
        //b2.Unparse(mdl);

        //var c = mdl.Root.Children.ElementAt(8);
        //mdl.Root.Children.Clear();
        //mdl.Root.Children.Add(c);
        b2.Unparse(mdl);

        var mdlstream = File.OpenWrite(@"C:\Users\hugin\Desktop\ext\test.mdl");
        var mdxstream = File.OpenWrite(@"C:\Users\hugin\Desktop\ext\test.mdx");
        b2.Write(mdlstream, mdxstream);
        mdlstream.Close();
        mdxstream.Close();

        var abc = GetBinaryMDL(@"C:\Users\hugin\Desktop\ext\test");
        var remdl = abc.Parse();


    }
}
