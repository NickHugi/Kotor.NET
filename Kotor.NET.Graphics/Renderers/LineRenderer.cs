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

public class LineRenderer : IRenderer
{
    public void Render(IAssetManager assets, IEnumerable<IDrawCallDescriptor> descriptors, Camera camera, Vector2 viewport)
    {
        var shader = assets.GetShader("line");
        shader.Activate();
        shader.SetMatrix4x4("uProjection", camera.GetProjectionTransform((uint)viewport.X, (uint)viewport.Y));
        shader.SetMatrix4x4("uView", camera.GetViewTransform());
        shader.SetUniform2("uViewport", viewport);

        assets.Line.Draw(descriptors.OfType<LineDescriptor>());
    }
}
