using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Clipper2Lib;
using DynamicData;
using Kotor.DevelopmentKit.AreaDesigner.Geometry;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Tools;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;

public static class AreaExporter
{
    public static MDL RoomToMDL(Room room)
    {
        var mdl = new MDL();
        mdl.Name = "test";

        //var xyz = room.AllObjects.ElementAt(1);
        //return MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{xyz.KitID}/{xyz.Template.Model}.mdl");

        mdl.Root.Children.AddRange(room.AllObjects.Select(WorldObjectToMDLNode));

        var walkmeshes = mdl.Root.GetAllDescendants().OfType<MDLWalkmeshNode>();
        var newWalkmesh = MergeWalkmeshes(walkmeshes.ToList());
        MergeVerticesByDistance(newWalkmesh, 0.1f);
        mdl.DeleteWalkmesh();
        newWalkmesh.RootNode = new AABBTreeBuilder().Build(newWalkmesh.Faces.OfType<IFace>().ToList());
        mdl.Root.Children.Add(newWalkmesh);

        mdl.Root.GetAllDescendants().OfType<MDLTrimeshNode>().ToList().ForEach(x => x.LightmapTexture = "");
        mdl.Root.GetAllDescendants().Select((x, i) => x.Name = i.ToString()).ToArray();
        newWalkmesh.Name = "walkmesh";
        mdl.RedoNodeNumbers();

        return mdl;
    }
    private static void DeleteWalkmeshesRecursive(MDLNode node)
    {
        foreach (var child in node.Children.ToArray())
        {
            if (child is MDLWalkmeshNode)
                node.Children.Remove(child);
            else
                DeleteWalkmeshesRecursive(child);
        }
    }

    private static MDLWalkmeshNode MergeWalkmeshes(List<MDLWalkmeshNode> walkmeshes)
    {
        var final = new MDLWalkmeshNode("walkmesh");
        final.EnableVertices();
        final.EnableNormals();

        foreach (var walkmesh in walkmeshes)
        {
            foreach (var face in walkmesh.Faces)
            {
                final.Faces.Add(new MDLFace()
                {
                    Vertex1 = new MDLVertex().SetPosition(face.Point1).SetNormal(Vector3.One),
                    Vertex2 = new MDLVertex().SetPosition(face.Point2).SetNormal(Vector3.One),
                    Vertex3 = new MDLVertex().SetPosition(face.Point3).SetNormal(Vector3.One)
                });
                final.Faces.Last().Material = face.Material;
            }
        }

        return final;
    }

    private static void MergeVerticesByDistance(MDLTrimeshNode trimesh, float threshold)
    {
        var vertices = trimesh.Faces.SelectMany(x => new List<MDLVertex>() { x.Vertex1, x.Vertex2, x.Vertex3 }).ToList();

        while (true)
        {
            var edited = false;

            foreach (var v1 in vertices)
            {
                foreach (var v2 in vertices)
                {
                    if (v1 == v2)
                        continue;

                    var distance = Vector3.Distance(v1.Position.Value, v2.Position.Value);
                    if (distance < threshold && distance > 0)
                    {
                        var middle = (v1.Position.Value + v2.Position.Value) / 2;
                        v1.SetPosition(middle);
                        v2.SetPosition(middle);
                        edited = true;
                    }
                }
            }

            if (!edited)
                break;
        }
    }

    private static void AdjustWalkmesh(MDL mdl, MDLTrimeshNode node)
    {
        var path = mdl.GetPathToNode(node);
        var position = path.First().GetController<MDLControllerDataPosition>().First().Data.First().ToVector3();
        var orientation = path.First().GetController<MDLControllerDataOrientation>().First().Data.First().ToQuaternion();
        var transform = Matrix4x4.CreateFromQuaternion(orientation) * Matrix4x4.CreateTranslation(position);
        foreach (var face in node.Faces)
        {
            face.Vertex1 = new MDLVertex().SetPosition(Vector3.Transform(face.Vertex1.Position.Value, transform));
            face.Vertex2 = new MDLVertex().SetPosition(Vector3.Transform(face.Vertex2.Position.Value, transform));
            face.Vertex3 = new MDLVertex().SetPosition(Vector3.Transform(face.Vertex3.Position.Value, transform));
        }
    }

