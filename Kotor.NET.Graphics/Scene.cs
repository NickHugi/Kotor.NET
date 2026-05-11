using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.Interface;

namespace Kotor.NET.Graphics;

public class Scene
{
    private readonly List<BaseEntity> _entities;
    public IReadOnlyList<BaseEntity> Entities { get; }

    private readonly List<BaseControl> _controls;
    public IReadOnlyList<BaseControl> Controls { get; }

    public Scene()
    {
        _entities = new();
        Entities = _entities;

        _controls = new();
        Controls = _controls;
    }

    public T AddEntity<T>(T entity) where T : BaseEntity
    {
        entity.Scene = this;
        _entities.Add(entity);
        return entity;
    }

    public T AddControl<T>(T control) where T : BaseControl
    {
        control.Scene = this;
        _controls.Add(control);
        return control;
    }

    public void Update(IAssetManager assetManager, float deltaTime)
    {
        _entities.ForEach(x => x.Update(assetManager, deltaTime));
    }
}
