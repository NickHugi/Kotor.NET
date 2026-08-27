using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.Geometry;

public class Triangle3D
{
    public Vector3 Point1 { get; set; }
    public Vector3 Point2 { get; set; }
    public Vector3 Point3 { get; set; }

    private Vector3? _normal;
    public Vector3 Normal
    {
        get => _normal.HasValue ? _normal.Value : (_normal = GetNormal()).Value;
    }

    private bool? _isDegenerate;
    public bool IsDegenerate
    {
        get => _isDegenerate.HasValue ? _isDegenerate.Value : (_isDegenerate = GetIsDegenerate()).Value;
    }

    public bool AreCoplanar(Triangle3D t1, Triangle3D t2, float epsilon = 1e-6f)
    {
        if (IsDegenerate)
            throw new ArgumentException("First triangle is degenerate.");

        var normal = Vector3.Normalize(Normal);

        return MathF.Abs(Vector3.Dot(normal, t2.Point1 - t1.Point1)) < epsilon &&
               MathF.Abs(Vector3.Dot(normal, t2.Point2 - t1.Point1)) < epsilon &&
               MathF.Abs(Vector3.Dot(normal, t2.Point3 - t1.Point1)) < epsilon;
    }

    private Vector3 GetNormal(float epsilon = 1e-6f)
    {
        var a = Point2 - Point1;
        var b = Point3 - Point1;
        return Vector3.Cross(a, b);
    }

    private bool GetIsDegenerate(float epsilon = 1e-6f)
    {
        return Normal.LengthSquared() < epsilon * epsilon;
    }
}
