using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryLIP;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryLIP;

public class TestLIPBinary
{
    public static readonly string File1Filepath = "Formats/BinaryLIP/file1.lip";

    [SetUp]
    public void Setup()
    {
        
    }

    private LIPBinary GetBinaryLIP(byte[] data)
    {
        return new LIPBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryLIP = GetBinaryLIP(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryLIP.FileHeader.FileType, Is.EqualTo("LIP "), "File type was not read correctly.");
        Assert.That(binaryLIP.FileHeader.FileVersion, Is.EqualTo("V1.0"), "File version was not read correctly.");

        Assert.That(binaryLIP.KeyFrames.Count, Is.EqualTo(3), "Key frame list did not build correctly.");

        var key0 = binaryLIP.KeyFrames.ElementAt(0);
        Assert.That(key0.Time, Is.EqualTo(0).Within(0.1));
        Assert.That(key0.Shape, Is.EqualTo(0));

        var key1 = binaryLIP.KeyFrames.ElementAt(1);
        Assert.That(key1.Time, Is.EqualTo(0.77).Within(0.1));
        Assert.That(key1.Shape, Is.EqualTo(5));

        var key2 = binaryLIP.KeyFrames.ElementAt(2);
        Assert.That(key2.Time, Is.EqualTo(1.25).Within(0.1));
        Assert.That(key2.Shape, Is.EqualTo(10));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryLIP = GetBinaryLIP(File.ReadAllBytes(File1Filepath));
        binaryLIP.FileHeader.KeyFrameCount = Int32.MinValue;
        binaryLIP.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryLIP.Write(stream);


        var fileHeader = new LIPBinaryFileHeader(reader);
        Assert.That(fileHeader.KeyFrameCount, Is.EqualTo(3));
    }

}
