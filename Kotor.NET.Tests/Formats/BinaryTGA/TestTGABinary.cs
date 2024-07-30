using System.Reflection.PortableExecutable;
using System.Text;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTGA;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryTGA;

public class TestTGABinary
{
    public static readonly string File1Filepath = "Formats/BinaryTGA/file1.tga"; // 32-bit Unmapped Uncompressed
    public static readonly string File2Filepath = "Formats/BinaryTGA/file2.tga"; // 24-bit Unmapped RLE

    private TGABinary GetBinaryTGA(byte[] data)
    {
        return new TGABinary(new MemoryStream(data));
    }

    [Fact]
    public void Test_ReadFile1()
    {
        var binaryTGA = GetBinaryTGA(File.ReadAllBytes(File1Filepath));

        Assert.Equal(64, binaryTGA.FileHeader.Width);
        Assert.Equal(64, binaryTGA.FileHeader.Height);
        Assert.Empty(binaryTGA.ColourMap);
        Assert.Equal(64 * 64, binaryTGA.ImageData.Count());

        // BGRA values of first pixel
        Assert.Equal(142, binaryTGA.ImageData[0][0]);
        Assert.Equal(179, binaryTGA.ImageData[0][1]);
        Assert.Equal(15, binaryTGA.ImageData[0][2]);
        Assert.Equal(0, binaryTGA.ImageData[0][3]);
    }

    [Fact]
    public void Test_ReadFile2()
    {
        var binaryTGA = GetBinaryTGA(File.ReadAllBytes(File2Filepath));

        Assert.Equal(64, binaryTGA.FileHeader.Width);
        Assert.Equal(64, binaryTGA.FileHeader.Height);
        Assert.Empty(binaryTGA.ColourMap);

        // First packet values - 64 white pixels
        Assert.Equal(191, binaryTGA.ImageData[0][0]);
        Assert.Equal(255, binaryTGA.ImageData[0][1]);
        Assert.Equal(255, binaryTGA.ImageData[0][2]);
        Assert.Equal(255, binaryTGA.ImageData[0][3]);
    }

}
