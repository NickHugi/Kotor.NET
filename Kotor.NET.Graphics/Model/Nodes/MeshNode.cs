using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model.Nodes;

public class MeshNode : DummyNode
{
    public IVertexArrayObject Mesh { get; set; }
    public string Texture1 { get; set; } = "";
    public string Texture2 { get; set; } = "";

    public override ICollection<RenderObject> Render(IAssetManager assetManager, Matrix4x4 entityTransform)
    {
        if (!Visible)
            return [];

        var shader = assetManager.GetShader("basic");

        var texture = (Texture1 == "NULL" || Texture1 == "")
            ? null
            : assetManager.GetTexture(Texture1);

        var renderObject = new RenderObject(shader, texture, Mesh, WorldTransformation, entityTransform);
        return [renderObject];
    }

    public override void Dispose()
    {
        Mesh.Dispose();
    }
}
