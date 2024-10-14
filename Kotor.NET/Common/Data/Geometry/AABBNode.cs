using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public class AABBNode
{
    public BoundingBox BoundingBox { get; init; }
    public Face? Face { get; init; }
    public Axis MostSignificantPlane { get; init; }
    public AABBNode? LeftNode { get; init; }
    public AABBNode? RightNode { get; init; }

    public AABBNode(Face face, BoundingBox boundingBox)
    {
        Face = face;
        BoundingBox = boundingBox;
    }
    public AABBNode(AABBNode left, AABBNode right, Axis mostSignificantPlane, BoundingBox boundingBox)
    {
        LeftNode = left;
        RightNode = right;
        BoundingBox = boundingBox;
        MostSignificantPlane = mostSignificantPlane;
    }
}

