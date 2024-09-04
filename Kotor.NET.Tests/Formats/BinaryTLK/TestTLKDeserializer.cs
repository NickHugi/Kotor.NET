using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryTLK;

namespace Kotor.NET.Tests.Formats.BinaryTLK;

public class TestTLKDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryTLK/file1.tlk";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new TLKBinary(stream);
        var deserializer = new TLKBinaryDeserializer(binary);
        var tlk = deserializer.Deserialize();

        Assert.Equal(3, tlk.Count());

        var entry1 = tlk.ElementAt(1);
        Assert.Equal("ghijklmnop", entry1.Text);
        Assert.Equal("resref02", entry1.Sound);
    }
}
