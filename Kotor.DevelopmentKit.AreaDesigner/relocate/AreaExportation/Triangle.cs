using System;
using System.Numerics;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;

public class Triangle
{
    public SurfaceMaterial Material { get; set; }
    public Vector3 Point1 { get; set; }
    public Vector3 Point2 { get; set; }
    public Vector3 Point3 { get; set; }

    public Vector3 Normal
    {
        get
        {
            // Calculate two edges of the triangle
            Vector3 edge1 = Point2 - Point1;
            Vector3 edge2 = Point3 - Point1;

            // Cross product gives a vector perpendicular to both edges
            Vector3 normal = Vector3.Cross(edge1, edge2);

            // Normalize to get a unit vector
            normal = Vector3.Normalize(normal);

            return normal;
        }
    }
    public Vector3 Centre => new Vector3
    {
        X = (Point1.X + Point2.X + Point3.X) / 3,
        Y = (Point1.Y + Point2.Y + Point3.Y) / 3,
    };
    public Triangle Squish => new Triangle()
    {
        Point1 = (Point1 * 0.5f) + (Centre * 0.5f),
        Point2 = (Point2 * 0.5f) + (Centre * 0.5f),
        Point3 = (Point3 * 0.5f) + (Centre * 0.5f),
    };

    public Triangle()
    {
        var rand = new Random();
    }
    public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Material = 0;
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
    }
    public Triangle(SurfaceMaterial color, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Material = color;
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
    }

    public Triangle S(int scale, int nudge)
    {
        return new(Material,
            (Point1 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point2 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point3 + new Vector3(nudge, nudge, nudge)) * scale);
    }
}