    private static MDLNode WorldObjectToMDLNode(WorldObject worldObject)
    {
        var mdl = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{worldObject.KitID}/{worldObject.Template.Model}.mdl");
        mdl.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(worldObject.GlobalPosition));
        mdl.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(worldObject.GlobalOrientation));

        var walkmesh = mdl.Root.GetAllDescendants().OfType<MDLWalkmeshNode>().FirstOrDefault();
        if (walkmesh is not null)
        {
            AdjustWalkmesh(mdl, walkmesh);
        }

        return mdl.Root;
    }
}

public class WalkmeshBuilder
{
    public static WalkmeshBuilder Instance { get; } = new();

    private Triangle3[] Simplify(MDLWalkmeshNode walkmesh)
    {
        return walkmesh.Faces.Select(x => new Triangle3
        {
            V1 = new(x.Vertex1.Position.Value.X, x.Vertex1.Position.Value.Y, x.Vertex1.Position.Value.Z),
            V2 = new(x.Vertex2.Position.Value.X, x.Vertex2.Position.Value.Y, x.Vertex2.Position.Value.Z),
            V3 = new(x.Vertex3.Position.Value.X, x.Vertex3.Position.Value.Y, x.Vertex3.Position.Value.Z),
        }).ToArray();
    }

    public MDLTrimeshNode Bake(IEnumerable<MDLWalkmeshNode> walkmeshes)
    {
        var walkmeshList = walkmeshes.Select(Simplify).ToList();

        List<Triangle3> result = walkmeshList.First().ToList();
        walkmeshList.RemoveAt(0);

        while (walkmeshList.Count > 0)
        {
            var clip = walkmeshList.First();
            walkmeshList.RemoveAt(0);

            result = TriangleMeshClipper.ClipHolesAndUnion(result, clip);
            result.RemoveAll(x => x.Area == 0);
        }

        var newNode = new MDLTrimeshNode("walkmesh");
        newNode.EnableVertices();
        newNode.Faces.AddRange(result.Select(x =>
        {
            return new MDLFace()
            {
                Vertex1 = new MDLVertex().SetPosition(x.V1),
                Vertex2 = new MDLVertex().SetPosition(x.V2),
                Vertex3 = new MDLVertex().SetPosition(x.V3),
            };
        }));
        return newNode;
    }

    public MDLTrimeshNode Bake2(IEnumerable<MDLWalkmeshNode> walkmeshes)
    {
        var unprocessed = walkmeshes.Select(Simplify).ToList();
        var processed = new List<Triangle3>();

        while (unprocessed.Any())
        {
            var face = unprocessed.First();
            unprocessed.RemoveAt(0);

            if (processed.Count == 0)
            {
                processed.AddRange(face);
            }
            else
            {
                
            }
        }

        var newNode = new MDLTrimeshNode("walkmesh");
        newNode.EnableVertices();
        newNode.Faces.AddRange(result.Select(x =>
        {
            return new MDLFace()
            {
                Vertex1 = new MDLVertex().SetPosition(x.V1),
                Vertex2 = new MDLVertex().SetPosition(x.V2),
                Vertex3 = new MDLVertex().SetPosition(x.V3),
            };
        }));
        return newNode;
    }
}

public static class TriangleMeshClipper
{
    private const float Epsilon = 1e-3f;

