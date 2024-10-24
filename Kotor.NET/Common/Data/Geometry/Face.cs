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
            var x = (Point1.Y * Point2.Z) - (Point1.Z * Point2.Y);
            var y = (Point1.Z * Point2.X) - (Point1.X * Point2.Z);
            var z = (Point1.X * Point2.Y) - (Point1.Y * Point2.X);
            return new(x, y, z);
        }
    }
    public float Distance
    {
        get
        {
            return -1 * Normal.Dot(Point1);
        }
    }

    internal readonly int[] _transition = new int[] { Edge.NoTransition, Edge.NoTransition, Edge.NoTransition };
    internal FaceCollection _collection = null;

    internal Face(FaceCollection collection)
    {
        _collection = collection;
    }

    public static BoundingBox BuildBuildingBoxFromFaces(List<Face> faces)
    {
        List<Vector3> points =
        [
            ..faces.Select(x => x.Point1),
            ..faces.Select(x => x.Point2),
            ..faces.Select(x => x.Point3),
        ];

        return new()
        {
            Min =
            {
                X = points.Min(point => point.X),
                Y = points.Min(point => point.Y),
                Z = points.Min(point => point.Z),
            },
            Max =
            {
                X = points.Min(point => point.X),
                Y = points.Min(point => point.Y),
                Z = points.Min(point => point.Z),
            }
        };
    }
}
