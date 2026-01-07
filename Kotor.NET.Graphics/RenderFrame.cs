using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public class RenderFrame : IRenderFrame
{
    public IAssetManager AssetManager { get; }

    private List<IRenderObject> _objects = new();

    public RenderFrame(IAssetManager assetManager, List<IRenderObject> objects)
    {
        AssetManager = assetManager;
        _objects = objects;
    }

    public void Render()
    {
        var optimized = _objects.OrderBy(x => x.Shader.ID).ThenBy(x => x.Texture?.ID).ToList();

        foreach (var renderable in optimized)
        {
            renderable.Shader.SetMatrix4x4("model", renderable.Transformation);
            if (renderable.Texture is not null)
                renderable.Texture.Activate();
            renderable.VAO.Draw();
        }
    }

    public void AddObject(IRenderObject renderObject)
    {
        _objects.Add(renderObject);
    }
}
