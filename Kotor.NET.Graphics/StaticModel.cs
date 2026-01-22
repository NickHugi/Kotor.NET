using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics;

public class StaticModel : IEntity
{
    public string Model { get; set; }
    public Matrix4x4 Transformation { get; set; }

    public void Render(IRenderFrame frame, IAssetManager assetManager)
    {
        if (assetManager.HasModel(Model))
        {
            var objects = assetManager.GetModel(Model).Render(assetManager, Transformation);
            objects.ToList().ForEach(frame.AddObject);
        }
    }

    public void Update(float delta)
    {

    }
}
