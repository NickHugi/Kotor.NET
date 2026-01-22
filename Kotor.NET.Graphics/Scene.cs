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
    public List<IEntity> Entities { get; }

    public Scene(IAssetManager assetManager)
    {
        AssetManager = assetManager;
        Entities = new();
    }

    public void Render()
    {
        var frame = new RenderFrame(AssetManager, []);
        Entities.ForEach(x => x.Render(frame, AssetManager));
        frame.Render();
    }

    public void Update(float deltaTime)
    {

    }

    private IRenderFrame BuildRenderFrame()
    {
        //var renderObjects = Entities.SelectMany(model => BuildNestedRenderObject(model, Matrix4x4.Identity)).ToList();
        //return new RenderFrame(AssetManager, renderObjects);
        return null;
    }
    private List<IRenderObject> BuildNestedRenderObject(IModel model, Matrix4x4 parentTransformation)
    {
        return null;
    //    var shader = AssetManager.GetShader("default");
    //    var texture = AssetManager.GetTexture(model.Texture);
    //    var vao = model.VertexArrayObject;
    //    var transformation = parentTransformation * model.Transformation;
    //    var renderObject = new RenderObject(shader, texture, vao, transformation);
    //    return [renderObject, ..model.Children.SelectMany(model => BuildNestedRenderObject(model, Matrix4x4.Identity))];
    }
}
