using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryLIP;
using Kotor.NET.Resources.KotorLIP;

namespace Kotor.NET.Tests.Formats.BinaryLIP;

public class TestLIPDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryLIP/file1.lip";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new LIPBinary(stream);
        var deserializer = new LIPBinaryDeserializer(binary);
        var lip = deserializer.Deserialize();

        Assert.Equal(3, lip.Count());

        var entry1 = lip.ElementAt(1);
        Assert.Equal(0.77, entry1.Time, 0.1);
        Assert.Equal(LIPMouthShape.UH_UW_W, entry1.Shape);
    }
}
