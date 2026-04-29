using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Extensions;

public static class Vector3Extensions
{
    /// <summary>
    /// Get the scalar value for a given axis on the vector.
    /// </summary>
    public static float GetComponent(this Vector3 vector, Axis axis)
    {
        return axis switch
        {
            Axis.X => vector.X,
            Axis.Y => vector.Y,
            Axis.Z => vector.Z,
            _ => throw new InvalidOperationException("Tried to get component using invalid axis.")
        };
    }
}
