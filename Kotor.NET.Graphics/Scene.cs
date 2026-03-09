using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public class Scene
{
    private readonly List<Entity> _entities;
    public IReadOnlyList<Entity> Entities { get; }

    public Scene()
    {
        _entities = new();
        Entities = _entities;
    }

    public T AddEntity<T>(T entity) where T : Entity
    {
        entity.Scene = this;
        _entities.Add(entity);
        return entity;
    }

    public void Render(IAssetManager assetManager)
    {
        var frame = new RenderFrame();
        _entities.ForEach(x => x.Render(frame, assetManager));
        frame.Render(assetManager);
    }

    public void PickRender(IAssetManager assetManager)
    {
        var frame = new RenderFrame();
        _entities.ForEach(x => x.Render(frame, assetManager));
        frame.Objects.ToList().ForEach(x => x.Shader = assetManager.GetShader("picker"));
        frame.Render(assetManager);
    }

    public void Update(IAssetManager assetManager, float deltaTime)
    {
        _entities.ForEach(x => x.Update(assetManager, deltaTime));
    }
}
