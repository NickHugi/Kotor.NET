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

        foreach (var tile in room.Tiles)
        {
            mdl.Root.Children.Add(FloorToMDLNode(tile.Floor));
            //mdl.Root.Children.Add(CeilingToMDLNode(tile.Ceiling));
            mdl.Root.Children.AddRange(tile.Walls.Where(x => x.Visible).Select(WallToMDLNode));
            mdl.Root.Children.AddRange(tile.Walls.Select(x => x.DoorFrame).Where(x => x?.Visible == true).Select(DoorFrameToMDLNode));
            mdl.Root.Children.AddRange(tile.InnerCorners.Where(x => x.Visible == true).Select(InnerCornerToMDLNode));
            mdl.Root.Children.AddRange(room.Objects.Select(ObjectToMDLNode));
        }

        var walkmeshes = mdl.Root.GetAllDescendants().OfType<MDLWalkmeshNode>();
        var newWalkmesh = MergeWalkmeshes(walkmeshes.ToList());
        DeleteWalkmeshesRecursive(mdl.Root); // move method to mdl
        newWalkmesh.RootNode = new AABBTreeBuilder().Build(newWalkmesh.Faces.OfType<IFace>().ToList());
        mdl.Root.Children.Add(newWalkmesh);

        //var walkmeshes = mdl.Root.GetAllDescendants().OfType<MDLWalkmeshNode>();
        //var finalWalkmesh = WalkmeshBuilder.Instance.Bake(walkmeshes);
        //mdl.DeleteWalkmesh();
        //mdl.Root.Children.Add(finalWalkmesh);

        mdl.Root.GetAllDescendants().OfType<MDLTrimeshNode>().ToList().ForEach(x => x.LightmapTexture = "");
        mdl.Root.GetAllDescendants().Select((x, i) => x.Name = i.ToString()).ToArray();
        newWalkmesh.Name = "walkmesh";
        mdl.RedoNodeNumbers();

        return mdl;
    }

    private static MDLNode FloorToMDLNode(Floor floor)
    {
        var floorMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{floor.KitID}/{floor.Template.Model}.mdl");
        floorMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(floor.Position));
        floorMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(floor.Orientation));
        AdjustWalkmesh(floorMDL, floorMDL.Root.GetAllDescendants().OfType<MDLWalkmeshNode>().First());
        return floorMDL.Root;
    }

    private static MDLNode CeilingToMDLNode(Ceiling ceiling)
    {
        var ceilingMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{ceiling.KitID}/{ceiling.Template.Model}.mdl");
        ceilingMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(ceiling.Position));
        ceilingMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(ceiling.Orientation));
        return ceilingMDL.Root;
    }

    private static MDLNode WallToMDLNode(Wall wall)
    {
        var wallMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{wall.KitID}/{wall.Template.Model}.mdl");
        wallMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(wall.Position));
        wallMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(wall.Orientation));
        AdjustWalkmesh(wallMDL, wallMDL.Root.GetAllDescendants().OfType<MDLWalkmeshNode>().First());
        return wallMDL.Root;
    }

    private static MDLNode DoorFrameToMDLNode(DoorFrame doorframe)
    {
        var doorframeMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{doorframe.KitID}/{doorframe.Template.Model}.mdl");
        doorframeMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(doorframe.Position));
        doorframeMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(doorframe.Orientation));
        AdjustWalkmesh(doorframeMDL, doorframeMDL.Root.GetAllDescendants().OfType<MDLWalkmeshNode>().First());
        return doorframeMDL.Root;
    }

    private static MDLNode InnerCornerToMDLNode(InnerCorner corner)
    {
        var cornerMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{corner.KitID}/{corner.Template.Model}.mdl");
        cornerMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(corner.Position));
        cornerMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(corner.Orientation));
        return cornerMDL.Root;
    }

    private static MDLNode ObjectToMDLNode(Object @object)
    {
        var objectMDL = MDL.FromFile($"{Kit.Manager.ActiveDirectory}/{@object.KitID}/{@object.Template.Model}.mdl");
        objectMDL.Root.GetController<MDLControllerDataPosition>().AddLinear(0, new(@object.LocalPosition));
        objectMDL.Root.GetController<MDLControllerDataOrientation>().AddLinear(0, new(@object.LocalOrientation));
        AdjustWalkmesh(objectMDL, objectMDL.Root.GetAllDescendants().OfType<MDLWalkmeshNode>().First());
        return objectMDL.Root;
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

        foreach (var walkmesh in walkmeshes)
        {
            foreach (var face in walkmesh.Faces)
            {
                //final.Faces.Add(face);
                final.Faces.Add(new MDLFace()
                {
                    Vertex1 = new MDLVertex().SetPosition(face.Point1),
                    Vertex2 = new MDLVertex().SetPosition(face.Point2),
                    Vertex3 = new MDLVertex().SetPosition(face.Point3),
                });
            }
        }

        return final;
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

