using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryLIP.Serialisation;
using Kotor.NET.Formats.BinaryLIP;
using Kotor.NET.Formats.AsciiTXI;
using Kotor.NET.Formats.AsciiTXI.Serialisation;

namespace Kotor.NET.Resources.KotorTXI;

public class TXI
{
    public TXI()
    {
    }
    public static TXI FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static TXI FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static TXI FromStream(Stream stream)
    {
        var binary = new TXIAscii(stream);
        var deserializer = new TXIAsciiDeserializer(binary);
        return deserializer.Deserialize();
    }

    public TXIMaterial Material { get; } = new();
    public TXITexture Texture { get; } = new();
    public TXIBumpmap Bumpmap { get; } = new();
    public TXIProcedure Procedure { get; } = new();
    public TXIFont Font { get; } = new();
}