    /// <summary>
    /// Clips holes from list1 using list2's XY footprint, then unions the result with list2.
    /// Z values are preserved via linear interpolation at clip edges.
    /// </summary>
    public static List<Triangle3> ClipHolesAndUnion(
        IEnumerable<Triangle3> list1,
        IEnumerable<Triangle3> list2)
    {
        var clipperList = RemoveVerticalTriangles(list2.ToList());

        // Step 1: Subtract every clipper triangle from all subject triangles
        var clipped = new List<Triangle3>(list1);
        foreach (var clipTri in clipperList)
        {
            var next = new List<Triangle3>();
            foreach (var subjectTri in clipped)
                next.AddRange(SubtractTriangle(subjectTri, clipTri));
            clipped = next;
        }

        // Step 2: Union = clipped list1 + full list2
        //clipped = MergeTriangles(clipped.ToArray()).ToList();
        clipped.AddRange(list2.ToList());
        return clipped;
    }

    // -------------------------------------------------------------------------
    // Core: subtract one triangle from another on the XY plane
    // -------------------------------------------------------------------------

    private static IEnumerable<Triangle3> SubtractTriangle(Triangle3 subject, Triangle3 clipper)
    {
        // Normalise clipper winding to CCW so edge normals point inward consistently
        var cv = new[] { clipper.V1, clipper.V2, clipper.V3 };
        if (!IsCCW(cv[0], cv[1], cv[2]))
            (cv[1], cv[2]) = (cv[2], cv[1]);

        // Progressive half-plane splitting:
        //   remaining  = polygons still being tested against upcoming clipper edges
        //   confirmed  = polygons that exited through a clipper edge → definitely outside clipper
        var confirmed = new List<List<Vector3>>();
        var remaining = new List<List<Vector3>> { new() { subject.V1, subject.V2, subject.V3 } };

        for (int i = 0; i < 3 && remaining.Count > 0; i++)
        {
            Vector3 edgeA = cv[i], edgeB = cv[(i + 1) % 3];
            var nextRemaining = new List<List<Vector3>>();

            foreach (var poly in remaining)
            {
                SplitByEdge(poly, edgeA, edgeB,
                    out var insidePoly,   // still inside clipper's half-plane → keep testing
                    out var outsidePoly); // escaped this half-plane → confirmed outside clipper

                if (outsidePoly.Count >= 3) confirmed.Add(outsidePoly);
                if (insidePoly.Count >= 3) nextRemaining.Add(insidePoly);
            }

            remaining = nextRemaining;
        }
        // Anything left in `remaining` is fully inside the clipper → discard (it's the hole)

        return confirmed.SelectMany(FanTriangulate);
    }

    // -------------------------------------------------------------------------
    // Half-plane split
    // -------------------------------------------------------------------------

    /// <summary>
    /// Splits <paramref name="poly"/> by the directed edge A→B.
    /// "Inside"  = left-hand side (same side as CCW clipper interior).
    /// "Outside" = right-hand side (outside the clipper).
    /// Z is linearly interpolated at every intersection point.
    /// </summary>
    private static void SplitByEdge(
        List<Vector3> poly,
        Vector3 edgeA, Vector3 edgeB,
        out List<Vector3> insidePoly,
        out List<Vector3> outsidePoly)
    {
        insidePoly = new List<Vector3>();
        outsidePoly = new List<Vector3>();

        int n = poly.Count;
        Vector3 edgeDir = edgeB - edgeA;

        for (int i = 0; i < n; i++)
        {
            Vector3 curr = poly[i];
            Vector3 next = poly[(i + 1) % n];

            float currDist = Cross2D(edgeDir, curr - edgeA);
            float nextDist = Cross2D(edgeDir, next - edgeA);

            bool currInside = currDist >= -Epsilon;
            bool nextInside = nextDist >= -Epsilon;

            // Emit current vertex to its side
            if (currInside) insidePoly.Add(curr);
            else outsidePoly.Add(curr);

            // Edge crosses the boundary → emit the intersection to both sides
            bool exitingInside = currInside && !nextInside;
            bool enteringInside = !currInside && nextInside;

            if (exitingInside || enteringInside)
            {
                float t = currDist / (currDist - nextDist);         // safe: signs differ
                Vector3 intersection = Vector3.Lerp(curr, next, t); // Z interpolated here
                insidePoly.Add(intersection);
                outsidePoly.Add(intersection);
            }
        }
    }

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    /// <summary>Fan-triangulates a convex polygon (safe because all fragments are convex).</summary>
    private static IEnumerable<Triangle3> FanTriangulate(List<Vector3> poly)
    {
        for (int i = 1; i < poly.Count - 1; i++)
            yield return new Triangle3(poly[0], poly[i], poly[i + 1]);
    }

