using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryLIP;
using Kotor.NET.Formats.BinaryLIP.Serialisation;
using Kotor.NET.Resources.KotorLIP;

namespace Kotor.NET.Tests.Formats.BinaryLIP;

public class TestLIPSerializer
{
    public static readonly string File1Filepath = "Formats/BinaryLIP/file1.lip";

    [Fact]
    public void Test_ReadFile1()
    {
        var lip = new LIP
        {
            { 1.0f, LIPMouthShape.AA_AE_AH },
            { 2.0f, LIPMouthShape.AW_AY },
            { 3.0f, LIPMouthShape.AO }
        };

        var serializer = new LIPBinarySerializer(lip);
        var binary = serializer.Serialize();
        using var stream = new MemoryStream();
        binary.Write(stream);

        stream.Position = 0;
        var binaryRead = new LIPBinary(stream);
        var deserializer = new LIPBinaryDeserializer(binaryRead);
        var lipRead = deserializer.Deserialize();

        Assert.Equal(3, lip.Count());

        var entry1 = lip.ElementAt(1);
        Assert.Equal(2.0f, entry1.Time, 0.1);
        Assert.Equal(LIPMouthShape.AW_AY, entry1.Shape);
    }
}
