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
    private readonly List<BaseEntity> _entities;
    public IReadOnlyList<BaseEntity> Entities { get; }

    public Scene()
    {
        _entities = new();
        Entities = _entities;
    }

    public T AddEntity<T>(T entity) where T : BaseEntity
    {
        entity.Scene = this;
        _entities.Add(entity);
        return entity;
    }

    public void Update(IAssetManager assetManager, float deltaTime)
    {
        _entities.ForEach(x => x.Update(assetManager, deltaTime));
    }
}