    /// <summary>2-D cross product (Z component of 3-D cross) using only X/Y.</summary>
    private static float Cross2D(Vector3 a, Vector3 b) => a.X * b.Y - a.Y * b.X;

    /// <summary>Returns true if the triangle is counter-clockwise in XY.</summary>
    private static bool IsCCW(Vector3 a, Vector3 b, Vector3 c) =>
        Cross2D(b - a, c - a) > 0f;

    public static List<Triangle3> RemoveVerticalTriangles(IEnumerable<Triangle3> triangles)
    {
        var result = new List<Triangle3>();

        foreach (var t in triangles)
        {
            var e1 = t.V2 - t.V1;
            var e2 = t.V3 - t.V1;

            var normal = Vector3.Cross(e1, e2);

            // If Z component is ~0 → triangle is vertical (parallel to Z axis)
            if (MathF.Abs(normal.Z) < Epsilon)
                continue;

            result.Add(t);
        }

        return result;
    }

}

public struct Triangle2
{
    public Vector2 V1 { get; init; }
    public Vector2 V2 { get; init; }
    public Vector2 V3 { get; init; }
    public Vector2[] Vertices => [V1, V2, V3];

    public Vector2[] ToArray()
    {
        var v =new List<Vector2>() { V1, V2, V3 };
        EnsureCCW(v);
        return v.ToArray();
    }

    public Triangle2()
    {
    }
    public Triangle2(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }

    static void EnsureCCW(List<Vector2> poly)
    {
        if (SignedArea(poly) < 0)
            poly.Reverse();
    }

    static float SignedArea(List<Vector2> poly)
    {
        float area = 0;
        for (int i = 0; i < poly.Count; i++)
        {
            var a = poly[i];
            var b = poly[(i + 1) % poly.Count];
            area += (a.X * b.Y - b.X * a.Y);
        }
        return area * 0.5f;
    }
}

public struct Triangle3
{
    public Vector3 V1 { get; init; }
    public Vector3 V2 { get; init; }
    public Vector3 V3 { get; init; }
    public Vector3[] Vertices => [V1, V2, V3];

    public Vector3 Normal
    {
        get
        {
            // Calculate two edges of the triangle
            Vector3 edge1 = V2 - V1;
            Vector3 edge2 = V3 - V1;

            // Cross product gives a vector perpendicular to both edges
            Vector3 normal = Vector3.Cross(edge1, edge2);

            // Normalize to get a unit vector
            normal = Vector3.Normalize(normal);

            return normal;
        }
    }
    public float Area
    {
        get
        {
            Vector3 ab = V2 - V1;
            Vector3 ac = V3 - V1;

            Vector3 cross = Vector3.Cross(ab, ac);

            return 0.5f * cross.Length();
        }
    }

    public Triangle3()
    {
    }
    public Triangle3(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }
    public Triangle3(Triangle2 triangle2)
    {
        V1 = new(triangle2.V1.X, triangle2.V1.Y, 0);
        V2 = new(triangle2.V2.X, triangle2.V2.Y, 0);
        V3 = new(triangle2.V3.X, triangle2.V3.Y, 0);
    }

    public (Vector3, Vector3)[] Edges() => new[]
    {
        (V1, V2),
        (V2, V3),
        (V3, V1)
    };

