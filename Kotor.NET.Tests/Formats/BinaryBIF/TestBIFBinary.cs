using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryBIF;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryBIF;

public class TestBIFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryBIF/file1.bif";

    private BIFBinary GetBinaryBIF(byte[] data)
    {
        return new BIFBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryBIF = GetBinaryBIF(File.ReadAllBytes(File1Filepath));

        Assert.Equal("BIFF", binaryBIF.FileHeader.FileType);
        Assert.Equal("V1  ", binaryBIF.FileHeader.FileVersion);
        Assert.Equal(1, binaryBIF.FileHeader.ResourceCount);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryBIF = GetBinaryBIF(File.ReadAllBytes(File1Filepath));
        binaryBIF.FileHeader.ResourceCount = Int32.MinValue;
        binaryBIF.FileHeader.OffsetToUnused = Int32.MinValue;
        binaryBIF.FileHeader.OffsetToResources = Int32.MinValue;
        binaryBIF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryBIF.Write(stream);


        Assert.Equal(1, binaryBIF.FileHeader.ResourceCount);

        stream.Position = binaryBIF.FileHeader.OffsetToResources;
        var resource0 = new BIFBinaryVariableResource(reader);
        Assert.Equal<uint>(resource0.ResourceID, 25165824);
        Assert.Equal(resource0.ResourceType, ResourceType.JRL.ID);
    }
}
