using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Kotor.NET.Formats.BinaryBWM;
using Kotor.NET.Formats.BinaryBWM.Serialisation;

namespace Kotor.NET.Tests.Formats.BinaryBWM;

public class TestBWMDeserializer
{
    public static readonly string File1Filepath = "Formats/BinaryBWM/file1.bwm";

    [Fact]
    public void Test_ReadFile1()
    {
        using var stream = File.OpenRead(File1Filepath);
        var binary = new BWMBinary(stream);
        var deserializer = new BWMBinaryDeserializer(binary);
        var bwm = deserializer.Deserialize();

        bwm.Faces.Count.Should().Be(4);
    }
}