    public bool TryGetZ(float x, float y, out float z)
    {
        // --- Barycentric in XY ---
        var p = new Vector2(x, y);
        var a = new Vector2(V1.X, V1.Y);
        var b = new Vector2(V2.X, V2.Y);
        var c = new Vector2(V3.X, V3.Y);

        var v0 = b - a;
        var v1_ = c - a;
        var v2_ = p - a;

        float d00 = Vector2.Dot(v0, v0);
        float d01 = Vector2.Dot(v0, v1_);
        float d11 = Vector2.Dot(v1_, v1_);
        float d20 = Vector2.Dot(v2_, v0);
        float d21 = Vector2.Dot(v2_, v1_);

        float denom = d00 * d11 - d01 * d01;

        if (MathF.Abs(denom) < 1e-6f)
        {
            z = 0;
            return false; // degenerate triangle
        }

        float v = (d11 * d20 - d01 * d21) / denom;
        float w = (d00 * d21 - d01 * d20) / denom;
        float u = 1.0f - v - w;

        // --- Inside triangle check ---
        if (u < 0 || v < 0 || w < 0)
        {
            z = 0;
            return false;
        }

        // --- Interpolate Z ---
        z = u * V1.Z + v * V2.Z + w * V3.Z;
        return true;
    }
}



public class Triangle
{
    public Color Color { get; set; }
    public Vector3 Point1 { get; set; }
    public Vector3 Point2 { get; set; }
    public Vector3 Point3 { get; set; }

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
        Color = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
    }
    public Triangle(Color color, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Color = color;
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
    }

    public Triangle S(int scale, int nudge)
    {
        return new(Color,
            (Point1 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point2 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point3 + new Vector3(nudge, nudge, nudge)) * scale);
    }
}


public class Triangle
{
    public Color Color { get; set; }
    public Vector3 Point1 { get; set; }
    public Vector3 Point2 { get; set; }
    public Vector3 Point3 { get; set; }

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
        Color = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
    }
    public Triangle(Color color, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Color = color;
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
    }

    public Triangle S(int scale, int nudge)
    {
        return new(Color,
            (Point1 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point2 + new Vector3(nudge, nudge, nudge)) * scale,
            (Point3 + new Vector3(nudge, nudge, nudge)) * scale);
    }
}

public static class PolygonUnion
{
    const float Eps = 1e-4f;

    public static List<Vector3> UnionOfTwoTriangles(Triangle t1, Triangle t2, float gapTolerance = 1e-2f)
    {
        var A = new List<Vector3> { t1.Point1, t1.Point2, t1.Point3 };
        var B = new List<Vector3> { t2.Point1, t2.Point2, t2.Point3 };

        // Every orientation/crossing test below is evaluated relative to a
        // single shared plane normal. That's only valid if the two triangles
        // are coplanar (or very nearly so) — that's the only case where a
        // 2D-style polygon union is well-defined. We derive the working
        // normal from t1 and fail loudly if t2 isn't close to that plane.
        Vector3 normal = TriangleNormal(t1);
        Vector3 normal2 = TriangleNormal(t2);

        if (MathF.Abs(Vector3.Dot(normal, normal2)) < 0.99f) // ~8 degrees
            throw new InvalidOperationException(
                "Triangles are not coplanar — UnionOfTwoTriangles only supports " +
                "coplanar (or near-coplanar) triangles. Normals differ by more than ~8 degrees.");

        var segsA = SplitEdges(A, B, gapTolerance, normal);
        var segsB = SplitEdges(B, A, gapTolerance, normal);

        var kept = new List<(Vector3 p1, Vector3 p2)>();

        foreach (var s in segsA)
        {
            var mid = (s.p1 + s.p2) * 0.5f;
            if (DistanceToTriangle(mid, B[0], B[1], B[2], normal) > gapTolerance)
                kept.Add(s);
        }
        foreach (var s in segsB)
        {
            var mid = (s.p1 + s.p2) * 0.5f;
            if (DistanceToTriangle(mid, A[0], A[1], A[2], normal) > gapTolerance)
                kept.Add(s);
        }

        return ChainSegments(kept, MathF.Max(Eps, gapTolerance));
    }

    static Vector3 TriangleNormal(Triangle t)
        => Vector3.Normalize(Vector3.Cross(t.Point2 - t.Point1, t.Point3 - t.Point1));

