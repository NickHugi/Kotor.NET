using System.Numerics;
using Kotor.NET.Common;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Resources.KotorBWM;

public class BWMFace : IFace
{
    public required Vector3 Point1 { get; set; }
    public required Vector3 Point2 { get; set; }
    public required Vector3 Point3 { get; set; }
    public required SurfaceMaterial Material { get; set; }

    public BWMEdge Edge1 => new(this, 0);
    public BWMEdge Edge2 => new(this, 1);
    public BWMEdge Edge3 => new(this, 2);

    public float PlaneDistance
    {
        get
        {
            return -1 * Vector3.Dot(Normal, Point1);
        }
    }
    public Vector3 Center
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

    internal readonly int[] _transition = new int[] { BWMEdge.NoTransition, BWMEdge.NoTransition, BWMEdge.NoTransition };
    internal BWMFaceCollection _collection = null;

    internal BWMFace(BWMFaceCollection collection)
    {
        _collection = collection;
    }
}
