using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Entities;

public abstract class Entity
{
    public Scene Scene { get; internal set; }
    public int ID => Scene.Entities.ToList().IndexOf(this);

    public abstract void Render(RenderFrame frame, IAssetManager assetManager);

    public abstract void Update(IAssetManager assetManager, float delta);
}
