using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryRIM;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryRIM;

public class TestRIMBinary
{
    public static readonly string File1Filepath = "Formats/BinaryRIM/file1.rim";

    [SetUp]
    public void Setup()
    {
        
    }

    private RIMBinary GetBinaryRIM(byte[] data)
    {
        var reader = new BinaryReader(new MemoryStream(data));
        return new RIMBinary(reader);
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryRIM = GetBinaryRIM(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryRIM.FileHeader.FileType, Is.EqualTo("RIM "), "File type was not read correctly.");
        Assert.That(binaryRIM.FileHeader.FileVersion, Is.EqualTo("V1.0"), "File version was not read correctly.");

        Assert.That(binaryRIM.ResourceEntries.Count, Is.EqualTo(2), "Resource entries list did not build correctly.");

        var key0 = binaryRIM.ResourceEntries.ElementAt(0);
        Assert.That(key0.ResourceID, Is.EqualTo(0));
        Assert.That(key0.ResourceTypeID, Is.EqualTo(ResourceType.MDL.ID));
        Assert.That(key0.ResRef.Get(), Is.EqualTo("test"));

        var key1 = binaryRIM.ResourceEntries.ElementAt(1);
        Assert.That(key1.ResourceID, Is.EqualTo(1));
        Assert.That(key1.ResourceTypeID, Is.EqualTo(ResourceType.MDX.ID));
        Assert.That(key1.ResRef.Get(), Is.EqualTo("test"));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryRIM = GetBinaryRIM(File.ReadAllBytes(File1Filepath));
        binaryRIM.FileHeader.OffsetToResources = Int32.MinValue;
        binaryRIM.ResourceEntries.ForEach(x => x.Offset = Int32.MinValue);
        binaryRIM.ResourceEntries.ForEach(x => x.Size = Int32.MinValue);
        binaryRIM.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryRIM.Write(new BinaryWriter(stream));


        Assert.That(binaryRIM.FileHeader.ResourceCount, Is.EqualTo(2));

        stream.Position = binaryRIM.FileHeader.OffsetToResources;
        var resource0 = new RIMBinaryResourceEntry(reader);
        Assert.That(resource0.ResourceID, Is.EqualTo(0));
        Assert.That(resource0.ResourceTypeID, Is.EqualTo(ResourceType.MDL.ID));
        Assert.That(resource0.ResRef.Get(), Is.EqualTo("test"));
        Assert.That(resource0.Offset, Is.EqualTo(binaryRIM.ResourceEntries[0].Offset));
        Assert.That(resource0.Size, Is.EqualTo(binaryRIM.ResourceEntries[0].Size));
    }

}
