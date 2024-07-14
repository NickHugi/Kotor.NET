using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTLK;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace Kotor.NET.Tests.Formats.BinaryTLK;

public class TestTLKBinary
{
    public static readonly string File1Filepath = "Formats/BinaryTLK/file1.tlk";

    [SetUp]
    public void Setup()
    {
        
    }

    private TLKBinary GetBinaryTLK(byte[] data)
    {
        return new TLKBinary(new MemoryStream(data));
    }
   
    [Test]
    public void Test_ReadFile1()
    {
        var binaryTLK = GetBinaryTLK(File.ReadAllBytes(File1Filepath));

        Assert.That(binaryTLK.FileHeader.FileType, Is.EqualTo("TLK "));
        Assert.That(binaryTLK.FileHeader.FileVersion, Is.EqualTo("V3.0"));

        Assert.That(binaryTLK.Entries.Count(), Is.EqualTo(3));

        var entry0 = binaryTLK.Entries[0];
        Assert.That(entry0.SoundResRef.Get(), Is.EqualTo("resref01"));

        var entry1 = binaryTLK.Entries[1];
        Assert.That(entry1.SoundResRef.Get(), Is.EqualTo("resref02"));

        var entry2 = binaryTLK.Entries[2];
        Assert.That(entry2.SoundResRef.Get(), Is.EqualTo(""));

        Assert.That(binaryTLK.Strings[0], Is.EqualTo("abcdef"));
        Assert.That(binaryTLK.Strings[1], Is.EqualTo("ghijklmnop"));
        Assert.That(binaryTLK.Strings[2], Is.EqualTo("qrstuvwxyz"));
    }

    [Test]
    public void Test_RecalculateFile1()
    {
        var binaryTLK = GetBinaryTLK(File.ReadAllBytes(File1Filepath));
        binaryTLK.FileHeader.EntryCount = Int32.MinValue;
        binaryTLK.FileHeader.OffsetToEntries = Int32.MinValue;
        binaryTLK.Entries.ForEach(x => x.OffsetToString = Int32.MinValue);
        binaryTLK.Entries.ForEach(x => x.StringSize = Int32.MinValue);
        binaryTLK.Recalculate();

        var stream = new MemoryStream();
        var reader = new BinaryReader(stream);
        binaryTLK.Write(new BinaryWriter(stream));


        Assert.That(binaryTLK.Entries[0].StringSize, Is.EqualTo("abcdef".Length));

        stream.Position = binaryTLK.FileHeader.OffsetToEntries;
        var entry0 = new TLKBinaryEntry(reader);
        Assert.That(entry0.SoundResRef.Get(), Is.EqualTo("resref01"));

        stream.Position = binaryTLK.Entries[0].OffsetToString;
        var string0 = reader.ReadString(entry0.StringSize);
    }

}