    // Signed "2D" cross product of two vectors as seen looking along `normal`.
    // Equivalent to a.X*b.Y - a.Y*b.X when normal = (0,0,1); generalizes that
    // orientation test to any shared plane in 3D.
    static float Cross2D(Vector3 a, Vector3 b, Vector3 normal)
        => Vector3.Dot(Vector3.Cross(a, b), normal);

    static float DistanceToTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
    {
        if (PointInTriangleStrict(p, a, b, c, normal)) return 0f;

        float d1 = Vector3.Distance(p, ClosestPointOnSegment(p, a, b));
        float d2 = Vector3.Distance(p, ClosestPointOnSegment(p, b, c));
        float d3 = Vector3.Distance(p, ClosestPointOnSegment(p, c, a));
        return MathF.Min(d1, MathF.Min(d2, d3));
    }

    static Vector3 ClosestPointOnSegment(Vector3 p, Vector3 a, Vector3 b)
    {
        Vector3 ab = b - a;
        float len2 = ab.LengthSquared();
        if (len2 < Eps) return a;
        float t = Math.Clamp(Vector3.Dot(p - a, ab) / len2, 0f, 1f);
        return a + t * ab;
    }

    static bool PointInTriangleStrict(Vector3 p, Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
    {
        float d1 = Cross2D(b - a, p - a, normal);
        float d2 = Cross2D(c - b, p - b, normal);
        float d3 = Cross2D(a - c, p - c, normal);

        bool hasNeg = d1 < -Eps || d2 < -Eps || d3 < -Eps;
        bool hasPos = d1 > Eps || d2 > Eps || d3 > Eps;

        return !(hasNeg && hasPos);
    }

    static List<(Vector3 p1, Vector3 p2)> SplitEdges(List<Vector3> poly, List<Vector3> other, float gapTolerance, Vector3 normal)
    {
        var result = new List<(Vector3, Vector3)>();

        for (int i = 0; i < poly.Count; i++)
        {
            Vector3 a1 = poly[i];
            Vector3 a2 = poly[(i + 1) % poly.Count];
            Vector3 r = a2 - a1;
            float rLenSq = r.LengthSquared();

            var ts = new List<float> { 0f, 1f };

            for (int j = 0; j < other.Count; j++)
            {
                Vector3 b1 = other[j];
                Vector3 b2 = other[(j + 1) % other.Count];
                Vector3 s = b2 - b1;

                float rxs = Cross2D(r, s, normal);

                if (MathF.Abs(rxs) >= Eps)
                {
                    Vector3 qp = b1 - a1;
                    float t = Cross2D(qp, s, normal) / rxs;
                    float u = Cross2D(qp, r, normal) / rxs;

                    if (t > Eps && t < 1 - Eps && u > -Eps && u < 1 + Eps)
                        ts.Add(Math.Clamp(t, 0f, 1f));
                }

                if (gapTolerance > Eps && rLenSq > Eps)
                {
                    float tApproach = ClosestApproachParam(a1, a2, b1, b2);
                    Vector3 pOnA = a1 + tApproach * r;
                    Vector3 pOnB = ClosestPointOnSegment(pOnA, b1, b2);
                    float dist = Vector3.Distance(pOnA, pOnB);

                    if (dist <= gapTolerance && tApproach > Eps && tApproach < 1 - Eps)
                        ts.Add(tApproach);
                }
            }

            ts.Sort();
            for (int k = 0; k < ts.Count - 1; k++)
            {
                if (ts[k + 1] - ts[k] < Eps) continue;
                // Interpolated on the real 3D edge, so Z is exact — never
                // reconstructed from a 2D projection.
                Vector3 p1 = a1 + ts[k] * r;
                Vector3 p2 = a1 + ts[k + 1] * r;
                result.Add((p1, p2));
            }
        }

        return result;
    }

    static float ClosestApproachParam(Vector3 a1, Vector3 a2, Vector3 b1, Vector3 b2)
    {
        float best = 0f;
        float bestDist = float.PositiveInfinity;

        void Consider(Vector3 p)
        {
            Vector3 c = ClosestPointOnSegment(p, a1, a2);
            float d = Vector3.Distance(p, c);
            if (d < bestDist)
            {
                bestDist = d;
                Vector3 r = a2 - a1;
                float len2 = r.LengthSquared();
                best = len2 < Eps ? 0f : Vector3.Dot(c - a1, r) / len2;
            }
        }

        Consider(b1);
        Consider(b2);

        return Math.Clamp(best, 0f, 1f);
    }

    static List<Vector3> ChainSegments(List<(Vector3 p1, Vector3 p2)> segs, float matchTolerance)
    {
        if (segs.Count == 0) return new List<Vector3>();

        var remaining = new List<(Vector3 p1, Vector3 p2)>(segs);
        var loop = new List<Vector3> { remaining[0].p1, remaining[0].p2 };
        remaining.RemoveAt(0);

        while (remaining.Count > 0)
        {
            Vector3 tail = loop[^1];
            int idx = remaining.FindIndex(s =>
                Vector3.Distance(s.p1, tail) < matchTolerance ||
                Vector3.Distance(s.p2, tail) < matchTolerance);

            if (idx < 0)
                throw new InvalidOperationException(
                    "Could not close the loop — triangles may still be too far apart " +
                    "for the given gapTolerance, or only touch at a point.");

            var seg = remaining[idx];
            bool matchedP1 = Vector3.Distance(seg.p1, tail) < matchTolerance;
            Vector3 next = matchedP1 ? seg.p2 : seg.p1;

            if (Vector3.Distance(next, loop[0]) > matchTolerance)
                loop.Add(next);

            remaining.RemoveAt(idx);
        }

        return loop;
    }
}

public static class PolygonTriangulator
{
    const float Eps = 1e-4f;
    const float MinTriangleArea = 1e-3f;

