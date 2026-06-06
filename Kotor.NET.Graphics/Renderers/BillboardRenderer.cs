using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.Renderers;

public class BillboardRenderer : IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height)
    {
        var descriptors = scene.Entities
            .SelectMany(x => x.GetMeshDescriptors(assets))
            .OfType<BillboardDescriptor>()
            .ToList();

        var shader = assets.GetShader("billboard");

        shader.Activate();
        shader.SetMatrix4x4("uProjection", camera.GetProjectionTransform(width, height));
        shader.SetMatrix4x4("uView", camera.GetViewTransform());
        shader.SetUniform2("uViewport", new(width, height));
        shader.SetUniform1("uTexture", 0);

        descriptors.ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, BillboardDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetUniform3("uPosition", descriptor.Location);
        shader.SetUniform1("uSize", descriptor.Size);
        shader.SetUniform1("uFixedSize", descriptor.FixedSize);

        assets.Quad.Draw();
    }
}
