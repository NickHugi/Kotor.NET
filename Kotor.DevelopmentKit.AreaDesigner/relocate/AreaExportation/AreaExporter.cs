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
        var newWalkmesh = WalkmeshBuilder.Instance.Bake2(walkmeshes.ToList());
        //var newWalkmesh = MergeWalkmeshes(walkmeshes.ToList());
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

    private Triangle[] Simplify(MDLWalkmeshNode walkmesh)
    {
        return walkmesh.Faces.Select(x => new Triangle
        {
            Point1 = new(x.Vertex1.Position.Value.X, x.Vertex1.Position.Value.Y, x.Vertex1.Position.Value.Z),
            Point2 = new(x.Vertex2.Position.Value.X, x.Vertex2.Position.Value.Y, x.Vertex2.Position.Value.Z),
            Point3 = new(x.Vertex3.Position.Value.X, x.Vertex3.Position.Value.Y, x.Vertex3.Position.Value.Z),
        }).ToArray();
    }

    public MDLWalkmeshNode Bake2(IEnumerable<MDLWalkmeshNode> walkmeshes)
    {
        var unprocessed = walkmeshes.SelectMany(Simplify).ToList();
        var processed = TriangleListUnion.UnionAll(unprocessed, out var changed);
        //var processed = new List<Triangle>();

        //foreach (var triangle in unprocessed.ToList())
        //{
        //    if (MathF.Abs(Vector3.Dot(triangle.Normal, new Vector3(0, 0, 1))) < 1e-1f)
        //    {
        //        unprocessed.Remove(triangle);
        //        processed.AddRange(triangle);
        //    }
        //}

        //while (unprocessed.Any())
        //{
        //    var triangle1 = unprocessed.First();
        //    unprocessed.RemoveAt(0);

        //    if (processed.Count == 0)
        //    {
        //        processed.Add(triangle1);
        //    }
        //    else
        //    {
        //        var xyz = false;
        //        foreach (var triangle2 in processed.ToList())
        //        {
        //            List<Triangle> clipped;
        //            try
        //            {
        //                //List<Vector3> unionOutline = PolygonUnion.UnionOfTwoTriangles(triangle1, triangle2);
        //                //clipped = PolygonTriangulator.Triangulate(unionOutline);
        //                clipped = PolygonUnion.UnionOfTwoTrianglesTriangulated(triangle1, triangle2, out bool changed);
        //                // REVIST - Add clipped to unprocessed

        //                if (clipped.Count() != 2)
        //                {
        //                    xyz = true;
        //                    processed.Remove(triangle2);
        //                    unprocessed.AddRange(clipped);
        //                    break;
        //                }
        //                //else
        //                //{
        //                //    processed.Add(triangle1);
        //                //}

        //                //break;
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        if (!xyz)
        //            processed.Add(triangle1);
        //    }
        //}

        var newNode = new MDLWalkmeshNode("walkmesh");
        newNode.EnableVertices();
        newNode.Faces.AddRange(processed.Select(x =>
        {
            return new MDLFace()
            {
                Vertex1 = new MDLVertex().SetPosition(x.Point1),
                Vertex2 = new MDLVertex().SetPosition(x.Point2),
                Vertex3 = new MDLVertex().SetPosition(x.Point3),
                Material = SurfaceMaterial.Grass
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
    public static List<Triangle> ClipHolesAndUnion(
        IEnumerable<Triangle> list1,
        IEnumerable<Triangle> list2)
    {
        var clipperList = RemoveVerticalTriangles(list2.ToList());

        // Step 1: Subtract every clipper triangle from all subject triangles
        var clipped = new List<Triangle>(list1);
        foreach (var clipTri in clipperList)
        {
            var next = new List<Triangle>();
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

    private static IEnumerable<Triangle> SubtractTriangle(Triangle subject, Triangle clipper)
    {
        // Normalise clipper winding to CCW so edge normals point inward consistently
        var cv = new[] { clipper.Point1, clipper.Point2, clipper.Point3 };
        if (!IsCCW(cv[0], cv[1], cv[2]))
            (cv[1], cv[2]) = (cv[2], cv[1]);

        // Progressive half-plane splitting:
        //   remaining  = polygons still being tested against upcoming clipper edges
        //   confirmed  = polygons that exited through a clipper edge → definitely outside clipper
        var confirmed = new List<List<Vector3>>();
        var remaining = new List<List<Vector3>> { new() { subject.Point1, subject.Point2, subject.Point3 } };

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
    private static IEnumerable<Triangle> FanTriangulate(List<Vector3> poly)
    {
        for (int i = 1; i < poly.Count - 1; i++)
            yield return new Triangle(poly[0], poly[i], poly[i + 1]);
    }

    /// <summary>2-D cross product (Z component of 3-D cross) using only X/Y.</summary>
    private static float Cross2D(Vector3 a, Vector3 b) => a.X * b.Y - a.Y * b.X;

    /// <summary>Returns true if the triangle is counter-clockwise in XY.</summary>
    private static bool IsCCW(Vector3 a, Vector3 b, Vector3 c) =>
        Cross2D(b - a, c - a) > 0f;

    public static List<Triangle> RemoveVerticalTriangles(IEnumerable<Triangle> triangles)
    {
        var result = new List<Triangle>();

        foreach (var t in triangles)
        {
            var e1 = t.Point2 - t.Point1;
            var e2 = t.Point3 - t.Point1;

            var normal = Vector3.Cross(e1, e2);

            // If Z component is ~0 → triangle is vertical (parallel to Z axis)
            if (MathF.Abs(normal.Z) < Epsilon)
                continue;

            result.Add(t);
        }

        return result;
    }

}

public class Triangle
{
    public SurfaceMaterial Color { get; set; }
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
        Color = 0;
        Point1 = p1;
        Point2 = p2;
        Point3 = p3;
    }
    public Triangle(SurfaceMaterial color, Vector3 p1, Vector3 p2, Vector3 p3)
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

public static class TriangleListUnion
{
    const float Eps = 1e-4f;

    public static List<Triangle> UnionAll(
        List<Triangle> triangles,
        out bool changed,
        float gapTolerance = 1e-2f,
        float matchTolerance = 1e-2f)
    {
        if (triangles == null || triangles.Count == 0)
        {
            changed = false;
            return new List<Triangle>();
        }
        if (triangles.Count == 1)
        {
            changed = false;
            return new List<Triangle> { triangles[0] };
        }

        Vector3 normal = TriangleNormal(triangles[0]);
        Vector3 origin = triangles[0].Point1;
        foreach (var t in triangles)
        {
            if (MathF.Abs(Vector3.Dot(normal, TriangleNormal(t))) < 0.99f)
                throw new InvalidOperationException(
                    "All triangles must be coplanar (or near-coplanar) to union as a group.");
        }

        var (u, v) = BuildBasis(normal);
        Vector2 ToUV(Vector3 p)
        {
            Vector3 d = p - origin;
            return new Vector2(Vector3.Dot(d, u), Vector3.Dot(d, v));
        }
        Vector3 ToWorld(Vector2 p) => origin + p.X * u + p.Y * v;

        // Each cluster: a merged 2D outline + the 2D source triangles (for color lookup)
        var clusters = new List<(List<Vector2> Outline, List<(List<Vector2> Pts, SurfaceMaterial Color)> Sources)>();
        foreach (var t in triangles)
        {
            var pts = new List<Vector2> { ToUV(t.Point1), ToUV(t.Point2), ToUV(t.Point3) };
            clusters.Add((pts, new List<(List<Vector2>, SurfaceMaterial)> { (pts, t.Color) }));
        }

        bool mergedAny = true;
        while (mergedAny)
        {
            mergedAny = false;
            for (int i = 0; i < clusters.Count && !mergedAny; i++)
            {
                for (int j = i + 1; j < clusters.Count && !mergedAny; j++)
                {
                    if (TryUnionOutlines(clusters[i].Outline, clusters[j].Outline, gapTolerance, out var mergedOutline))
                    {
                        var mergedSources = new List<(List<Vector2>, SurfaceMaterial)>(clusters[i].Sources);
                        mergedSources.AddRange(clusters[j].Sources);
                        clusters[i] = (mergedOutline, mergedSources);
                        clusters.RemoveAt(j);
                        mergedAny = true;
                    }
                }
            }
        }

        var result = new List<Triangle>();
        foreach (var cluster in clusters)
        {
            var cleanOutline = Clean2D(cluster.Outline);
            if (cleanOutline.Count < 3) continue;

            foreach (var (a, b, c) in Triangulate2D(cleanOutline))
            {
                Vector2 centroid = (a + b + c) / 3f;
                result.Add(new Triangle
                {
                    Color = PickColor(centroid, cluster.Sources),
                    Point1 = ToWorld(a),
                    Point2 = ToWorld(b),
                    Point3 = ToWorld(c)
                });
            }
        }

        changed = !MatchesOriginals(result, triangles, matchTolerance);
        return result;
    }

    static SurfaceMaterial PickColor(Vector2 p, List<(List<Vector2> Pts, SurfaceMaterial Color)> sources)
    {
        foreach (var (pts, color) in sources)
            if (PointInPolygon(p, pts))
                return color;

        // Gap-bridge slice with no exact source — fall back to nearest source centroid.
        float bestDist = float.PositiveInfinity;
        SurfaceMaterial best = sources[0].Color;
        foreach (var (pts, color) in sources)
        {
            Vector2 c = (pts[0] + pts[1] + pts[2]) / 3f;
            float d = Vector2.DistanceSquared(p, c);
            if (d < bestDist) { bestDist = d; best = color; }
        }
        return best;
    }

    // ---- 2D geometry core ----

    static bool TryUnionOutlines(List<Vector2> A, List<Vector2> B, float gapTolerance, out List<Vector2> merged)
    {
        merged = null;

        var segsA = SplitEdges(A, B, gapTolerance);
        var segsB = SplitEdges(B, A, gapTolerance);

        var kept = new List<(Vector2 p1, Vector2 p2)>();
        foreach (var s in segsA)
        {
            var mid = (s.p1 + s.p2) * 0.5f;
            if (DistanceToPolygon(mid, B) > gapTolerance) kept.Add(s);
        }
        foreach (var s in segsB)
        {
            var mid = (s.p1 + s.p2) * 0.5f;
            if (DistanceToPolygon(mid, A) > gapTolerance) kept.Add(s);
        }

        if (kept.Count == 0)
        {
            // Everything got absorbed both ways — identical or one fully
            // contains the other. Merge into whichever has the larger area.
            merged = MathF.Abs(SignedArea(A)) >= MathF.Abs(SignedArea(B)) ? A : B;
            return true;
        }

        return TryChainSingleLoop(kept, MathF.Max(Eps, gapTolerance), out merged);
    }

    static List<(Vector2 p1, Vector2 p2)> SplitEdges(List<Vector2> poly, List<Vector2> other, float gapTolerance)
    {
        var result = new List<(Vector2, Vector2)>();

        for (int i = 0; i < poly.Count; i++)
        {
            Vector2 a1 = poly[i];
            Vector2 a2 = poly[(i + 1) % poly.Count];
            Vector2 r = a2 - a1;
            float rLenSq = r.LengthSquared();

            var ts = new List<float> { 0f, 1f };

            for (int j = 0; j < other.Count; j++)
            {
                Vector2 b1 = other[j];
                Vector2 b2 = other[(j + 1) % other.Count];
                Vector2 s = b2 - b1;

                float rxs = Cross(r, s);
                if (MathF.Abs(rxs) >= Eps)
                {
                    Vector2 qp = b1 - a1;
                    float t = Cross(qp, s) / rxs;
                    float uu = Cross(qp, r) / rxs;
                    if (t > Eps && t < 1 - Eps && uu > -Eps && uu < 1 + Eps)
                        ts.Add(Math.Clamp(t, 0f, 1f));
                }

                if (gapTolerance > Eps && rLenSq > Eps)
                {
                    float tApproach = ClosestApproachParam(a1, a2, b1, b2);
                    Vector2 pOnA = a1 + tApproach * r;
                    Vector2 pOnB = ClosestPointOnSegment(pOnA, b1, b2);
                    float dist = Vector2.Distance(pOnA, pOnB);
                    if (dist <= gapTolerance && tApproach > Eps && tApproach < 1 - Eps)
                        ts.Add(tApproach);
                }
            }

            ts.Sort();
            for (int k = 0; k < ts.Count - 1; k++)
            {
                if (ts[k + 1] - ts[k] < Eps) continue;
                result.Add((a1 + ts[k] * r, a1 + ts[k + 1] * r));
            }
        }

        return result;
    }

    static float ClosestApproachParam(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        float best = 0f;
        float bestDist = float.PositiveInfinity;
        void Consider(Vector2 p)
        {
            Vector2 c = ClosestPointOnSegment(p, a1, a2);
            float d = Vector2.Distance(p, c);
            if (d < bestDist)
            {
                bestDist = d;
                Vector2 r = a2 - a1;
                float len2 = r.LengthSquared();
                best = len2 < Eps ? 0f : Vector2.Dot(c - a1, r) / len2;
            }
        }
        Consider(b1);
        Consider(b2);
        return Math.Clamp(best, 0f, 1f);
    }

    static Vector2 ClosestPointOnSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        float len2 = ab.LengthSquared();
        if (len2 < Eps) return a;
        float t = Math.Clamp(Vector2.Dot(p - a, ab) / len2, 0f, 1f);
        return a + t * ab;
    }

    // Ray-casting — works for concave outlines too, unlike a triangle-only inside test.
    static bool PointInPolygon(Vector2 p, List<Vector2> poly)
    {
        bool inside = false;
        int n = poly.Count;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 pi = poly[i], pj = poly[j];

            bool onSegment = MathF.Abs(Cross(pj - pi, p - pi)) < Eps
                && Vector2.Dot(p - pi, pj - pi) >= -Eps
                && Vector2.Dot(p - pj, pi - pj) >= -Eps;
            if (onSegment) return true;

            if ((pi.Y > p.Y) != (pj.Y > p.Y))
            {
                float xIntersect = pi.X + (p.Y - pi.Y) / (pj.Y - pi.Y) * (pj.X - pi.X);
                if (p.X < xIntersect) inside = !inside;
            }
        }
        return inside;
    }

    static float DistanceToPolygon(Vector2 p, List<Vector2> poly)
    {
        if (PointInPolygon(p, poly)) return 0f;

        float best = float.PositiveInfinity;
        int n = poly.Count;
        for (int i = 0; i < n; i++)
        {
            float d = Vector2.Distance(p, ClosestPointOnSegment(p, poly[i], poly[(i + 1) % n]));
            if (d < best) best = d;
        }
        return best;
    }

    // Returns false (instead of throwing) if segments don't form one closed loop —
    // used as the "no merge happened" signal for disjoint shapes.
    static bool TryChainSingleLoop(List<(Vector2 p1, Vector2 p2)> segs, float matchTolerance, out List<Vector2> loopOut)
    {
        loopOut = null;
        if (segs.Count == 0) return false;

        var remaining = new List<(Vector2 p1, Vector2 p2)>(segs);
        var loop = new List<Vector2> { remaining[0].p1, remaining[0].p2 };
        remaining.RemoveAt(0);

        while (remaining.Count > 0)
        {
            Vector2 tail = loop[^1];
            int idx = remaining.FindIndex(s =>
                Vector2.Distance(s.p1, tail) < matchTolerance ||
                Vector2.Distance(s.p2, tail) < matchTolerance);

            if (idx < 0) return false;

            var seg = remaining[idx];
            bool matchedP1 = Vector2.Distance(seg.p1, tail) < matchTolerance;
            Vector2 next = matchedP1 ? seg.p2 : seg.p1;

            if (Vector2.Distance(next, loop[0]) > matchTolerance)
                loop.Add(next);

            remaining.RemoveAt(idx);
        }

        if (loop.Count < 3) return false;
        loopOut = loop;
        return true;
    }

    static List<Vector2> Clean2D(List<Vector2> poly)
    {
        var noDup = new List<Vector2>();
        foreach (var p in poly)
            if (noDup.Count == 0 || Vector2.Distance(noDup[^1], p) > Eps)
                noDup.Add(p);
        if (noDup.Count > 1 && Vector2.Distance(noDup[0], noDup[^1]) < Eps)
            noDup.RemoveAt(noDup.Count - 1);

        if (noDup.Count < 3) return noDup;

        var result = new List<Vector2>();
        int n = noDup.Count;
        for (int i = 0; i < n; i++)
        {
            Vector2 prev = noDup[(i - 1 + n) % n];
            Vector2 curr = noDup[i];
            Vector2 next = noDup[(i + 1) % n];
            if (MathF.Abs(Cross(curr - prev, next - curr)) > Eps)
                result.Add(curr);
        }
        return result.Count >= 3 ? result : noDup;
    }

    static List<(Vector2, Vector2, Vector2)> Triangulate2D(List<Vector2> polygon)
    {
        var triangles = new List<(Vector2, Vector2, Vector2)>();
        var verts = new List<Vector2>(polygon);
        if (verts.Count < 3) return triangles;
        if (verts.Count == 3)
        {
            triangles.Add((verts[0], verts[1], verts[2]));
            return triangles;
        }

        if (SignedArea(verts) < 0) verts.Reverse();

        var indices = new List<int>();
        for (int i = 0; i < verts.Count; i++) indices.Add(i);

        int guard = 0;
        int maxIterations = verts.Count * verts.Count;
        const float minArea = 1e-3f;

        while (indices.Count > 3 && guard++ < maxIterations)
        {
            int bestEar = -1;
            float bestScore = float.NegativeInfinity;

            for (int i = 0; i < indices.Count; i++)
            {
                int iPrev = indices[(i - 1 + indices.Count) % indices.Count];
                int iCurr = indices[i];
                int iNext = indices[(i + 1) % indices.Count];

                Vector2 a = verts[iPrev], b = verts[iCurr], c = verts[iNext];
                if (Cross(b - a, c - b) <= Eps) continue;

                float area = MathF.Abs(Cross(b - a, c - a)) * 0.5f;
                if (area < minArea) continue;

                bool anyInside = false;
                for (int j = 0; j < indices.Count; j++)
                {
                    int idx = indices[j];
                    if (idx == iPrev || idx == iCurr || idx == iNext) continue;
                    if (PointInTriangle2D(verts[idx], a, b, c)) { anyInside = true; break; }
                }
                if (anyInside) continue;

                float ab = Vector2.DistanceSquared(a, b);
                float bc = Vector2.DistanceSquared(b, c);
                float ca = Vector2.DistanceSquared(c, a);
                float denom = ab + bc + ca;
                float quality = denom < Eps ? 0f : (4f * MathF.Sqrt(3f) * area) / denom;

                if (quality > bestScore) { bestScore = quality; bestEar = i; }
            }

            if (bestEar < 0)
                throw new InvalidOperationException("No valid ear found while triangulating merged outline.");

            int prevI = indices[(bestEar - 1 + indices.Count) % indices.Count];
            int currI = indices[bestEar];
            int nextI = indices[(bestEar + 1) % indices.Count];
            triangles.Add((verts[prevI], verts[currI], verts[nextI]));
            indices.RemoveAt(bestEar);
        }

        if (indices.Count == 3)
            triangles.Add((verts[indices[0]], verts[indices[1]], verts[indices[2]]));

        return triangles;
    }

    static bool PointInTriangle2D(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float d1 = Cross(b - a, p - a);
        float d2 = Cross(c - b, p - b);
        float d3 = Cross(a - c, p - c);
        bool hasNeg = d1 < -Eps || d2 < -Eps || d3 < -Eps;
        bool hasPos = d1 > Eps || d2 > Eps || d3 > Eps;
        return !(hasNeg && hasPos);
    }

    static float SignedArea(List<Vector2> poly)
    {
        float area = 0f;
        for (int i = 0; i < poly.Count; i++)
            area += Cross(poly[i], poly[(i + 1) % poly.Count]);
        return area * 0.5f;
    }

    static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

    // ---- 3D plane helpers ----

    static Vector3 TriangleNormal(Triangle t)
        => Vector3.Normalize(Vector3.Cross(t.Point2 - t.Point1, t.Point3 - t.Point1));

    static (Vector3 U, Vector3 V) BuildBasis(Vector3 normal)
    {
        normal = Vector3.Normalize(normal);
        Vector3 arbitrary = MathF.Abs(normal.X) < 0.9f ? Vector3.UnitX : Vector3.UnitY;
        Vector3 u = Vector3.Normalize(Vector3.Cross(normal, arbitrary));
        Vector3 v = Vector3.Cross(normal, u);
        return (u, v);
    }

    static bool MatchesOriginals(List<Triangle> result, List<Triangle> originals, float tolerance)
    {
        if (result.Count != originals.Count) return false;

        var remaining = new List<Triangle>(originals);
        foreach (var r in result)
        {
            int idx = remaining.FindIndex(o => TrianglesMatch(r, o, tolerance));
            if (idx < 0) return false;
            remaining.RemoveAt(idx);
        }
        return true;
    }

    static bool TrianglesMatch(Triangle a, Triangle b, float tolerance)
    {
        if (a.Color != b.Color) return false;

        var aPts = new[] { a.Point1, a.Point2, a.Point3 };
        var bPts = new[] { b.Point1, b.Point2, b.Point3 };

        for (int rot = 0; rot < 3; rot++)
        {
            bool allMatch = true;
            for (int i = 0; i < 3; i++)
                if (Vector3.Distance(aPts[i], bPts[(i + rot) % 3]) > tolerance) { allMatch = false; break; }
            if (allMatch) return true;

            allMatch = true;
            for (int i = 0; i < 3; i++)
                if (Vector3.Distance(aPts[i], bPts[(rot - i + 3) % 3]) > tolerance) { allMatch = false; break; }
            if (allMatch) return true;
        }
        return false;
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

