using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Entities;

public class AnimatedModel : IEntity
{
    public string Model { get; set; }

    public string? Animation { get; set; }
    public float AnimationCurrentTime { get; set; }

    public string? AnimationFade { get; set; }
    public float AnimationFadeCurrentTime { get; set; }

    public Matrix4x4 Transformation { get; set; }

    public void Render(IRenderFrame frame, IAssetManager assetManager)
    {
        if (assetManager.HasModel(Model))
        {
            if (Animation is not null)
            {
                var objects = assetManager.GetModel(Model).Render(assetManager, Transformation, Animation, AnimationCurrentTime);
                objects.ToList().ForEach(frame.AddObject);
            }
            else
            {
                var objects = assetManager.GetModel(Model).Render(assetManager, Transformation);
                objects.ToList().ForEach(frame.AddObject);
            }
        }
    }

    public void Update(IAssetManager assetManager, float delta)
    {
        AnimationCurrentTime += delta;
        AnimationCurrentTime %= assetManager.GetModel(Model).Animations.Single(x => x.Name == Animation).Length;
    }
}
