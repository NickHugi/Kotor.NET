using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Interface;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Renderers;

public class ImageRenderer : IRenderer
{
    public void Render(IAssetManager assets, Scene scene, Camera camera, uint width, uint height, Action<List<MeshDescriptor>> renderInterceptor)
    {
        var descriptors = scene.Controls.SelectMany(x => x.GetImageDescriptors(assets)).ToList();

        var shader = assets.GetShader("image");

        shader.Activate();
        shader.SetUniform1("texture", 0);
        shader.SetUniform2("uScreenSize", new Vector2(width, height));

        descriptors.ForEach(x => Render(assets, shader, x));
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
