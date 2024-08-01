using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public override bool Equals(object? obj)
    {
        var vector3 = obj as Vector3;

        if (vector3 is null)
            return false;

        return vector3.X == X && vector3.Y == Y && vector3.Z == Z;
    }
}
