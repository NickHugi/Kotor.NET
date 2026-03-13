using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model.Nodes;

public class MeshNode : DummyNode
{
    public IMesh Mesh { get; set; }
    public string Texture1 { get; set; } = "";
    public string Texture2 { get; set; } = "";

    public override ICollection<MeshDescriptor> GetMeshDescriptors(Entity entity)
    {
        return
        [
            new()
            {
                Mesh = Mesh,
                Texture1 = Texture1,
                Texture2 = Texture2,
                Transform = WorldTransformation * entity.Transformation,
                DoRender = Visible,
                DoShadow = false,
                BoneTransforms = Enumerable.Range(0, 16).Select(x => Matrix4x4.Identity).ToArray(),
                BoundingBox = null,
                BoundingSphere = null,
                EntityID = (uint)entity.ID,
            }
        ];
    }

    public override void Dispose()
    {
        Mesh.Dispose();
    }
}
