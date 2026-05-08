using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Formats.BinaryBWM;
using Kotor.NET.Formats.BinaryBWM.Serialisation;
using Kotor.NET.Formats.BinaryERF.Serialisation;
using Kotor.NET.Resources.KotorBWM;
using Kotor.NET.Resources.KotorERF;
using Kotor.NET.Tools;

namespace Kotor.NET.Resources.KotorBWM;

public class BWM
{
    public BWMWalkmeshType WalkmeshType { get; set; }
    public Vector3 Position { get; set; }
    public FaceCollection Faces { get; }

    public BWM(BWMWalkmeshType walkmeshType)
    {
        WalkmeshType = walkmeshType;
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

    public static void ToFile(BWM bwm, string filepath)
    {
        using var stream = File.OpenWrite(filepath);
        new BWMBinarySerializer(bwm).Serialize().Write(stream);
    }
    public static byte[] ToBytes(BWM bwm)
    {
        using var stream = new MemoryStream();
        new BWMBinarySerializer(bwm).Serialize().Write(stream);
        return stream.ToArray();
    }
    public static void ToStream(BWM bwm, Stream stream)
    {
        new BWMBinarySerializer(bwm).Serialize().Write(stream);
    }

    public AABBNode GenerateTree()
    {
        return GenerateTree(new AABBTreeBuilder());
    }
    public AABBNode GenerateTree(IAABBTreeBuilder builder)
    {
        return builder.Build(Faces.ToList());
    }
}

public enum BWMWalkmeshType
{
    Area,
    Placeable
}
