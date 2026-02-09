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
    public List<IEntity> Entities { get; }

    public Scene()
    {
        Entities = new();
    }

    public void Render(IAssetManager assetManager)
    {
        var frame = new RenderFrame([]);
        Entities.ForEach(x => x.Render(frame, assetManager, "cwalk", 0.0f));
        frame.Render(assetManager);
    }

    public void Update(float deltaTime)
    {
        Entities.ForEach(x => x.Update(deltaTime));
    }
}
