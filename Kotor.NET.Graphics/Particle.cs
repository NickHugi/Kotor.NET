using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public class Particle
{
    public Vector3 Position { get; set; }
    public Vector3 Velocity { get; set; }
    public float Life { get; set; }
}
