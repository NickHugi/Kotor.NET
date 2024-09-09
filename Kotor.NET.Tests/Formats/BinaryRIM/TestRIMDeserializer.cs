using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryRIM;

namespace Kotor.NET.Tests.Formats.BinaryRIM;

public class TestRIMDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryRIM/file1.rim";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new RIMBinary(stream);
        var deserializer = new RIMBinaryDeserializer(binary);
        var rim = deserializer.Deserialize();

        Assert.Equal(2, rim.Count());

        var resource1 = binary.ResourceEntries.ElementAt(1);
        Assert.Equal(resource1.ResourceTypeID, ResourceType.MDX.ID);
        Assert.Equal("test", resource1.ResRef.Get());
    }
}
