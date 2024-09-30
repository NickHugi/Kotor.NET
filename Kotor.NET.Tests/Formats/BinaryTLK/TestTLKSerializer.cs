using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryTLK;
using Kotor.NET.Formats.BinaryTLK.Serialisation;
using Kotor.NET.Resources.KotorTLK;

namespace Kotor.NET.Tests.Formats.BinaryTLK;

public class TestTLKSerializer
{
    public static readonly string File1Filepath = "Formats/BinaryTLK/file1.tlk";

    [Fact]
    public void Test_ReadFile1()
    {
        var tlk = new TLK();
        tlk.Add("abc", "123");
        tlk.Add("def", "456");
        tlk.Add("ghi", "789");

        var serializer = new TLKBinarySerializer(tlk);
        var binary = serializer.Serialize();
        using var stream = new MemoryStream();
        binary.Write(stream);

        //using var streamRead = File.OpenRead(File1Filepath);
        stream.Position = 0;
        var binaryRead = new TLKBinary(stream);
        var deserializer = new TLKBinaryDeserializer(binaryRead);
        var tlkRead = deserializer.Deserialize();

        Assert.Equal(3, tlkRead.Count());

        var entry1 = tlkRead.ElementAt(1);
        Assert.Equal("def", entry1.Text);
        Assert.Equal("456", entry1.Sound);
    }
}
