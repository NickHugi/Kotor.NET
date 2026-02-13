using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Entities;

public class AnimationItem
{
    public string Name { get; }
    public float CurrentTime { get; set; }
    public bool Paused { get; set; }
    public float BlendFactor { get; set; }
    public bool Fading { get; set; }

    public AnimationItem(Animation animation)
    {
        Name = animation.Name;
    }
    public AnimationItem(string name)
    {
        Name = name;
    }

    public void Restart()
    {
        CurrentTime = 0;
    }

    public void FadeOut()
    {
        Paused = true;
        Fading = true;
        BlendFactor = 1.0f;
    }
}

// is it fading
// how fast is it fading
