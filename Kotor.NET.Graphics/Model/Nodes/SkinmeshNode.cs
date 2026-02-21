using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model.Nodes;

public class SkinmeshNode : MeshNode
{
    public int[] BoneIndices { get; set; } = new int[16];
    public int[] Bonemap { get; set; }
    public Vector3[] TBones { get; set; }
    public Quaternion[] QBones { get; set; }

    public SkinmeshNode()
    {

    }

    public BaseNode? GetNodeFromBone(int index)
    {
        var nodeID = BoneIndices[index];
        var node = Model.Root.FindNode(nodeID);
        return node;
    }

    public override ICollection<RenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform)
    {
        if (!Visible)
            return [];

        var shader = assetManager.GetShader("basic");

        var texture = Texture1 == "NULL" || Texture1 == ""
            ? null
            : assetManager.GetTexture(Texture1);

        var finalBoneMatrices = Enumerable.Repeat(Matrix4x4.Identity, 16).ToArray();
        for (int i = 0; i < 16; i++)
        {
            var bone = GetNodeFromBone(i);
            if (bone == null)
                break;
            if (bone.NodeID == 0)
                break;

            finalBoneMatrices[i] = bone.InverseBindMatrix * bone.WorldTransformation;
        }

        var renderObject = new RenderObject(shader, texture, Mesh, WorldTransformation, entityTransform, finalBoneMatrices);
        return [renderObject];
    }

    public override void Dispose()
    {
        Mesh.Dispose();
    }
}
