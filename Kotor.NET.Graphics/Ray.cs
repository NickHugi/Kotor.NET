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

    public Vector3 FindPointOnPlane(Axis axis, float component)
    {
        float t;

        switch (axis)
        {
            case Axis.X:
                if (Direction.X == 0f) throw new ArgumentException("Direction X component cannot be zero.");
                t = (component - Origin.X) / Direction.X;
                return new Vector3(component, Origin.Y + t * Direction.Y, Origin.Z + t * Direction.Z);

            case Axis.Y:
                if (Direction.Y == 0f) throw new ArgumentException("Direction Y component cannot be zero.");
                t = (component - Origin.Y) / Direction.Y;
                return new Vector3(Origin.X + t * Direction.X, component, Origin.Z + t * Direction.Z);

            case Axis.Z:
                if (Direction.Z == 0f) throw new ArgumentException("Direction Z component cannot be zero.");
                t = (component - Origin.Z) / Direction.Z;
                return new Vector3(Origin.X + t * Direction.X, Origin.Y + t * Direction.Y, component);

            default:
                throw new ArgumentException("Invalid axis.");
        }
    }
}
