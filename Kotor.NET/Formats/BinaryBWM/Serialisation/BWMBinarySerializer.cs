using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
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

        var perimeters = GetPerimeters();
        foreach (var perimeter in perimeters)
        {
            foreach (var edge in perimeter)
            {
                binary.Edges.Add(new()
                {
                    // TODO - dont know how much of this logic is correct, review when less of a headache
                    EdgeIndex = (_bwm.Faces.IndexOf(edge._face) * 3) + edge._index,
                    Transition = (edge.Adjacent is null) ? -1 : ((_bwm.Faces.IndexOf(edge.Adjacent) * 3) + edge._index)
                });
            }
            binary.Perimeters.Add(binary.Edges.Count);
        }

        var aabbs = _bwm.GenerateTree().GetDescendants().ToList();
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

        List<Vector3> vertices =
        [
            .._bwm.Faces.Select(x => x.Point1),
            .._bwm.Faces.Select(x => x.Point2),
            .._bwm.Faces.Select(x => x.Point3),
        ];

        for (int i = 0; i < _bwm.Faces.Count; i++)
        {
            var face = _bwm.Faces[i];
            binary.FaceMaterials.Add((int)face.Material);
            binary.FaceNormals.Add(face.Normal);
            binary.FacePlaneDistances.Add(face.Distance);
            binary.FaceIndices.Add(new()
            {
                Index1 = vertices.IndexOf(face.Point1),
                Index2 = vertices.IndexOf(face.Point2),
                Index3 = vertices.IndexOf(face.Point3),
            });
            binary.Edges.Add(new() { EdgeIndex = i+0, Transition = face.Edge1.Transition });
            binary.Edges.Add(new() { EdgeIndex = i+1, Transition = face.Edge2.Transition });
            binary.Edges.Add(new() { EdgeIndex = i+2, Transition = face.Edge3.Transition });
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
        var perimeters = new List<List<Edge>>();
        var islands = GetWalkableIslands();

        while (islands.Count > 0)
        {
            var island = islands.First();
            islands.RemoveAt(0);

            var perimeter = new List<Edge>();
            perimeters.Add(perimeter);

            while (island.Count > 0)
            {
                var face = island.First();
                island.RemoveAt(0);

                if (face.Edge1.Adjacent is null || !face.Edge1.Adjacent.Material.IsWalkable())
                    perimeter.Add(face.Edge1);
                if (face.Edge2.Adjacent is null || !face.Edge2.Adjacent.Material.IsWalkable())
                    perimeter.Add(face.Edge2);
                if (face.Edge3.Adjacent is null || !face.Edge3.Adjacent.Material.IsWalkable())
                    perimeter.Add(face.Edge3);
            }
        }

        return perimeters;
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
}
