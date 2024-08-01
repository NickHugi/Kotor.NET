using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data;

public class Vector2
{
    public float X { get; set; }
    public float Y { get; set; }

    public Vector2()
    {

    }

    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj)
    {
        var vector2 = obj as Vector2;

        if (vector2 is null)
            return false;

        return vector2.X == X && vector2.Y == Y;
    }
}
