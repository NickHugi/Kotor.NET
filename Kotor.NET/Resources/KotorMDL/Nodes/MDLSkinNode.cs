using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Resources.KotorMDL.Nodes;

public class MDLSkinNode : MDLTrimeshNode
{
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
