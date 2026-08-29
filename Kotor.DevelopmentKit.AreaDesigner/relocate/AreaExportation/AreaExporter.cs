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
using Kotor.NET.Resources.KotorBWM;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using Kotor.NET.Tools;
using NetTopologySuite.Geometries;
using NetTopologySuite.GeometriesGraph;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;

public static class AreaExporter
{
    public static MDL RoomToMDL(Room room)
    {
        var mdl = new MDL();
        mdl.Name = "test";

        mdl.Root.Children.AddRange(room.AllObjects.Where(x => x.Visible).Where(x => !string.IsNullOrWhiteSpace(x.Template.Model)).Select(WorldObjectToMDLNode));

        var walkmeshes = mdl.Root.GetAllDescendants().OfType<MDLWalkmeshNode>();
        var newWalkmesh = WalkmeshBuilder.Instance.Bake2(walkmeshes.ToList());

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
            Material = x.Material
        }).ToArray();
    }

    public MDLWalkmeshNode Bake2(IEnumerable<MDLWalkmeshNode> walkmeshNodes)
    {
        var unprocessed = walkmeshNodes.SelectMany(Simplify).ToList();
        List<Triangle> processed = TriangleOverlay.RemoveCoplanarOverlaps(
            unprocessed,
            angleToleranceDegrees: 5f,
            planeDistanceTolerance: 0.01f,
            overlaySnapTolerance: 0.01);

        var newNode = new MDLWalkmeshNode("walkmesh");
        newNode.EnableVertices();
        newNode.Faces.AddRange(processed.Select(x =>
        {
            return new MDLFace()
            {
                Vertex1 = new MDLVertex().SetPosition(x.Point1),
                Vertex2 = new MDLVertex().SetPosition(x.Point2),
                Vertex3 = new MDLVertex().SetPosition(x.Point3),
                Material = x.Material
            };
        }));
        return newNode;
    }

    public void StitchWalkmeshes(List<BWM> walkmeshes)
    {
        var edgesByWalkmesh = walkmeshes
        .Select(walkmesh => walkmesh.Faces
            .SelectMany(face => new[]
            {
                face.Edge1,
                face.Edge2,
                face.Edge3
            })
            .ToList())
        .ToList();

        for (var i = 0; i < walkmeshes.Count; i++)
        {
            for (var j = i + 1; j < walkmeshes.Count; j++)
            {
                foreach (var edge1 in edgesByWalkmesh[i])
                {
                    foreach (var edge2 in edgesByWalkmesh[j])
                    {
                        if (!edge1.Equals(edge2))
                            continue;

                        edge1.Transition = j;
                        edge2.Transition = i;
                    }
                }
            }
        }
    }
}
