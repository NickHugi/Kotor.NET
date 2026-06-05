using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.Renderers;

public class LineRenderer : IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height)
    {
        var descriptors = scene.Entities
            .SelectMany(x => x.GetMeshDescriptors(assets))
            .OfType<LineDescriptor>()
            .ToList();

        var shader = assets.GetShader("line");
        shader.Activate();
        shader.SetMatrix4x4("uProjection", camera.GetProjectionTransform(width, height));
        shader.SetMatrix4x4("uView", camera.GetViewTransform());
        shader.SetUniform2("uViewport", new(width, height));

        assets.Line.Draw(descriptors);
    }
}
