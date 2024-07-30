using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTPC;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryTPC;

public class TestTPCBinary
{
    public static readonly string File1Filepath = "Formats/BinaryTPC/file1.tpc";
    public static readonly string File2Filepath = "Formats/BinaryTPC/file2.tpc";

    private TPCBinary GetBinaryTPC(byte[] data)
    {
        return new TPCBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File1Filepath));

        Assert.Equal(32, binaryTPC.FileHeader.Height);
        Assert.Equal(32, binaryTPC.FileHeader.Width);
        Assert.True(binaryTPC.FileHeader.Compressed);
        Assert.False(binaryTPC.FileHeader.CubeMap);
        Assert.Equal(6, binaryTPC.FileHeader.MipmapCount);
        Assert.Equal(4, binaryTPC.FileHeader.Encoding);
        Assert.Single(binaryTPC.Layers);
        Assert.Equal(6, binaryTPC.Layers.First().Mipmaps.Count());
        Assert.Equal("envmaptexture CM_Baremetal", binaryTPC.TXI);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File1Filepath));
        binaryTPC.FileHeader.MipmapCount = Byte.MinValue;
        binaryTPC.FileHeader.Unused = new byte[0];
        binaryTPC.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryTPC.Write(stream);

        Assert.Equal(6, binaryTPC.FileHeader.MipmapCount);
        Assert.Equal(114, binaryTPC.FileHeader.Unused.Count());
    }

    [Fact]
    public void Test_ReadFile2()
    {
        var binaryTPC = GetBinaryTPC(File.ReadAllBytes(File2Filepath));

        Assert.Equal(192, binaryTPC.FileHeader.Height);
        Assert.Equal(32, binaryTPC.FileHeader.Width);
        Assert.True(binaryTPC.FileHeader.Compressed);
        Assert.True(binaryTPC.FileHeader.CubeMap);
        Assert.Equal(6, binaryTPC.FileHeader.MipmapCount);
        Assert.Equal(4, binaryTPC.FileHeader.Encoding);
        Assert.Equal(6, binaryTPC.Layers.Count());
        Assert.Equal(6, binaryTPC.Layers.First().Mipmaps.Count());
        Assert.Equal(6, binaryTPC.Layers.Last().Mipmaps.Count());
        Assert.Equal("cube 1\r\n", binaryTPC.TXI);
    }
}
