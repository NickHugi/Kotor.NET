using System.Numerics;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Extensions;

namespace Kotor.NET.Tools;

public class AABBTreeBuilder : IAABBTreeBuilder
{
    public AABBNode Build(List<Face> faces)
    {
        if (faces.Count == 0)
            return null;

        return BuildNode(faces);
    }

    private AABBNode BuildNode(List<Face> faces)
    {
        var boundingBox = new BoundingBox(faces);

        if (faces.Count == 1)
        {
            return new AABBNodeLeaf(boundingBox, faces.First());
        }

        var (left, right, axis) = SplitFaces(faces, boundingBox);
        return new AABBNodeBranch(boundingBox, BuildNode(left), BuildNode(right), axis);
    }

    // Tries each axis in order of preference, falling back to an even split if
    // no axis produces a non-degenerate partition.
    private (List<Face> Left, List<Face> Right, Axis Axis) SplitFaces(List<Face> faces, BoundingBox boundingBox)
    {
        var axis = GetBestAxisToSplit(boundingBox, faces);

        for (int i = 0; i < 3; i++)
        {
            var (left, right) = SplitFacesByAxis(faces, axis, boundingBox);

            if (left.Count > 0 && right.Count > 0)   // FIX: was inverted — good split, return it
            {
                return (left, right, axis);
            }

            axis = (Axis)(((int)axis + 1) % 3);       // try the next axis
        }

        // No axis produced a valid split; divide the list in half
        var (evenLeft, evenRight) = SplitFacesEvenly(faces, axis);
        return (evenLeft, evenRight, axis);             // FIX: was (left, left, axis)
    }

    private (List<Face> Left, List<Face> Right) SplitFacesByAxis(List<Face> faces, Axis axis, BoundingBox boundingBox)
    {
        var midpoint = boundingBox.Centre.GetComponent(axis);
        var left = new List<Face>();
        var right = new List<Face>();

        foreach (var face in faces)
        {
            if (face.Centre.GetComponent(axis) < midpoint)
                left.Add(face);
            else
                right.Add(face);
        }

        return (left, right);
    }

    private (List<Face> Left, List<Face> Right) SplitFacesEvenly(List<Face> faces, Axis axis)
    {
        var split = faces.Count / 2;
        return (faces.Take(split).ToList(), faces.Skip(split).ToList());
    }

    private Axis GetBestAxisToSplit(BoundingBox boundingBox, List<Face> faces)
    {
        var axis = boundingBox.GetLongestAxis();

        if (AreFacesCoplanarOnAxis(faces, axis))
        {
            axis = boundingBox.GetSecondLongestAxis();
        }

        return axis;
    }

    // Returns true when all face centres share the same position along `axis`,
    // meaning a split on that axis would place every face on one side.
    private bool AreFacesCoplanarOnAxis(List<Face> faces, Axis axis) // FIX: was comparing 3D centres and ignoring axis
    {
        if (faces.Count == 0)
            return true;

        var reference = faces[0].Centre.GetComponent(axis);
        return faces.All(face => face.Centre.GetComponent(axis).Equals(reference, 1e-4f));
    }
}
