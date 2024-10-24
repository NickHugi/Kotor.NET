using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Formats.BinaryBWM.Serialisation;
using Kotor.NET.Formats.BinaryBWM;
using Kotor.NET.Resources.KotorBWM;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Resources.KotorBWM;

public class BWM
{
    public FaceCollection Faces { get; set; }

    public BWM()
    {
        Faces = new();
    }
    public static BWM FromFile(string filepath)
    {
        using var stream = File.OpenRead(filepath);
        return FromStream(stream);
    }
    public static BWM FromBytes(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        return FromStream(stream);
    }
    public static BWM FromStream(Stream stream)
    {
        var binary = new BWMBinary(stream);
        var deserializer = new BWMBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }
}
