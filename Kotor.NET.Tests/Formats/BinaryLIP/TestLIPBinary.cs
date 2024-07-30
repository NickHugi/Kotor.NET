using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryLIP;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryLIP;

public class TestLIPBinary
{
    public static readonly string File1Filepath = "Formats/BinaryLIP/file1.lip";

    private LIPBinary GetBinaryLIP(byte[] data)
    {
        return new LIPBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryLIP = GetBinaryLIP(File.ReadAllBytes(File1Filepath));

        Assert.Equal("LIP ", binaryLIP.FileHeader.FileType);
        Assert.Equal("V1.0", binaryLIP.FileHeader.FileVersion);

        Assert.Equal(3, binaryLIP.KeyFrames.Count);

        var key0 = binaryLIP.KeyFrames.ElementAt(0);
        Assert.Equal(0, key0.Time, 0.1);
        Assert.Equal(0, key0.Shape);

        var key1 = binaryLIP.KeyFrames.ElementAt(1);
        Assert.Equal(0.77, key1.Time, 0.1);
        Assert.Equal(5, key1.Shape);

        var key2 = binaryLIP.KeyFrames.ElementAt(2);
        Assert.Equal(1.25, key2.Time, 0.1);
        Assert.Equal(10, key2.Shape);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryLIP = GetBinaryLIP(File.ReadAllBytes(File1Filepath));
        binaryLIP.FileHeader.KeyFrameCount = Int32.MinValue;
        binaryLIP.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryLIP.Write(stream);

        stream.Position = 0;
        var fileHeader = new LIPBinaryFileHeader(reader);
        Assert.Equal(3, fileHeader.KeyFrameCount);
    }

}
