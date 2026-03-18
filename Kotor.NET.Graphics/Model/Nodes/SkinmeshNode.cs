using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
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

    public override ICollection<MeshDescriptor> GetMeshDescriptors(Matrix4x4 transform)
    {
        return
        [
            new()
            {
                Mesh = Mesh,
                Texture1 = Texture1,
                Texture2 = Texture2,
                Transform = WorldTransformation * transform,
                DoRender = Visible,
                DoShadow = false,
                BoneTransforms = CalculateBoneTransforms(),
                BoundingBox = null,
                BoundingSphere = null,
                PickerID = 0xFFFFFFFF // TODO... (uint)entity.ID,
            }
        ];
    }
    private Matrix4x4[] CalculateBoneTransforms()
    {
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
        return finalBoneMatrices;
    }

    public override void Dispose()
    {
        Mesh.Dispose();
    }
}
