using System.Numerics;

namespace Kotor.NET.Common.Data.Geometry;

public enum Axis
{
    X,
    Y,
    Z,
}

public static class AxisExtensions
{
    public static Vector3 GetUnitVector(this Axis axis)
    {
        return axis switch
        {
            Axis.X => Vector3.UnitX,
            Axis.Y => Vector3.UnitY,
            Axis.Z => Vector3.UnitZ,
        };
    }
}
