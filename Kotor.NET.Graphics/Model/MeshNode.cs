using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model;

public class MeshNode : DummyNode
{
    public IVertexArrayObject Mesh { get; set; }
    public string Texture1 { get; set; } = "";
    public string Texture2 { get; set; } = "";

    public override void Render(IRenderFrame frame)
    {
        var shader = frame.AssetManager.GetShader("basic");

        var texture = (Texture1 == "NULL" || Texture1 == "")
            ? null
            : frame.AssetManager.GetTexture(Texture1);

        var renderObject = new RenderObject(shader, texture, Mesh, Transformation);
        frame.AddObject(renderObject);

        base.Render(frame);
    }
}
