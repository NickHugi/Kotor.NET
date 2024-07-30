using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryKEY;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryKEY;

public class TestKEYBinary
{
    public static readonly string File1Filepath = "Formats/BinaryKEY/file1.key";

    private KEYBinary GetBinaryKEY(byte[] data)
    {
        return new KEYBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryKEY = GetBinaryKEY(File.ReadAllBytes(File1Filepath));

        Assert.Equal("KEY ", binaryKEY.FileHeader.FileType);
        Assert.Equal("V1  ", binaryKEY.FileHeader.FileVersion);
        Assert.Equal(26, binaryKEY.FileHeader.FileCount);
        Assert.Equal(25836, binaryKEY.FileHeader.KeyCount);

        Assert.Equal(26, binaryKEY.FileTable.Count);
        Assert.Equal(26, binaryKEY.Filenames.Count);
        Assert.Equal(25836, binaryKEY.Keys.Count);

        Assert.Equal(1, binaryKEY.FileTable[0].Drives);
        Assert.Equal(@"data\2da.bif", binaryKEY.Filenames[0]);

        Assert.Equal<uint>(0, binaryKEY.Keys[0].ResourceID);
        Assert.Equal("acbonus", binaryKEY.Keys[0].ResRef.Get());
        Assert.Equal(binaryKEY.Keys[0].ResourceType, ResourceType.TWODA.ID);
    }

    [Fact]
    public void Test_RecalculateFile1()
    {
        var binaryKEY = GetBinaryKEY(File.ReadAllBytes(File1Filepath));
        binaryKEY.FileHeader.FileCount = Int32.MinValue;
        binaryKEY.FileHeader.KeyCount = Int32.MinValue;
        binaryKEY.FileHeader.OffsetToFileEntries = Int32.MinValue;
        binaryKEY.FileHeader.OffsetToKeyEntries = Int32.MinValue;
        binaryKEY.FileTable.ForEach(x => x.FilenameOffset = Int32.MinValue);
        binaryKEY.FileTable.ForEach(x => x.FilenameLength = Int16.MinValue);
        binaryKEY.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryKEY.Write(stream);


        Assert.Equal(26, binaryKEY.FileHeader.FileCount);
        Assert.Equal(25836, binaryKEY.FileHeader.KeyCount);

        stream.Position = binaryKEY.FileHeader.OffsetToFileEntries;
        var file0 = new KEYBinaryFileEntry(reader);
        Assert.Equal(1, file0.Drives);

        stream.Position = binaryKEY.FileTable[0].FilenameOffset;
        var filename1 = reader.ReadString(binaryKEY.FileTable[0].FilenameLength);
        Assert.Equal(@"data\2da.bif", filename1);

        stream.Position = binaryKEY.FileHeader.OffsetToKeyEntries;
        var key0 = new KEYBinaryKeyEntry(reader);
        Assert.Equal<uint>(0, key0.ResourceID);
        Assert.Equal("acbonus", key0.ResRef.Get());
        Assert.Equal(key0.ResourceType, ResourceType.TWODA.ID);
    }
}
