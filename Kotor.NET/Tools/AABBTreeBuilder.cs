using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;

namespace Kotor.NET.Tools;

public class AABBTreeBuilder : IAABBTreeBuilder
{
    public AABBNode Build(List<Face> faces)
    {
        return BuildNode(faces);
    }

    private AABBNode BuildNode(List<Face> faces)
    {
        var boundingBox = Face.BuildBuildingBoxFromFaces(faces);

        if (faces.Count == 1)
        {
            return new AABBNode(faces.First(), boundingBox);
        }
        else
        {
            var bestAxis = GetBestAxisToSplit(boundingBox, faces);
            var (left, right, axis) = SplitFaces(faces, bestAxis, boundingBox);
            return new AABBNode(BuildNode(left), BuildNode(right), axis, boundingBox);
        }
    }

    private (List<Face> Left, List<Face> Right, Axis axis) SplitFaces(List<Face> faces, Axis bestAxis, BoundingBox boundingBox)
    {
        var axis = GetBestAxisToSplit(boundingBox, faces);
        List<Face> left, right;

        for (int i = 0; i < 3; i++)
        {
            (left, right) = SplitFacesByAxis(faces, axis, boundingBox);
            if (left.Count == 0 || right.Count == 0)
            {
                return (left, right, axis);
            }
            else
            {
                axis = (Axis)(((int)axis + 1) % 3);
            }
        }
        // else
        (left, right) = SplitFacesEvenly(faces, axis);
        return (left, left, axis);
    }
    private (List<Face> Left, List<Face> Right) SplitFacesByAxis(List<Face> faces, Axis axis, BoundingBox boundingBox)
    {
        List<Face> left = new(), right = new();

        foreach (var face in faces)
        {
            if (face.Centre[axis] < boundingBox.Centre[axis])
            {
                left.Add(face);
            }
            else
            {
                right.Add(face);
            }
        }

        return (left, right);
    }
    private (List<Face> Left, List<Face> Right) SplitFacesEvenly(List<Face> faces, Axis axis)
    {
        var split = faces.Count / 2;
        var left = faces.Take(split).ToList();
        var right = faces.Skip(split).ToList();
        return (left, right);
    }

    private Axis GetBestAxisToSplit(BoundingBox boundingBox, List<Face> faces)
    {
        var axis = boundingBox.GetLongestAxis();

        if (AreFacesCoplanar(faces, axis, boundingBox))
        {
            axis = boundingBox.GetSecondLongestAxis();
        }

        return axis;
    }

    private bool AreFacesCoplanar(List<Face> faces, Axis axis, BoundingBox boundingBox)
    {

        var coplanar = true;
        faces.ForEach(face => coplanar = coplanar && face.Centre.Equals(boundingBox.Centre, 1e-4f));
        return coplanar;
    }
}

