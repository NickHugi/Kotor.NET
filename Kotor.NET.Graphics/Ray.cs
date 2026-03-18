using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics;

public class Ray
{
    public Vector3 Origin { get; set; }
    public Vector3 Direction { get; set; }

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    public float ShortestDistanceTo(Vector3 point)
    {
        Vector3 OP = point - Origin;

        float t = Vector3.Dot(OP, Direction);
        if (t < 0)
            return float.MaxValue;

        Vector3 closestPoint = Origin + t * Direction;

        float distance = (point - closestPoint).Length();
        return distance;
    }
}
