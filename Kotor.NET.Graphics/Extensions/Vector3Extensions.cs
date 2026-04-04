using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Graphics.Extensions;

public static class Vector3Extensions
{
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
