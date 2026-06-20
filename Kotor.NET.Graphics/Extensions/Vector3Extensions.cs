using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Graphics.Extensions;

public static class Vector3Extensions
{
    public static Vector3 Snap(this Vector3 value, Axis axis, float size = 0, float offset = 0)
    {
        if (size == 0)
            return value;

        if (axis == Axis.X)
        {
            var x = MathF.Round(value.X / size) * size + offset;
            return new(x, value.Y, value.Z);
        }
        else if (axis == Axis.Y)
        {
            var y = MathF.Round(value.Y / size) * size + offset;
            return new(value.X, y, value.Z);
        }
        else if (axis == Axis.Z)
        {
            var z = MathF.Round(value.Z / size) * size + offset;
            return new(value.X, value.Y, z);
        }
        else
        {
            return value;
        }
    }

    public static Color ToColor(this Vector3 value)
    {
        var red = (int)(value.X * 255);
        var green = (int)(value.Y * 255);
        var blue = (int)(value.Z * 255);
        return Color.FromArgb(red, green, blue);
    }

    public static float[] ToFloatArray(this Vector3 value)
    {
        return [value.X, value.Y, value.Z];
    }
}
