using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Extensions;
using Kotor.NET.Resources.KotorBWM;
using Kotor.NET.Tools;

namespace Kotor.NET.Formats.BinaryBWM.Serialisation;

public class BWMBinarySerializer
{
    private BWM _bwm { get; }

    public BWMBinarySerializer(BWM bwm)
    {
        _bwm = bwm;
    }

    public BWMBinary Serialize()
    {
        var binary = new BWMBinary();

        binary.FileHeader.Position = _bwm.Position;
        binary.FileHeader.WalkmeshType = _bwm.WalkmeshType == BWMWalkmeshType.Area ? 1 : 0;

        var aabbs = _bwm.GenerateTree()?.GetDescendants()?.ToList() ?? [];
        binary.AABBs = aabbs.Select(x =>
        {
            if (x is AABBNodeBranch branch)
            {
                return new BWMBinaryAABBNode()
                {
                    FaceIndex = -1,
                    BoundingBoxMin = branch.BoundingBox.Min,
                    BoundingBoxMax = branch.BoundingBox.Max,
                    LeftChildIndex = aabbs.IndexOf(branch.LeftNode),
                    RightChildIndex = aabbs.IndexOf(branch.RightNode),
                    MostSignificantPlane = (int)branch.MostSignificantPlane,
                    UnknownAlways4 = 4
                };
            }
            else if (x is AABBNodeLeaf leaf)
            {
                return new BWMBinaryAABBNode()
                {
                    FaceIndex = _bwm.Faces.IndexOf(leaf.Face),
                    BoundingBoxMin = leaf.BoundingBox.Min,
                    BoundingBoxMax = leaf.BoundingBox.Max,
                    LeftChildIndex = -1,
                    RightChildIndex = -1,
                    MostSignificantPlane = 0,
                    UnknownAlways4 = 4
                };
            }
            else
            {
                throw new InvalidOperationException("Invalid AABB node type");
            }
        }).ToList();

        binary.Vertices =
        [
            .._bwm.Faces.Select(x => x.Point1),
            .._bwm.Faces.Select(x => x.Point2),
            .._bwm.Faces.Select(x => x.Point3),
        ];
        binary.Vertices = binary.Vertices.Distinct().ToList();

        for (int i = 0; i < _bwm.Faces.Count; i++)
        {
            var face = _bwm.Faces[i];

            binary.FaceMaterials.Add((int)face.Material);
            binary.FaceNormals.Add(face.Normal);
            binary.FacePlaneDistances.Add(face.Distance);
            binary.FaceIndices.Add(new()
            {
                Index1 = binary.Vertices.IndexOf(face.Point1),
                Index2 = binary.Vertices.IndexOf(face.Point2),
                Index3 = binary.Vertices.IndexOf(face.Point3),
            });

            if (face.Material.IsWalkable())
            {
                binary.Adjacencies.Add(new()
                {
                    Index1 = (face.Edge1.Adjacent is null) ? -1 : _bwm.Faces.IndexOf(face.Edge1.Adjacent),
                    Index2 = (face.Edge2.Adjacent is null) ? -1 : _bwm.Faces.IndexOf(face.Edge2.Adjacent),
                    Index3 = (face.Edge3.Adjacent is null) ? -1 : _bwm.Faces.IndexOf(face.Edge3.Adjacent),
                });
            }
        }

        var perimeters = GetPerimeters();
        foreach (var perimeter in perimeters)
        {
            foreach (var edge in perimeter)
            {
                binary.Edges.Add(new()
                {
                    EdgeIndex = (_bwm.Faces.IndexOf(edge._face) * 3) + edge._index,
                    Transition = (edge.Adjacent is not null) ? -1 : edge.Transition
                });
            }
            binary.Perimeters.Add(binary.Edges.Count);
        }

        binary.Recalculate();

        return binary;
    }

