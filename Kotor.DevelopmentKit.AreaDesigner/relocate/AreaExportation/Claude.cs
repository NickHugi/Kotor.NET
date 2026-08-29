
namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.NET.Common.Data.Geometry;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Overlay.Snap;
using NetTopologySuite.Operation.OverlayNG;
using NetTopologySuite.Operation.Overlay.Snap;
using NetTopologySuite.Triangulate.Polygon;


public static class TriangleOverlay
{
    private sealed class ProjectedTriangle
    {
        public required Triangle Source { get; init; }
        public required Geometry Geometry { get; set; }
    }

    private sealed class PlaneGroup
    {
        public required PlaneFrame Frame { get; init; }
        public List<Triangle> Triangles { get; } = new();
    }

    /// <summary>
    /// A local 2D coordinate system embedded in a 3D plane.
    /// </summary>
    private readonly record struct PlaneFrame(
        Vector3 Origin,
        Vector3 Normal,
        Vector3 U,
        Vector3 V)
    {
        public static PlaneFrame FromTriangle(Triangle triangle)
        {
            Vector3 normal = Vector3.Normalize(Vector3.Cross(
                triangle.Point2 - triangle.Point1,
                triangle.Point3 - triangle.Point1));

            // Pick a helper axis that is not nearly parallel to the normal.
            Vector3 helper = MathF.Abs(normal.Z) < 0.9f
                ? Vector3.UnitZ
                : Vector3.UnitY;

            Vector3 u = Vector3.Normalize(Vector3.Cross(helper, normal));
            Vector3 v = Vector3.Cross(normal, u);

            return new PlaneFrame(
                triangle.Point1,
                normal,
                u,
                v);
        }

        public Coordinate Project(Vector3 point, double snapTolerance)
        {
            Vector3 offset = point - Origin;

            double x = Vector3.Dot(offset, U);
            double y = Vector3.Dot(offset, V);

            // Optional snapping for almost-identical boundaries.
            if (snapTolerance > 0)
            {
                x = Math.Round(x / snapTolerance) * snapTolerance;
                y = Math.Round(y / snapTolerance) * snapTolerance;
            }

            return new Coordinate(x, y);
        }

        public Vector3 Lift(Coordinate point)
        {
            return Origin
                + U * (float)point.X
                + V * (float)point.Y;
        }

        public float DistanceTo(Vector3 point)
        {
            return MathF.Abs(Vector3.Dot(Normal, point - Origin));
        }
    }

    /// <summary>
    /// Part (a): groups triangles whose supporting planes are equal within
    /// the supplied angular and positional tolerances.
    /// </summary>
    public static List<List<Triangle>> GroupByPlane(
        IReadOnlyList<Triangle> triangles,
        float angleToleranceDegrees = 0.1f,
        float planeDistanceTolerance = 0.0001f)
    {
        return BuildGroups(
                triangles,
                angleToleranceDegrees,
                planeDistanceTolerance)
            .Select(group => group.Triangles)
            .ToList();
    }

    public static List<Triangle> RemoveCoplanarOverlaps(
        IReadOnlyList<Triangle> triangles,
        float angleToleranceDegrees = 0.1f,
        float planeDistanceTolerance = 0.0001f,
        double joinTolerance = 0.001,
        double overlaySnapTolerance = 0.0)
    {
        if (joinTolerance < 0)
            throw new ArgumentOutOfRangeException(nameof(joinTolerance));

        List<PlaneGroup> groups = BuildGroups(
            triangles,
            angleToleranceDegrees,
            planeDistanceTolerance);

        var output = new List<Triangle>();
        var geometryFactory = new GeometryFactory();

        foreach (PlaneGroup group in groups)
        {
            // Project every triangle before doing any overlay work.
            var projected = group.Triangles
                .Select(source => new ProjectedTriangle
                {
                    Source = source,
                    Geometry = ToPolygon(
                        source,
                        group.Frame,
                        geometryFactory,
                        overlaySnapTolerance)
                })
                .ToList();

            // Move nearby boundaries onto common coordinates.
            SnapNearbyPolygons(projected, joinTolerance);

            Geometry? alreadyCovered = null;

            foreach (ProjectedTriangle item in projected)
            {
                Geometry polygon = item.Geometry;

                Geometry visiblePart = alreadyCovered is null
                    ? polygon
                    : OverlayNGRobust.Overlay(
                        polygon,
                        alreadyCovered,
                        OverlayNG.DIFFERENCE);

                AppendTriangulation(
                    visiblePart,
                    item.Source.Material,
                    group.Frame,
                    output);

                alreadyCovered = alreadyCovered is null
                    ? polygon
                    : OverlayNGRobust.Overlay(
                        alreadyCovered,
                        polygon,
                        OverlayNG.UNION);
            }
        }

        return output;
    }

