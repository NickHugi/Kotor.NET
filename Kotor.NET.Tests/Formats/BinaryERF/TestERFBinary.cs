using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryERF;

public class TestERFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryERF/file1.erf";

    private ERFBinary GetBinaryERF(byte[] data)
    {
        return new ERFBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryERF = GetBinaryERF(File.ReadAllBytes(File1Filepath));

        Assert.Equal("ERF ", binaryERF.FileHeader.FileType);
        Assert.Equal("V1.0", binaryERF.FileHeader.FileVersion);
        Assert.Equal(2, binaryERF.FileHeader.EntryCount);

        Assert.Equal(2, binaryERF.KeyEntries.Count);
        Assert.Equal(2, binaryERF.ResourceEntries.Count);

        var key0 = binaryERF.KeyEntries.ElementAt(0);
        Assert.Equal<uint>(0, key0.ResID);
        Assert.Equal("test", key0.ResRef.Get());
        Assert.Equal(key0.ResType, ResourceType.MDL.ID);

        var key1 = binaryERF.KeyEntries.ElementAt(1);
        Assert.Equal<uint>(1, key1.ResID);
        Assert.Equal("test", key1.ResRef.Get());
        Assert.Equal(key1.ResType, ResourceType.MDX.ID);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryERF = GetBinaryERF(File.ReadAllBytes(File1Filepath));
        binaryERF.FileHeader.EntryCount = Int32.MinValue;
        binaryERF.FileHeader.OffsetToKeyList = Int32.MinValue;
        binaryERF.FileHeader.OffsetToResourceList = Int32.MinValue;
        binaryERF.ResourceEntries.ForEach(x => x.Offset = Int32.MinValue);
        binaryERF.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryERF.Write(stream);


        stream.Position = 0;
        var fileHeader = new ERFBinaryFileHeader(reader);
        Assert.Equal(2, fileHeader.EntryCount);

        stream.Position = fileHeader.OffsetToKeyList;
        var key0 = new ERFBinaryKeyEntry(reader);
        Assert.Equal<uint>(0, key0.ResID);
        Assert.Equal("test", key0.ResRef.Get());
        Assert.Equal(key0.ResType, ResourceType.MDL.ID);

        stream.Position = fileHeader.OffsetToResourceList;
        var resource0 = new ERFBinaryResourceEntry(reader);
        Assert.Equal(resource0.Offset, binaryERF.ResourceEntries[0].Offset);
        Assert.Equal(resource0.Size, binaryERF.ResourceEntries[0].Size);
    }

}
