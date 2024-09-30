using System.Reflection.PortableExecutable;
using Kotor.NET.Common.Data;
using Kotor.NET.Extensions;
using Kotor.NET.Formats.BinaryTLK;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Xunit;

namespace Kotor.NET.Tests.Formats.BinaryTLK;

public class TestTLKBinary
{
    public static readonly string File1Filepath = "Formats/BinaryTLK/file1.tlk";

    private TLKBinary GetBinaryTLK(byte[] data)
    {
        return new TLKBinary(new MemoryStream(data));
    }
   
    [Fact]
    public void Test_ReadFile1()
    {
        var binaryTLK = GetBinaryTLK(File.ReadAllBytes(File1Filepath));

        Assert.Equal("TLK ", binaryTLK.FileHeader.FileType);
        Assert.Equal("V3.0", binaryTLK.FileHeader.FileVersion);

        Assert.Equal(3, binaryTLK.Entries.Count());

        var entry0 = binaryTLK.Entries[0];
        Assert.Equal("resref01", entry0.SoundResRef.Get());

        var entry1 = binaryTLK.Entries[1];
        Assert.Equal("resref02", entry1.SoundResRef.Get());

        var entry2 = binaryTLK.Entries[2];
        Assert.Equal("", entry2.SoundResRef.Get());

        Assert.Equal("abcdef", binaryTLK.Strings[0]);
        Assert.Equal("ghijklmnop", binaryTLK.Strings[1]);
        Assert.Equal("qrstuvwxyz", binaryTLK.Strings[2]);
    }

    [Fact]
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
        binaryTLK.Write(stream);


        Assert.Equal(binaryTLK.Entries[0].StringSize, "abcdef".Length);

        stream.Position = binaryTLK.FileHeader.OffsetToEntries;
        var entry0 = new TLKBinaryEntry(reader);
        Assert.Equal("resref01", entry0.SoundResRef.Get());

        stream.Position = binaryTLK.Entries[0].OffsetToString;
        var string0 = reader.ReadString(entry0.StringSize);
    }

}
