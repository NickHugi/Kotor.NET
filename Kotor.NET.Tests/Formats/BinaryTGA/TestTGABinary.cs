using System.Reflection.PortableExecutable;
using System.Text;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTGA;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryTGA;

public class TestTGABinary
{
    public static readonly string File1Filepath = "Formats/BinaryTGA/file1.tga"; // 32-bit Unmapped Uncompressed
    public static readonly string File2Filepath = "Formats/BinaryTGA/file2.tga"; // 24-bit Unmapped RLE

    [SetUp]
    public void Setup()
    {
        
    }

    private TGABinary GetBinaryTGA(byte[] data)
    {
        var reader = new BinaryReader(new MemoryStream(data));
        return new TGABinary(reader);
    }

    [Test]
    public void Test_ReadFile1()
    {
        var binaryTGA = GetBinaryTGA(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryTGA.FileHeader.Width, Is.EqualTo(64));
        Assert.That(binaryTGA.FileHeader.Height, Is.EqualTo(64));
        Assert.That(binaryTGA.ColourMap.Count(), Is.EqualTo(0));
        Assert.That(binaryTGA.ImageData.Count(), Is.EqualTo(64 * 64));

        // BGRA values of first pixel
        Assert.That(binaryTGA.ImageData[0][0], Is.EqualTo(142));
        Assert.That(binaryTGA.ImageData[0][1], Is.EqualTo(179));
        Assert.That(binaryTGA.ImageData[0][2], Is.EqualTo(15));
        Assert.That(binaryTGA.ImageData[0][3], Is.EqualTo(0));
    }

    [Test]
    public void Test_ReadFile2()
    {
        var binaryTGA = GetBinaryTGA(File.ReadAllBytes(File2Filepath));

        Assert.That(binaryTGA.FileHeader.Width, Is.EqualTo(64));
        Assert.That(binaryTGA.FileHeader.Height, Is.EqualTo(64));
        Assert.That(binaryTGA.ColourMap.Count(), Is.EqualTo(0));

        // First packet values - 64 white pixels
        Assert.That(binaryTGA.ImageData[0][0], Is.EqualTo(191));
        Assert.That(binaryTGA.ImageData[0][1], Is.EqualTo(255));
        Assert.That(binaryTGA.ImageData[0][2], Is.EqualTo(255));
        Assert.That(binaryTGA.ImageData[0][3], Is.EqualTo(255));
    }

}
