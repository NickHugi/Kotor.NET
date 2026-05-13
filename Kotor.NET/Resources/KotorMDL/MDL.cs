using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryMDL;
using Kotor.NET.Formats.BinaryMDL.Serialisation;
using Kotor.NET.Resources.KotorBWM;
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
    // todo - could have sworn I made a BoundingRadius(avgpoint, radius) class already 
    //public BoundingRadius BoundingRadius { get; set; } = new();
    [Obsolete]
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

    /// <summary>
    /// Resets all node indices and assigns them a unique number counting from 0.
    /// </summary>
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

    /// <summary>
    /// Deletes all walkmeshe nodes from the MDL hierarchy.
    /// </summary>
    public void DeleteWalkmesh()
    {
        var scan = new List<MDLNode>() { Root };
        MDLNode node;
        while (scan.Count > 0)
        {
            node = scan.First();
            scan.RemoveAt(0);

            node.Children.RemoveAll(x => x is MDLWalkmeshNode);
            scan.AddRange(node.Children);
        }
    }

    public MDLWalkmeshNode GetWalkmesh()
    {
        return Root.GetAllDescendants().FirstOrDefault(x => x is MDLWalkmeshNode) as MDLWalkmeshNode;
    }

    public void RecalculateBounds()
    {
        // todo
        throw new NotImplementedException();
    }

    public MDLNode[] GetPathToNode(MDLNode node)
    {
        return GetPathToNode([Root], node).ToArray();
    }
    private List<MDLNode>? GetPathToNode(List<MDLNode> from, MDLNode to)
    {
        if (from.Last() == to)
            return from;

        foreach (var child in from.Last().Children)
        {
            var path = GetPathToNode([.. from, child], to);
            if (path is not null)
                return path;
        }

        return null;
    }
}
