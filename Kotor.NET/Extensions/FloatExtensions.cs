using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Extensions;

public static class FloatExtensions
{
    public static bool Equals(this float a, float b, float tolerance)
    {
        var difference = Math.Abs(a - b);
        return difference < tolerance;
    }
}
