using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Model;

public class SkinmeshNode : MeshNode
{
    public int[] BoneIndices { get; set; } = new int[16];

    public BaseNode? GetNodeFromBone(int index)
    {
        var nodeID = BoneIndices[index];
        var node = Model.Root.FindNode(nodeID);
        return node;
    }
}
