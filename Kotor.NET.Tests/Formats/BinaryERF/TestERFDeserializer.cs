using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryERF;
using Kotor.NET.Formats.BinaryERF.Serialisation;

namespace Kotor.NET.Tests.Formats.BinaryERF;

public class TestERFDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryERF/file1.erf";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new ERFBinary(stream);
        var deserializer = new ERFBinaryDeserializer(binary);
        var erf = deserializer.Deserialize();

        Assert.Equal(2, erf.Count());

        var key1 = binary.KeyEntries.ElementAt(1);
        Assert.Equal(key1.ResType, ResourceType.MDX.ID);
        Assert.Equal("test", key1.ResRef.Get());
    }
}
