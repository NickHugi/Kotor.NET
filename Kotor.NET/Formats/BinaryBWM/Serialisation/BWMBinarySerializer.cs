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

        var rootAABB = _bwm.GenerateTree();
        List<AABBNode> aabbs = (rootAABB is null) ? [] : [rootAABB, .. rootAABB.GetDescendants()];
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
            .. _bwm.Faces.SelectMany(x => new List<Vector3>() {x.Point1, x.Point2, x.Point3 })
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
                    Index1 = (face.Edge1.AdjacentEdge is null || !face.Edge1.AdjacentFace!.Material.IsWalkable()) ? -1 : (_bwm.Faces.IndexOf(face.Edge1.AdjacentEdge.Face) * 3) + face.Edge1.AdjacentEdge._index,
                    Index2 = (face.Edge2.AdjacentEdge is null || !face.Edge2.AdjacentFace!.Material.IsWalkable()) ? -1 : (_bwm.Faces.IndexOf(face.Edge2.AdjacentEdge.Face) * 3) + face.Edge2.AdjacentEdge._index,
                    Index3 = (face.Edge3.AdjacentEdge is null || !face.Edge3.AdjacentFace!.Material.IsWalkable()) ? -1 : (_bwm.Faces.IndexOf(face.Edge3.AdjacentEdge.Face) * 3) + face.Edge3.AdjacentEdge._index,
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
                    Transition = (edge.AdjacentFace is not null) ? -1 : edge.Transition
                });
            }
            binary.Perimeters.Add(binary.Edges.Count);
        }

        binary.Recalculate();

        return binary;
    }

    private List<List<Edge>> GetPerimeters()
    {
        var pending = _bwm.Faces
            .Where(x => x.Material.IsWalkable())
            .SelectMany(x => new[] { x.Edge1, x.Edge2, x.Edge3 })
            .Where(e => e.AdjacentFace is null || !e.AdjacentFace.Material.IsWalkable())
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
    }
}
