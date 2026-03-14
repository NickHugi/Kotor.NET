using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.Model;

namespace Kotor.NET.Graphics.Model.Nodes;

public class ActiveAnimation
{
    public string Name { get; }
    public float CurrentTime { get; set; }
    public bool Paused { get; set; }
    public float BlendFactor { get; set; }
    public float FadeFactor { get; set; }

    public ActiveAnimation(Animation animation)
    {
        Name = animation.Name;
    }
    public ActiveAnimation(string name)
    {
        Name = name;
    }
}
