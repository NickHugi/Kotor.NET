using System.Numerics;

namespace Kotor.NET.Common.Data.Geometry;

public class Face
{
    public required Vector3 Point1 { get; set; }
    public required Vector3 Point2 { get; set; }
    public required Vector3 Point3 { get; set; }
    public required SurfaceMaterial Material { get; set; }

    public Edge Edge1 => new(this, 0);
    public Edge Edge2 => new(this, 1);
    public Edge Edge3 => new(this, 2);
    public Vector3 Centre
    {
        get
        {
            var x = (Point1.X + Point2.X + Point3.X) / 3;
            var y = (Point1.Y + Point2.Y + Point3.Y) / 3;
            var z = (Point1.Z + Point2.Z + Point3.Z) / 3;
            return new Vector3(x, y, z);
        }
    }
    public Vector3 Normal
    {
        get
        {
            return Vector3.Normalize(Vector3.Cross(Point2 - Point1, Point3 - Point1));
        }
    }
    public float Distance
    {
        get
        {
            return -1 * Vector3.Dot(Normal, Point1);
        }
    }

    internal readonly int[] _transition = new int[] { Edge.NoTransition, Edge.NoTransition, Edge.NoTransition };
    internal FaceCollection _collection = null;

    internal Face(FaceCollection collection)
    {
        _collection = collection;
    }
}
