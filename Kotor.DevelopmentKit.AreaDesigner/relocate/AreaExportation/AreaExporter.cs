using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Clipper2Lib;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;

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
        var finalWalkmesh = WalkmeshBuilder.Instance.BuildNode(walkmeshes);
        mdl.DeleteWalkmesh();
        mdl.Root.Children.Add(finalWalkmesh);

        mdl.Root.GetAllDescendants().OfType<MDLTrimeshNode>().ToList().ForEach(x => x.LightmapTexture = "");
        mdl.Root.GetAllDescendants().Select((x, i) => x.Name = i.ToString()).ToArray();
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

    public MDLTrimeshNode BuildNode(IEnumerable<MDLWalkmeshNode> walkmeshes)
    {
        var walkmeshList = walkmeshes.Select(x =>
        {
            return x.Faces.Select(x => new Face
            {
                V1 = new(x.Vertex1.Position.Value.X, x.Vertex1.Position.Value.Y),
                V2 = new(x.Vertex2.Position.Value.X, x.Vertex2.Position.Value.Y),
                V3 = new(x.Vertex3.Position.Value.X, x.Vertex3.Position.Value.Y),
            }).ToArray();
        }).ToList();

        var latestWalkmesh = walkmeshList.First();
        walkmeshList = walkmeshList.Skip(1).ToList();

        while (walkmeshList.Count > 0)
        {
            latestWalkmesh = SubtractWalkmesh(latestWalkmesh, walkmeshList.First());
            walkmeshList.RemoveAt(0);
        }

        var newNode = new MDLTrimeshNode("walkmesh");
        newNode.EnableVertices();
        newNode.Faces.AddRange(latestWalkmesh.Select(x =>
        {
            return new MDLFace()
            {
                Vertex1 = new MDLVertex().SetPosition(new Vector3(x.V1, 0)),
                Vertex2 = new MDLVertex().SetPosition(new Vector3(x.V2, 0)),
                Vertex3 = new MDLVertex().SetPosition(new Vector3(x.V3, 0)),
            };
        }));
        return newNode;
    }

    public Face[] SubtractWalkmesh(Face[] walkmesh, Face[] clip)
    {
        var result = walkmesh.ToList();
        var remainingClips = clip.ToList();

        while (remainingClips.Count > 0)
        {
            var subtract = remainingClips.First();
            remainingClips.RemoveAt(0);

            var gothru = result.ToList();
            foreach (var face in gothru)
            {
                result.Remove(face);
                var newFaces = TriangleClipper.Subtract(face, subtract);
                result.AddRange(newFaces);
            }
        }

        return result.ToArray();
    }
}



public static class TriangleClipper
{
    const float EPS = 1e-5f;

    public static List<Face> Subtract(Face subject, Face clip)
    {
        List<Face> pieces = new List<Face> { subject };

        // Split subject by each edge of clip
        for (int i = 0; i < 3; i++)
        {
            Vector2 a = clip.ToArray()[i];
            Vector2 b = clip.ToArray()[(i + 1) % 3];

            List<Face> newPieces = new List<Face>();

            foreach (var tri in pieces)
            {
                SplitTriangle(tri, a, b, newPieces);
            }

            pieces = newPieces;
        }

        // Remove triangles that are inside the clip
        List<Face> result = new List<Face>();
        foreach (var tri in pieces)
        {
            Vector2 center = (tri.V1 + tri.V2 + tri.V3) / 3f;
            if (!PointInTriangle(center, clip.V1, clip.V2, clip.V3))
                result.Add(tri);
        }

        return result;
    }

    private static void SplitTriangle(Face tri, Vector2 a, Vector2 b, List<Face> output)
    {
        var verts = tri.ToArray();

        List<Vector2> left = new List<Vector2>();
        List<Vector2> right = new List<Vector2>();

        for (int i = 0; i < 3; i++)
        {
            Vector2 curr = verts[i];
            Vector2 next = verts[(i + 1) % 3];

            int sideCurr = SideClass(a, b, curr);
            int sideNext = SideClass(a, b, next);

            // ON the line → belongs to BOTH sides
            if (sideCurr >= 0) left.Add(curr);
            if (sideCurr <= 0) right.Add(curr);

            // Edge crosses the line
            if (sideCurr * sideNext < 0)
            {
                Vector2 intersect = LineIntersect(curr, next, a, b);
                left.Add(intersect);
                right.Add(intersect);
            }
        }

        TriangulateSafe(left, output);
        TriangulateSafe(right, output);
    }

