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
    public void Render(IAssetManager assets, IEnumerable<IDrawCallDescriptor> descriptors, Camera camera, Vector2 viewport)
    {
        var shader = assets.GetShader("billboard");

        shader.Activate();
        shader.SetMatrix4x4("uProjection", camera.GetProjectionTransform((uint)viewport.X, (uint)viewport.Y));
        shader.SetMatrix4x4("uView", camera.GetViewTransform());
        shader.SetUniform2("uViewport", viewport);
        shader.SetUniform1("uTexture", 0);

        descriptors.OfType<BillboardDescriptor>().ToList().ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, BillboardDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetUniform3("uPosition", descriptor.Location);
        shader.SetUniform1("uSize", descriptor.Size);
        shader.SetUniform1("uFixedSize", descriptor.FixedSize);
        shader.SetDepthTest(!descriptor.AllwaysOnTop);

        var texturePlaceholder = assets.GetTexture("placeholder");
        var texture = assets.GetTexture(descriptor.Image);
        if (texture is null) texturePlaceholder.Activate(0); else texture.Activate(0);

        assets.Billboard.Draw();
    }
}
