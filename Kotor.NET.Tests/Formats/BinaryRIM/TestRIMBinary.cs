using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryRIM;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryRIM;

public class TestRIMBinary
{
    public static readonly string File1Filepath = "Formats/BinaryRIM/file1.rim";

    private RIMBinary GetBinaryRIM(byte[] data)
    {
        return new RIMBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryRIM = GetBinaryRIM(File.ReadAllBytes(File1Filepath));

        Assert.Equal("RIM ", binaryRIM.FileHeader.FileType);
        Assert.Equal("V1.0", binaryRIM.FileHeader.FileVersion);

        Assert.Equal(2, binaryRIM.ResourceEntries.Count);

        var key0 = binaryRIM.ResourceEntries.ElementAt(0);
        Assert.Equal(0, key0.ResourceID);
        Assert.Equal(key0.ResourceTypeID, ResourceType.MDL.ID);
        Assert.Equal("test", key0.ResRef.Get());

        var key1 = binaryRIM.ResourceEntries.ElementAt(1);
        Assert.Equal(1, key1.ResourceID);
        Assert.Equal(key1.ResourceTypeID, ResourceType.MDX.ID);
        Assert.Equal("test", key1.ResRef.Get());
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryRIM = GetBinaryRIM(File.ReadAllBytes(File1Filepath));
        binaryRIM.FileHeader.OffsetToResources = Int32.MinValue;
        binaryRIM.ResourceEntries.ForEach(x => x.Offset = Int32.MinValue);
        binaryRIM.ResourceEntries.ForEach(x => x.Size = Int32.MinValue);
        binaryRIM.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryRIM.Write(stream);


        Assert.Equal(2, binaryRIM.FileHeader.ResourceCount);

        stream.Position = binaryRIM.FileHeader.OffsetToResources;
        var resource0 = new RIMBinaryResourceEntry(reader);
        Assert.Equal(0, resource0.ResourceID);
        Assert.Equal(resource0.ResourceTypeID, ResourceType.MDL.ID);
        Assert.Equal("test", resource0.ResRef.Get());
        Assert.Equal(resource0.Offset, binaryRIM.ResourceEntries[0].Offset);
        Assert.Equal(resource0.Size, binaryRIM.ResourceEntries[0].Size);
    }

}
