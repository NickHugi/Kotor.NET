using System.Numerics;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Common.Data;

public class BoundingSphere
{
    public Vector3 Center { get; set; }
    public float Radius { get; set; }

    public BoundingSphere()
    {
        Center = new Vector3();
        Radius = 0.0f;
    }
    public BoundingSphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }
    public BoundingSphere(IEnumerable<Face> faces)
    {
        List<Vector3> points =
        [
            ..faces.Select(x => x.Point1),
            ..faces.Select(x => x.Point2),
            ..faces.Select(x => x.Point3),
        ];

        Center = new(points.Average(x => x.X), points.Average(x => x.Y), points.Average(x => x.Z));
        Radius = (float)Math.Sqrt(points.Max(x => Vector3.DistanceSquared(Center, x)));
    }
}
