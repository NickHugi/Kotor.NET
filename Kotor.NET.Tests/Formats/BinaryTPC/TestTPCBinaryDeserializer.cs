using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Kotor.NET.Formats.BinaryTPC;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Resources.KotorTPC.TextureFormats;

namespace Kotor.NET.Tests.Formats.BinaryTPC;

public class TestTPCBinaryDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryTPC/file1.tpc";

    [Fact(DisplayName="Compressed RGBA (DXT5)")]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new TPCBinary(stream);
        var deserializer = new TPCBinaryDeserializer(binary);
        var tpc = deserializer.Deserialize();

        using (var scope = new AssertionScope())
        {
            tpc.LayerCount.Should().Be(1);
            tpc.MipmapCount.Should().Be(6);
            tpc.Width.Should().Be(32);
            tpc.Height.Should().Be(32);
            tpc.TextureFormat.Should().BeSameAs(TPCTextureFormat.DXT5);
        }
    }
}
