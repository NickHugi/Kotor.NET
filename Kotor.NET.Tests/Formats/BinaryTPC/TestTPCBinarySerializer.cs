using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Formats.BinaryTPC;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Resources.KotorTPC;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Tests.Formats.BinaryTPC;

public class TestTPCBinarySerializer
{
    public static readonly string File1Filepath = "Formats/BinaryTPC/file1.tpc";

    [Fact]
    public void Serialize()
    {
        var tpc = new TPC(32, 32, 1, 6, TPCTextureFormat.Grayscale);
        var serializer = new TPCBinarySerializer(tpc);
        var binary = serializer.Serialize();
        using var stream = new MemoryStream();
        binary.Write(stream);

        stream.Position = 0;
        var binaryRead = new TPCBinary(stream);
        var deserializer = new TPCBinaryDeserializer(binaryRead);
        var tpcRead = deserializer.Deserialize();

        using (var scope = new AssertionScope())
        {
            tpcRead.LayerCount.Should().Be(1);
            tpcRead.MipmapCount.Should().Be(6);
            tpcRead.Width.Should().Be(32);
            tpcRead.Height.Should().Be(32);
            tpcRead.TextureFormat.Should().BeSameAs(TPCTextureFormat.Grayscale);
        }
    }
}
