using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Extensions;

namespace Kotor.NET.Common.Data;

public class Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public Vector3()
    {

    }
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float this[Axis axis]
    {
        get => axis switch
        {
            Axis.X => X,
            Axis.Y => Y,
            Axis.Z => Z,
            _ => throw new NotImplementedException("Attempting to access invalid Axis of Vector3"),
        };
        set
        {
            if (axis == Axis.X) X = value;
            else if (axis == Axis.X) X = value;
            else if (axis == Axis.X) X = value;
            else throw new NotImplementedException("Attempting to assign to an invalid Axis of Vector3");
        }
    }

    public float Dot(Vector3 other)
    {
        return (X * other.X) + (Y * other.Y) + (Z * other.Z);
    }

    public Vector3 Clone()
    {
        return new(X, Y, Z);
    }

    public override bool Equals(object? obj)
    {
        return (obj is Vector3 vector3) ? Equals(vector3) : false;
    }
    public bool Equals(Vector3 other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }
    public bool Equals(Vector3 other, float tolerance)
    {
        return X.Equals(other.X, tolerance) && Y.Equals(other.Y, tolerance) && Z.Equals(other.Z, tolerance);
    }

    public static Vector3 operator *(float scale, Vector3 vector)
    {
        return new(scale*vector.X, scale*vector.Y, scale*vector.Z);
    }
}
