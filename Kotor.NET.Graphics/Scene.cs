using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public class Scene
{
    public IAssetManager AssetManager { get; }
    public List<IModel> Models { get; }

    public Scene(IAssetManager assetManager)
    {
        AssetManager = assetManager;
        Models = new();
    }

    public void Render()
    {
        var frame = BuildRenderFrame();
        frame.Render();
    }

    private IRenderFrame BuildRenderFrame()
    {
        var renderObjects = Models.SelectMany(model => BuildNestedRenderObject(model, Matrix4x4.Identity)).ToList();
        return new RenderFrame(renderObjects); ;
    }
    private List<IRenderObject> BuildNestedRenderObject(IModel model, Matrix4x4 parentTransformation)
    {
        var shader = AssetManager.GetShader("default");
        var texture = AssetManager.GetTexture(model.Texture);
        var vao = model.VertexArrayObject;
        var transformation = parentTransformation * model.Transformation;
        var renderObject = new RenderObject(shader, texture, vao, transformation);
        return [renderObject, ..model.Children.SelectMany(model => BuildNestedRenderObject(model, Matrix4x4.Identity))];
    }
}
