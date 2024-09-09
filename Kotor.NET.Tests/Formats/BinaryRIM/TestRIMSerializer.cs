using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryRIM;
using Kotor.NET.Resources.KotorRIM;

namespace Kotor.NET.Tests.Formats.BinaryRIM;

public class TestRIMSerializer
{
    public static readonly string File1Filepath = "Formats/BinaryRIM/file1.rim";

    [Fact]
    public void Test_ReadFile1()
    {
        var rim = new RIM
        {
            { "ifo", ResourceType.IFO, [0x01, 0x02, 0x03] },
            { "are", ResourceType.ARE, [0x04, 0x05, 0x06] },
            { "git", ResourceType.GIT, [0x07, 0x08, 0x09] }
        };

        var serializer = new RIMBinarySerializer(rim);
        var binary = serializer.Serialize();
        using var stream = new MemoryStream();
        binary.Write(stream);

        stream.Position = 0;
        var binaryRead = new RIMBinary(stream);
        var deserializer = new RIMBinaryDeserializer(binaryRead);
        var rimRead = deserializer.Deserialize();

        Assert.Equal(3, rimRead.Count());

        var resource = rimRead.ElementAt(1);
        Assert.Equal("are", resource.ResRef);
        Assert.Equal(ResourceType.ARE, resource.Type);
        Assert.Equal([0x04, 0x05, 0x06], resource.Data);
    }
}