    private static List<PlaneGroup> BuildGroups(
        IReadOnlyList<Triangle> triangles,
        float angleToleranceDegrees,
        float planeDistanceTolerance)
    {
        if (angleToleranceDegrees < 0 || angleToleranceDegrees >= 90)
        {
            throw new ArgumentOutOfRangeException(
                nameof(angleToleranceDegrees));
        }

        if (planeDistanceTolerance < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(planeDistanceTolerance));
        }

        float minimumNormalDot = MathF.Cos(
            angleToleranceDegrees * MathF.PI / 180f);

        var groups = new List<PlaneGroup>();

        foreach (Triangle triangle in triangles)
        {
            Vector3 cross = Vector3.Cross(
                triangle.Point2 - triangle.Point1,
                triangle.Point3 - triangle.Point1);

            if (cross.LengthSquared() <= 1e-20f)
            {
                throw new ArgumentException(
                    "A degenerate triangle does not define a plane.",
                    nameof(triangles));
            }

            Vector3 normal = Vector3.Normalize(cross);

            PlaneGroup? match = groups.FirstOrDefault(group =>
                // Abs allows triangles with opposite winding.
                MathF.Abs(Vector3.Dot(
                    normal,
                    group.Frame.Normal)) >= minimumNormalDot &&

                group.Frame.DistanceTo(triangle.Point1)
                    <= planeDistanceTolerance &&

                group.Frame.DistanceTo(triangle.Point2)
                    <= planeDistanceTolerance &&

                group.Frame.DistanceTo(triangle.Point3)
                    <= planeDistanceTolerance);

            if (match is null)
            {
                match = new PlaneGroup
                {
                    Frame = PlaneFrame.FromTriangle(triangle)
                };

                groups.Add(match);
            }

            match.Triangles.Add(triangle);
        }

        return groups;
    }

    private static Polygon ToPolygon(
        Triangle triangle,
        PlaneFrame frame,
        GeometryFactory factory,
        double snapTolerance)
    {
        Coordinate a = frame.Project(
            triangle.Point1,
            snapTolerance);

        Coordinate b = frame.Project(
            triangle.Point2,
            snapTolerance);

        Coordinate c = frame.Project(
            triangle.Point3,
            snapTolerance);

        // NetTopologySuite polygon rings must be closed.
        return factory.CreatePolygon(new[]
        {
            a,
            b,
            c,
            new Coordinate(a.X, a.Y)
        });
    }

    private static void AppendTriangulation(
        Geometry polygonalGeometry,
        SurfaceMaterial material,
        PlaneFrame frame,
        List<Triangle> destination)
    {
        if (polygonalGeometry.IsEmpty ||
            polygonalGeometry.Area <= 0)
        {
            return;
        }

        Geometry triangulation =
            PolygonTriangulator.Triangulate(polygonalGeometry);

        for (int i = 0; i < triangulation.NumGeometries; i++)
        {
            Coordinate[] coordinates =
                triangulation.GetGeometryN(i).Coordinates;

            // A triangle polygon normally has four coordinates:
            // A, B, C, and A again to close the ring.
            if (coordinates.Length < 4)
                continue;

            Vector3 a = frame.Lift(coordinates[0]);
            Vector3 b = frame.Lift(coordinates[1]);
            Vector3 c = frame.Lift(coordinates[2]);

            // Give all generated triangles the group's winding direction.
            if (Vector3.Dot(
                    Vector3.Cross(b - a, c - a),
                    frame.Normal) < 0)
            {
                (b, c) = (c, b);
            }

            destination.Add(new Triangle(material, a, b, c));
        }
    }

    private static void SnapNearbyPolygons(
        List<ProjectedTriangle> triangles,
        double joinTolerance)
    {
        if (joinTolerance <= 0 || triangles.Count < 2)
            return;

        // More than one pass allows snapping changes to propagate through
        // chains of triangles: A touches B, and B touches C.
        const int passCount = 2;

        for (int pass = 0; pass < passCount; pass++)
        {
            for (int i = 0; i < triangles.Count - 1; i++)
            {
                for (int j = i + 1; j < triangles.Count; j++)
                {
                    Geometry first = triangles[i].Geometry;
                    Geometry second = triangles[j].Geometry;

                    // Avoid snapping unrelated triangles on the same plane.
                    if (first.Distance(second) > joinTolerance)
                        continue;

                    Geometry[] snapped = GeometrySnapper.Snap(
                        first,
                        second,
                        joinTolerance);

                    triangles[i].Geometry = snapped[0];
                    triangles[j].Geometry = snapped[1];
                }
            }
        }
    }
}
