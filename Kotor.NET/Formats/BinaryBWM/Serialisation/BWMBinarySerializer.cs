using System;
using System.Collections.Generic;
using System.Linq;
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

        var aabbBuilder = new AABBTreeBuilder();
        var rootAABB = aabbBuilder.Build(_bwm.Faces.ToList());
        var flatten = new List<AABBNode>() { rootAABB };
        var checkNext = new List<AABBNode>() { rootAABB };
        while (checkNext.Count > 0)
        {
            var node = checkNext.First();
            checkNext.RemoveAt(0);

            flatten.Add(node);

            if (node.LeftNode is not null)
                checkNext.Add(node.LeftNode);
            if (node.RightNode is not null)
                checkNext.Add(node.RightNode);
        }
        

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

    private List<List<Edge>> GetPerimeters()
    {
        var perimeters = new List<List<Edge>>();
        var walkable = _bwm.Faces.ToList();
        var pending = walkable.ToList();

        while (true)
        {
            var face = GetBestStartingFace(pending);
            if (face is null)
                break;

            var perimeter = new List<Edge>();

            var hasBorder = face.Edge1.Adjacent is null || face.Edge2.Adjacent is null || face.Edge3.Adjacent is null;
            var hadBorder = hasBorder;

            while (hasBorder)
            {
                pending.RemoveAt(0);

                if (face.Edge1.Adjacent is null)
                {
                    perimeter.Add(face.Edge1);
                }
                if (face.Edge2.Adjacent is null)
                {
                    perimeter.Add(face.Edge2);
                }
                if (face.Edge3.Adjacent is null)
                {
                    perimeter.Add(face.Edge3);
                }

                if (face.Edge1.Adjacent is not null && pending.Contains(face.Edge1.Adjacent))
                {
                    if (pending.Contains(face.Edge1.Adjacent))
                        face = face.Edge1.Adjacent;
                }
                else if (face.Edge2.Adjacent is not null && pending.Contains(face.Edge2.Adjacent))
                {
                    if (pending.Contains(face.Edge2.Adjacent))
                        face = face.Edge2.Adjacent;
                }
                else if (face.Edge3.Adjacent is not null && pending.Contains(face.Edge3.Adjacent))
                {
                    if (pending.Contains(face.Edge3.Adjacent))
                        face = face.Edge3.Adjacent;
                }
                else
                {
                    break;
                }

                //var deadend1 = face.Edge1.Adjacent is null || (face.Edge1.Adjacent is not null && !pending.Contains(face.Edge1.Adjacent));
                //var deadend2 = face.Edge2.Adjacent is null || (face.Edge2.Adjacent is not null && !pending.Contains(face.Edge2.Adjacent));
                //var deadend3 = face.Edge3.Adjacent is null || (face.Edge3.Adjacent is not null && !pending.Contains(face.Edge3.Adjacent));
                //if (deadend1 && deadend2 && deadend3)
                //    break;
            }

            if (hadBorder)
            {
                perimeters.Add(perimeter);
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