    public static List<Triangle> Triangulate(List<Vector3> polygon)
    {
        var triangles = new List<Triangle>();
        var verts = CleanPolygon(polygon);

        if (verts.Count < 3) return triangles;

        if (verts.Count == 3)
        {
            triangles.Add(MakeTriangle(verts[0], verts[1], verts[2]));
            return triangles;
        }

        Vector3 normal = ComputeNormal(verts);

        if (SignedArea(verts, normal) < 0)
            verts.Reverse();

        var indices = new List<int>();
        for (int i = 0; i < verts.Count; i++) indices.Add(i);

        int guard = 0;
        int maxIterations = verts.Count * verts.Count;

        while (indices.Count > 3 && guard++ < maxIterations)
        {
            int bestEar = -1;
            float bestScore = float.NegativeInfinity;

            for (int i = 0; i < indices.Count; i++)
            {
                int iPrev = indices[(i - 1 + indices.Count) % indices.Count];
                int iCurr = indices[i];
                int iNext = indices[(i + 1) % indices.Count];

                Vector3 a = verts[iPrev];
                Vector3 b = verts[iCurr];
                Vector3 c = verts[iNext];

                if (Cross2D(b - a, c - b, normal) <= Eps) continue;

                float triArea = TriangleArea(a, b, c);
                if (triArea < MinTriangleArea) continue;

                bool anyPointInside = false;
                for (int j = 0; j < indices.Count; j++)
                {
                    int idx = indices[j];
                    if (idx == iPrev || idx == iCurr || idx == iNext) continue;
                    if (PointInTriangle(verts[idx], a, b, c, normal))
                    {
                        anyPointInside = true;
                        break;
                    }
                }
                if (anyPointInside) continue;

                float quality = TriangleQuality(a, b, c);
                if (quality > bestScore)
                {
                    bestScore = quality;
                    bestEar = i;
                }
            }

            if (bestEar < 0)
                throw new InvalidOperationException(
                    "No valid non-degenerate ear found — polygon may be self-intersecting, " +
                    "non-planar, have near-zero area, or MinTriangleArea may need adjusting.");

            int prevI = indices[(bestEar - 1 + indices.Count) % indices.Count];
            int currI = indices[bestEar];
            int nextI = indices[(bestEar + 1) % indices.Count];

            triangles.Add(MakeTriangle(verts[prevI], verts[currI], verts[nextI]));
            indices.RemoveAt(bestEar);
        }

        if (indices.Count == 3)
            triangles.Add(MakeTriangle(verts[indices[0]], verts[indices[1]], verts[indices[2]]));

        return triangles;
    }

