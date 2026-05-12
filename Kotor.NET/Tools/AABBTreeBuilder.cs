using System.Numerics;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Data.Geometry;
using Kotor.NET.Extensions;
using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Tools;

public class AABBTreeBuilder : IAABBTreeBuilder
{
    public AABBNode Build(List<IFace> faces)
    {
        if (faces.Count == 0)
            return null;

        return BuildNode(faces);
    }

    private AABBNode BuildNode(List<IFace> faces)
    {
        var boundingBox = new BoundingBox(faces);

        if (faces.Count == 1)
        {
            return new AABBNodeLeaf(boundingBox, faces.First());
        }

        var (left, right, axis) = SplitFaces(faces, boundingBox);
        return new AABBNodeBranch(boundingBox, BuildNode(left), BuildNode(right), axis);
    }

    private (List<IFace> Left, List<IFace> Right, Axis Axis) SplitFaces(List<IFace> faces, BoundingBox boundingBox)
    {
        var axis = GetBestAxisToSplit(boundingBox, faces);

        for (int i = 0; i < 3; i++)
        {
            var (left, right) = SplitFacesByAxis(faces, axis, boundingBox);

            if (left.Count > 0 && right.Count > 0)  
            {
                return (left, right, axis);
            }

            axis = (Axis)(((int)axis + 1) % 3); 
        }

        var (evenLeft, evenRight) = SplitFacesEvenly(faces, axis);
        return (evenLeft, evenRight, axis);
    }

    private (List<IFace> Left, List<IFace> Right) SplitFacesByAxis(List<IFace> faces, Axis axis, BoundingBox boundingBox)
    {
        var midpoint = boundingBox.Center.GetComponent(axis);
        var left = new List<IFace>();
        var right = new List<IFace>();

        foreach (var face in faces)
        {
            if (face.Center.GetComponent(axis) < midpoint)
                left.Add(face);
            else
                right.Add(face);
        }

        return (left, right);
    }

    private (List<IFace> Left, List<IFace> Right) SplitFacesEvenly(List<IFace> faces, Axis axis)
    {
        var split = faces.Count / 2;
        return (faces.Take(split).ToList(), faces.Skip(split).ToList());
    }

    private Axis GetBestAxisToSplit(BoundingBox boundingBox, List<IFace> faces)
    {
        var axis = boundingBox.GetLongestAxis();

        if (AreFacesCoplanarOnAxis(faces, axis))
        {
            axis = boundingBox.GetSecondLongestAxis();
        }

        return axis;
    }

    private bool AreFacesCoplanarOnAxis(List<IFace> faces, Axis axis)
    {
        if (faces.Count == 0)
            return true;

        var reference = faces[0].Center.GetComponent(axis);
        return faces.All(face => face.Center.GetComponent(axis).Equals(reference, 1e-4f));
    }
}
