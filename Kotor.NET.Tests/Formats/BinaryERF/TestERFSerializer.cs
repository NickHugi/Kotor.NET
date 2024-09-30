using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;
using Kotor.NET.Resources.KotorERF;

namespace Kotor.NET.Tests.Formats.BinaryERF;

public class TestERFSerializer
{
    public static readonly string File1Filepath = "Formats/BinaryERF/file1.erf";

    [Fact]
    public void Test_ReadFile1()
    {
        var erf = new ERF
        {
            { "ifo", ResourceType.IFO, [0x01, 0x02, 0x03] },
            { "are", ResourceType.ARE, [0x04, 0x05, 0x06] },
            { "git", ResourceType.GIT, [0x07, 0x08, 0x09] }
        };

        var serializer = new ERFBinarySerializer(erf);
        var binary = serializer.Serialize();
        using var stream = new MemoryStream();
        binary.Write(stream);

        stream.Position = 0;
        var binaryRead = new ERFBinary(stream);
        var deserializer = new ERFBinaryDeserializer(binaryRead);
        var erfRead = deserializer.Deserialize();

        Assert.Equal(3, erfRead.Count());

        var resource = erfRead.ElementAt(1);
        Assert.Equal("are", resource.ResRef);
        Assert.Equal(ResourceType.ARE, resource.Type);
        Assert.Equal([0x04, 0x05, 0x06], resource.Data);
    }
}
