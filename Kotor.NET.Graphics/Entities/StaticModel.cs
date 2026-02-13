using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.GPU;

namespace Kotor.NET.Graphics.Entities;

public class StaticModel : IEntity
{
    public string Model { get; set; }
    public Matrix4x4 Transformation { get; set; } = Matrix4x4.Identity;

    public void Render(RenderFrame frame, IAssetManager assetManager)
    {
        if (assetManager.HasModel(Model))
        {
            var objects = assetManager.GetModel(Model).Render(assetManager, Transformation, []);
            objects.ToList().ForEach(frame.AddObject);
        }
    }

    public void Update(IAssetManager assetManager, float delta)
    {

    }
}
