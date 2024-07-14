using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;

namespace Kotor.NET.Tests.Formats.BinaryERF;

public class TestERFBinary
{
    public static readonly string File1Filepath = "Formats/BinaryERF/file1.erf";

    [SetUp]
    public void Setup()
    {
        
    }

    private ERFBinary GetBinaryERF(byte[] data)
    {
        return new ERFBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryERF = GetBinaryERF(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryERF.FileHeader.FileType, Is.EqualTo("ERF "), "File type was not read correctly.");
        Assert.That(binaryERF.FileHeader.FileVersion, Is.EqualTo("V1.0"), "File version was not read correctly.");
        Assert.That(binaryERF.FileHeader.EntryCount, Is.EqualTo(2), "Entry count was not read correctly.");

        Assert.That(binaryERF.KeyEntries.Count, Is.EqualTo(2), "Key entries list did not build correctly.");
        Assert.That(binaryERF.ResourceEntries.Count, Is.EqualTo(2), "Resource entries list did not build correctly.");

        var key0 = binaryERF.KeyEntries.ElementAt(0);
        Assert.That(key0.ResID, Is.EqualTo(0));
        Assert.That(key0.ResRef.Get(), Is.EqualTo("test"));
        Assert.That(key0.ResType, Is.EqualTo(ResourceType.MDL.ID));

        var key1 = binaryERF.KeyEntries.ElementAt(1);
        Assert.That(key1.ResID, Is.EqualTo(1));
        Assert.That(key1.ResRef.Get(), Is.EqualTo("test"));
        Assert.That(key1.ResType, Is.EqualTo(ResourceType.MDX.ID));
    }

    [Test]
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


        Assert.That(binaryERF.FileHeader.EntryCount, Is.EqualTo(2));

        stream.Position = binaryERF.FileHeader.OffsetToKeyList;
        var key0 = new ERFBinaryKeyEntry(reader);
        Assert.That(key0.ResID, Is.EqualTo(0));
        Assert.That(key0.ResRef.Get(), Is.EqualTo("test"));
        Assert.That(key0.ResType, Is.EqualTo(ResourceType.MDL.ID));

        stream.Position = binaryERF.FileHeader.OffsetToResourceList;
        var resource0 = new ERFBinaryResourceEntry(reader);
        Assert.That(resource0.Offset, Is.EqualTo(binaryERF.ResourceEntries[0].Offset));
        Assert.That(resource0.Size, Is.EqualTo(binaryERF.ResourceEntries[0].Size));
    }

}