    private List<List<Face>> GetWalkableIslands()
    {
        var islands = new List<List<Face>>();
        var pending = _bwm.Faces.Where(x => x.Material.IsWalkable()).ToList();

        while (pending.Count > 0)
        {
            var island = new List<Face>();
            islands.Add(island);

            var scan = new List<Face>() { pending.First() };

            while (scan.Count > 0)
            {
                var face = scan.First();
                scan.RemoveAt(0);
                pending.Remove(face);

                island.Add(face);

                if (face.Edge1.Adjacent?.Material.IsWalkable() == true && !island.Contains(face.Edge1.Adjacent) && !scan.Contains(face.Edge1.Adjacent))
                    scan.Add(face.Edge1.Adjacent);
                if (face.Edge2.Adjacent?.Material.IsWalkable() == true && !island.Contains(face.Edge2.Adjacent) && !scan.Contains(face.Edge2.Adjacent))
                    scan.Add(face.Edge2.Adjacent);
                if (face.Edge3.Adjacent?.Material.IsWalkable() == true && !island.Contains(face.Edge3.Adjacent) && !scan.Contains(face.Edge3.Adjacent))
                    scan.Add(face.Edge3.Adjacent);
            }
        }

        return islands;
    }

    private List<List<Edge>> GetPerimeters()
    {
        var pending = _bwm.Faces
            .Where(x => x.Material.IsWalkable())
            .SelectMany(x => new[] { x.Edge1, x.Edge2, x.Edge3 })
            .Where(e => e.Adjacent is null || !e.Adjacent.Material.IsWalkable())
            .ToList();

        var result = new List<List<Edge>>();

        while (pending.Count > 0)
        {
            var perimeter = new List<Edge>();
            result.Add(perimeter);

            var target = pending.First();
            var cursor = target.Point2;

            while (true)
            {
                pending.Remove(target);
                perimeter.Add(target);

                target = null;
                foreach (var checkEdge in pending)
                {
                    if (cursor.ApproximatelyEquals(checkEdge.Point1))
                    {
                        cursor = checkEdge.Point2;
                        target = checkEdge;
                        break;
                    }
                    if (cursor.ApproximatelyEquals(checkEdge.Point2))
                    {
                        cursor = checkEdge.Point1;
                        target = checkEdge;
                        break;
                    }
                }

                if (target is null)
                    break;
            }
        }


        return result;

        //foreach (var startEdge in pending)
        //{
        //    if (visited.Contains(startEdge))
        //        continue;

        //    var loop = new List<Edge>();
        //    var currentEdge = startEdge;

        //    // pick a direction to start
        //    var currentVertex = currentEdge.Point1;

        //    while (true)
        //    {
        //        loop.Add(currentEdge);
        //        visited.Add(currentEdge);

        //        // move to the other vertex of the edge
        //        var nextVertex = currentEdge.Point1.Equals(currentVertex)
        //            ? currentEdge.Point1
        //            : currentEdge.Point2;

        //        // find next edge connected to this vertex that we haven't used
        //        var nextEdge = vertexToEdges[nextVertex]
        //            .FirstOrDefault(e => !visited.Contains(e));

        //        if (nextEdge == null)
        //            break; // open boundary (shouldn't happen in clean mesh)

        //        // advance
        //        currentVertex = nextVertex;
        //        currentEdge = nextEdge;

        //        // loop closed
        //        if (currentEdge == startEdge)
        //            break;
        //    }

        //    result.Add(loop);
        //}
    }

    private Face GetBestStartingFace(List<Face> faces)
    {
        int minAdajacencies = int.MaxValue;
        Face bestFace = null;

        foreach (var face in faces)
        {
            int adjacencies = 0;
            if (face.Edge1.Adjacent is not null)
                adjacencies++;
            if (face.Edge2.Adjacent is not null)
                adjacencies++;
            if (face.Edge3.Adjacent is not null)
                adjacencies++;

            if (adjacencies < minAdajacencies)
            {
                minAdajacencies = adjacencies;
                bestFace = face;
            }
        }

        return bestFace;
    }
    private bool IsAllAdjacentWalkable(Face face)
    {
        return face.Edge1.Adjacent?.Material.IsWalkable() == true
            && face.Edge2.Adjacent?.Material.IsWalkable() == true
            && face.Edge3.Adjacent?.Material.IsWalkable() == true;
    }
}