    // Robust best-fit normal for a (near-)planar polygon, via Newell's method.
    // Works even with slight numerical drift from the union step, or a
    // concave/reflex boundary.
    static Vector3 ComputeNormal(List<Vector3> poly)
    {
        Vector3 n = Vector3.Zero;
        int count = poly.Count;
        for (int i = 0; i < count; i++)
        {
            Vector3 curr = poly[i];
            Vector3 next = poly[(i + 1) % count];
            n.X += (curr.Y - next.Y) * (curr.Z + next.Z);
            n.Y += (curr.Z - next.Z) * (curr.X + next.X);
            n.Z += (curr.X - next.X) * (curr.Y + next.Y);
        }
        return Vector3.Normalize(n);
    }

    static float Cross2D(Vector3 a, Vector3 b, Vector3 normal)
        => Vector3.Dot(Vector3.Cross(a, b), normal);

    static List<Vector3> CleanPolygon(List<Vector3> poly)
    {
        var noDup = new List<Vector3>();
        foreach (var p in poly)
        {
            if (noDup.Count == 0 || Vector3.Distance(noDup[^1], p) > Eps)
                noDup.Add(p);
        }
        if (noDup.Count > 1 && Vector3.Distance(noDup[0], noDup[^1]) < Eps)
            noDup.RemoveAt(noDup.Count - 1);

        if (noDup.Count < 3) return noDup;

        Vector3 normal = ComputeNormal(noDup);

        var result = new List<Vector3>();
        int n = noDup.Count;
        for (int i = 0; i < n; i++)
        {
            Vector3 prev = noDup[(i - 1 + n) % n];
            Vector3 curr = noDup[i];
            Vector3 next = noDup[(i + 1) % n];

            if (MathF.Abs(Cross2D(curr - prev, next - curr, normal)) > Eps)
                result.Add(curr);
        }

        return result.Count >= 3 ? result : noDup;
    }

    static Triangle MakeTriangle(Vector3 a, Vector3 b, Vector3 c)
        => new Triangle { Point1 = a, Point2 = b, Point3 = c };

    static float TriangleArea(Vector3 a, Vector3 b, Vector3 c)
        => Vector3.Cross(b - a, c - a).Length() * 0.5f;

    static float TriangleQuality(Vector3 a, Vector3 b, Vector3 c)
    {
        float area = TriangleArea(a, b, c);
        float ab = Vector3.DistanceSquared(a, b);
        float bc = Vector3.DistanceSquared(b, c);
        float ca = Vector3.DistanceSquared(c, a);
        float denom = ab + bc + ca;
        return denom < Eps ? 0f : (4f * MathF.Sqrt(3f) * area) / denom;
    }

    static bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c, Vector3 normal)
    {
        float d1 = Cross2D(b - a, p - a, normal);
        float d2 = Cross2D(c - b, p - b, normal);
        float d3 = Cross2D(a - c, p - c, normal);

        bool hasNeg = d1 < -Eps || d2 < -Eps || d3 < -Eps;
        bool hasPos = d1 > Eps || d2 > Eps || d3 > Eps;

        return !(hasNeg && hasPos);
    }

    static float SignedArea(List<Vector3> poly, Vector3 normal)
    {
        float area = 0f;
        for (int i = 0; i < poly.Count; i++)
            area += Cross2D(poly[i], poly[(i + 1) % poly.Count], normal);
        return area * 0.5f;
    }
}