    private static Vector2 LineIntersect(Vector2 p1, Vector2 p2, Vector2 a, Vector2 b)
    {
        float A1 = p2.Y - p1.Y;
        float B1 = p1.X - p2.X;
        float C1 = A1 * p1.X + B1 * p1.Y;

        float A2 = b.Y - a.Y;
        float B2 = a.X - b.X;
        float C2 = A2 * a.X + B2 * a.Y;

        float det = A1 * B2 - A2 * B1;
        if (Math.Abs(det) < 1e-6f) return p1;

        return new Vector2(
            (B2 * C1 - B1 * C2) / det,
            (A1 * C2 - A2 * C1) / det
        );
    }

    private static void TriangulateConvex(List<Vector2> poly, List<Face> output)
    {
        if (poly.Count < 3) return;

        for (int i = 1; i < poly.Count - 1; i++)
        {
            output.Add(new Face(poly[0], poly[i], poly[i + 1]));
        }
    }

    private static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
    {
        float s1 = Side(a, b, p);
        float s2 = Side(b, c, p);
        float s3 = Side(c, a, p);

        bool hasNeg = (s1 < 0) || (s2 < 0) || (s3 < 0);
        bool hasPos = (s1 > 0) || (s2 > 0) || (s3 > 0);

        return !(hasNeg && hasPos);
    }

    private static float Side(Vector2 a, Vector2 b, Vector2 p)
    {
        return (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
    }

    private static int SideClass(Vector2 a, Vector2 b, Vector2 p)
    {
        float s = Side(a, b, p);
        if (s > EPS) return 1;
        if (s < -EPS) return -1;
        return 0; // ON the line
    }

    private static float Area(Vector2 a, Vector2 b, Vector2 c)
    {
        return MathF.Abs((b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X));
    }

    private static void TriangulateSafe(List<Vector2> poly, List<Face> output)
    {
        if (poly.Count < 3) return;

        for (int i = 1; i < poly.Count - 1; i++)
        {
            var a = poly[0];
            var b = poly[i];
            var c = poly[i + 1];

            if (Area(a, b, c) > EPS) // discard degenerate
            {
                output.Add(new Face(a, b, c));
            }
        }
    }

    private static List<Vector2> Dedup(List<Vector2> pts)
    {
        List<Vector2> result = new List<Vector2>();

        foreach (var p in pts)
        {
            bool exists = false;
            foreach (var q in result)
            {
                if (Vector2.DistanceSquared(p, q) < EPS * EPS)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists) result.Add(p);
        }

        return result;
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

    private static bool TryIntersect(
        Vector2 p1, Vector2 p2,
        Vector2 q1, Vector2 q2,
        out Vector2 result)
    {
        float A1 = p2.Y - p1.Y;
        float B1 = p1.X - p2.X;
        float C1 = A1 * p1.X + B1 * p1.Y;

        float A2 = q2.Y - q1.Y;
        float B2 = q1.X - q2.X;
        float C2 = A2 * q1.X + B2 * q1.Y;

        float det = A1 * B2 - A2 * B1;

        if (Math.Abs(det) < EPS)
        {
            result = default;
            return false; // parallel or colinear
        }

        float x = (B2 * C1 - B1 * C2) / det;
        float y = (A1 * C2 - A2 * C1) / det;

        result = new Vector2(x, y);

        return OnSegment(p1, p2, result) && OnSegment(q1, q2, result);
    }

    private static bool OnSegment(Vector2 a, Vector2 b, Vector2 p)
    {
        return p.X >= Math.Min(a.X, b.X) - EPS &&
               p.X <= Math.Max(a.X, b.X) + EPS &&
               p.Y >= Math.Min(a.Y, b.Y) - EPS &&
               p.Y <= Math.Max(a.Y, b.Y) + EPS;
    }
}



public struct Face
{
    public Vector2 V1 { get; init; }
    public Vector2 V2 { get; init; }
    public Vector2 V3 { get; init; }

    public Vector2[] ToArray()
    {
        var v =new List<Vector2>() { V1, V2, V3 };
        EnsureCCW(v);
        return v.ToArray();
    }

    public Face()
    {
    }
    public Face(Vector2 v1, Vector2 v2, Vector2 v3)
    {
        V1 = v1;
        V2 = v2;
        V3 = v3;
    }

    public bool IsLine()
    {
        return (V1.Equals(V2) && V1.Equals(V3))
            || (V2.Equals(V3) && V2.Equals(V3))
            || (V1.Equals(V2) && V1.Equals(V3));
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
