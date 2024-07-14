using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryKEY;

namespace Kotor.NET.Tests.Formats.BinaryKEY;

public class TestKEYBinary
{
    public static readonly string File1Filepath = "Formats/BinaryKEY/file1.key";

    [SetUp]
    public void Setup()
    {
        
    }

    private KEYBinary GetBinaryKEY(byte[] data)
    {
        return new KEYBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryKEY = GetBinaryKEY(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryKEY.FileHeader.FileType, Is.EqualTo("KEY "));
        Assert.That(binaryKEY.FileHeader.FileVersion, Is.EqualTo("V1  "));
        Assert.That(binaryKEY.FileHeader.FileCount, Is.EqualTo(26));
        Assert.That(binaryKEY.FileHeader.KeyCount, Is.EqualTo(25836));

        Assert.That(binaryKEY.FileTable.Count, Is.EqualTo(26));
        Assert.That(binaryKEY.Filenames.Count, Is.EqualTo(26));
        Assert.That(binaryKEY.Keys.Count, Is.EqualTo(25836));

        Assert.That(binaryKEY.FileTable[0].Drives, Is.EqualTo(1));
        Assert.That(binaryKEY.Filenames[0], Is.EqualTo(@"data\2da.bif"));

        Assert.That(binaryKEY.Keys[0].ResourceID, Is.EqualTo(0));
        Assert.That(binaryKEY.Keys[0].ResRef.Get(), Is.EqualTo("acbonus"));
        Assert.That(binaryKEY.Keys[0].ResourceType, Is.EqualTo(ResourceType.TWODA.ID));
    }

    [Test]
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


        Assert.That(binaryKEY.FileHeader.FileCount, Is.EqualTo(26));
        Assert.That(binaryKEY.FileHeader.KeyCount, Is.EqualTo(25836));

        stream.Position = binaryKEY.FileHeader.OffsetToFileEntries;
        var file0 = new KEYBinaryFileEntry(reader);
        Assert.That(file0.Drives, Is.EqualTo(1));

        stream.Position = binaryKEY.FileTable[0].FilenameOffset;
        var filename1 = reader.ReadString(binaryKEY.FileTable[0].FilenameLength);
        Assert.That(filename1, Is.EqualTo(@"data\2da.bif"));

        stream.Position = binaryKEY.FileHeader.OffsetToKeyEntries;
        var key0 = new KEYBinaryKeyEntry(reader);
        Assert.That(key0.ResourceID, Is.EqualTo(0));
        Assert.That(key0.ResRef.Get(), Is.EqualTo("acbonus"));
        Assert.That(key0.ResourceType, Is.EqualTo(ResourceType.TWODA.ID));
    }
}
