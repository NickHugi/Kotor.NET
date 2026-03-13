using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Entities;

public class AnimatedModel : Entity
{
    public required string Model { get; set; }
    public List<AnimationItem> Animations { get; set; } = [];
    public Matrix4x4 Transformation { get; set; } = Matrix4x4.Identity;

    //    var objects = assetManager.GetModel(Model).Render(assetManager, Transformation, Animations);

    public override void Update(IAssetManager assetManager, float delta)
    {
        foreach (var animation in Animations.ToList())
        {
            if (!animation.Paused)
            {
                animation.CurrentTime += delta;

                var animationLength = assetManager.GetModel(Model).Animations.Single(x => x.Name == animation.Name).Length;
                animation.CurrentTime %= animationLength;
            }

            animation.BlendFactor -= animation.FadeFactor * delta;
            if (animation.BlendFactor <= 0)
            {
                Animations.Remove(animation);
            }
        }
    }
}
