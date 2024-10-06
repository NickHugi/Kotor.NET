using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLSkinNode : MDLTrimeshNode
{
    public int[] BoneIndices { get; } = new int[16];
    public List<MDLBone> BoneMap { get; set; }

    public MDLSkinNode(string name) : base(name)
    {
        BoneMap = new();
        AllVertices().ToList().ForEach(x => x._skin = new());
    }

    public override string ToString()
    {
        return $"MDLSkinNode '{Name}'";
    }
}
