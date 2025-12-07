using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Model;

public class MeshNode : BaseNode
{
    public required string Mesh { get; set; }
    public required string Texture1 { get; set; }
    public required string Texture2 { get; set; }

    public override void Render(IRenderFrame frame)
    {
        var shader = IAssetManager.Manager.GetShader("basic");
        ITexture texture = null;
        var mesh = IAssetManager.Manager.GetVAO(Mesh);

        var renderObject = new RenderObject(shader, texture, mesh, Matrix4x4.Identity);
        frame.AddObject(renderObject);

        base.Render(frame);
    }
}
