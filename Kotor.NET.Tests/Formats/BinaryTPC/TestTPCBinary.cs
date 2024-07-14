using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTPC;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryTPC;

public class TestTPCBinary
{
    public static readonly string File1Filepath = "Formats/BinaryTPC/file1.tpc";
    public static readonly string File2Filepath = "Formats/BinaryTPC/file2.tpc";

    [SetUp]
    public void Setup()
    {
        
    }

    private TPCBinary GetBinaryTPC(byte[] data)
    {
        return new TPCBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryTPC.FileHeader.Height, Is.EqualTo(32));
        Assert.That(binaryTPC.FileHeader.Width, Is.EqualTo(32));
        Assert.That(binaryTPC.FileHeader.Compressed, Is.EqualTo(true));
        Assert.That(binaryTPC.FileHeader.CubeMap, Is.EqualTo(false));
        Assert.That(binaryTPC.FileHeader.MipmapCount, Is.EqualTo(6));
        Assert.That(binaryTPC.FileHeader.Encoding, Is.EqualTo(4));
        Assert.That(binaryTPC.Layers.Count(), Is.EqualTo(1));
        Assert.That(binaryTPC.Layers.First().Mipmaps.Count(), Is.EqualTo(6));
        Assert.That(binaryTPC.TXI, Is.EqualTo("envmaptexture CM_Baremetal"));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File1Filepath));
        binaryTPC.FileHeader.MipmapCount = Byte.MinValue;
        binaryTPC.FileHeader.Unused = new byte[0];
        binaryTPC.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryTPC.Write(stream);

        Assert.That(binaryTPC.FileHeader.MipmapCount, Is.EqualTo(6));
        Assert.That(binaryTPC.FileHeader.Unused.Count(), Is.EqualTo(114));
    }

    [Test]
    public void Test_ReadFile2()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File2Filepath));

        Assert.That(binaryTPC.FileHeader.Height, Is.EqualTo(192));
        Assert.That(binaryTPC.FileHeader.Width, Is.EqualTo(32));
        Assert.That(binaryTPC.FileHeader.Compressed, Is.EqualTo(true));
        Assert.That(binaryTPC.FileHeader.CubeMap, Is.EqualTo(true));
        Assert.That(binaryTPC.FileHeader.MipmapCount, Is.EqualTo(6));
        Assert.That(binaryTPC.FileHeader.Encoding, Is.EqualTo(4));
        Assert.That(binaryTPC.Layers.Count(), Is.EqualTo(6));
        Assert.That(binaryTPC.Layers.First().Mipmaps.Count(), Is.EqualTo(6));
        Assert.That(binaryTPC.Layers.Last().Mipmaps.Count(), Is.EqualTo(6));
        Assert.That(binaryTPC.TXI, Is.EqualTo("cube 1\r\n"));
    }
}
