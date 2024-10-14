namespace Kotor.NET.Common.Data.Geometry;

public class Face
{
    public required Vector3 Point1 { get; set; }
    public required Vector3 Point2 { get; set; }
    public required Vector3 Point3 { get; set; }
    public required SurfaceMaterial Material { get; set; }

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

