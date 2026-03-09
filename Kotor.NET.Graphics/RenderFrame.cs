using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public class RenderFrame
{
    private List<RenderObject> _objects = new();
    public IReadOnlyList<RenderObject> Objects => new List<RenderObject>(_objects);

    public void Render(IAssetManager assetManager)
    {
        var optimized = _objects.OrderBy(x => x.Shader.ID).ThenBy(x => x.Texture?.ID).ToList();

        foreach (var renderable in optimized)
        {
            renderable.Shader.SetMatrix4x4("entity", renderable.EntityTransform);
            renderable.Shader.SetMatrix4x4("mesh", renderable.ModelTransform);
            renderable.Shader.SetMatrix4x4Array("finalBonesMatrices", renderable.FinalBoneMatrices);
            renderable.Shader.SetUniform1("entityID", renderable.EntityID);

            if (renderable.Texture is not null)
            {
                renderable.Texture.Activate();
            }
            else
            {
                assetManager.GetTexture("placeholder").Activate();
            }

            renderable.VAO.Draw();
        }
    }

    public void AddObject(RenderObject renderObject)
    {
        _objects.Add(renderObject);
    }
}
