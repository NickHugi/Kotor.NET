using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Formats.BinaryMDL.Serialisation;
using Kotor.NET.Resources.KotorMDL.Nodes;

namespace Kotor.NET.Resources.KotorMDL;

public class MDL
{
    public List<MDLAnimation> Animations = new();

    public MDLNode Root { get; set; } = new MDLNode("root");

    public byte ModelType { get; set; }
    public bool AffectedByFog { get; set; }
    public float AnimationScale { get; set; } = 1;
    public string SupermodelName { get; set; } = "NULL";
    public string Name { get; set; } = "model";

    public BoundingBox BoundingBox { get; set; } = new();
    public float Radius { get; set; }

    public static MDL FromFile(string mdlFilepath)
    {
        var mdxFilepath = Path.ChangeExtension(mdlFilepath, ".mdx");
        using var mdlStream = File.OpenRead(mdlFilepath);
        using var mdxStream = File.OpenRead(mdxFilepath);
        return FromStream(mdlStream, mdxStream);
    }
    public static MDL FromFile(string mdlFilepath, string mdxFilepath)
    {
        using var mdlStream = File.OpenRead(mdlFilepath);
        using var mdxStream = File.OpenRead(mdxFilepath);
        return FromStream(mdlStream, mdxStream);
    }
    public static MDL FromBytes(byte[] mdlBytes, byte[] mdxBytes)
    {
        using var mdlStream = new MemoryStream(mdlBytes);
        using var mdxStream = new MemoryStream(mdxBytes);
        return FromStream(mdlStream, mdxStream);
    }
    public static MDL FromStream(Stream mdlStream, Stream mdxStream)
    {
        var binary = new MDLBinary(mdlStream, mdxStream);
        var deserializer = new MDLBinaryDeserializer(binary);
        return deserializer.Deserialize();
    }

    public static void ToFile(MDL mdl, string mdlFilepath, GameEngine game, Platform platform)
    {
        var mdxFilepath = mdlFilepath.Replace(".mdl", ".mdx");
        using var mdlStream = File.OpenWrite(mdlFilepath);
        using var mdxStream = File.OpenWrite(mdxFilepath);
        ToStream(mdl, mdlStream, mdxStream, game, platform);
    }
    public static (byte[] MDL, byte[] MDX) ToBytes(MDL mdl, GameEngine game, Platform platform)
    {
        using var mdlStream = new MemoryStream();
        using var mdxStream = new MemoryStream();
        ToStream(mdl, mdlStream, mdxStream, game, platform);
        return (mdlStream.ToArray(), mdxStream.ToArray());
    }
    public static void ToStream(MDL mdl, Stream mdlStream, Stream mdxStream, GameEngine game, Platform platform)
    {
        var binary = new MDLBinarySerializer(mdl).Serialize(game, platform);
        binary.Write(mdlStream, mdxStream);
    }

    public void RedoNodeNumbers()
    {
        Root.NodeIndex = 0;

        var index = 1;
        Root.GetAllDescendants().ToList().ForEach(x =>
        {
            x.NodeIndex = (ushort)index;
            index++;
        });
    }
}
