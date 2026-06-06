using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.Renderers.Descriptors;

namespace Kotor.NET.Graphics.Renderers;

public class ImageRenderer : IRenderer
{
    public void Render(IAssetManager assets, IEnumerable<IDrawCallDescriptor> descriptors, Camera camera, Vector2 viewport)
    {
        var shader = assets.GetShader("image");

        shader.Activate();
        shader.SetUniform1("uTexture", 0);
        shader.SetUniform2("uViewport", viewport);

        descriptors.OfType<ImageDescriptor>().ToList().ForEach(x => Render(assets, shader, x));
    }

    private void Render(IAssetManager assets, IShader shader, ImageDescriptor descriptor)
    {
        if (!descriptor.DoRender)
            return;

        shader.SetUniform2("uPosition", new Vector2(descriptor.X, descriptor.Y));
        shader.SetUniform2("uSize", new Vector2(descriptor.Width, descriptor.Height));

        var texturePlaceholder = assets.GetTexture("placeholder");
        var texture = assets.GetTexture(descriptor.Image);
        if (texture is null) texturePlaceholder.Activate(0); else texture.Activate(0);

        assets.Quad.Draw();
    }
}
