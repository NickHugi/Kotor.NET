using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.NET.Common.Data.Geometry;

public abstract class AABBNode
{
    public BoundingBox BoundingBox { get; init; }

    public AABBNode(BoundingBox boundingBox)
    {
        BoundingBox = boundingBox;
    }

    public abstract IEnumerable<AABBNode> GetDescendants();
}

public class AABBNodeLeaf : AABBNode
{
    public Face Face { get; init; }

    public AABBNodeLeaf(BoundingBox boundingBox, Face face) : base(boundingBox)
    {
        Face = face;
    }

    public override IEnumerable<AABBNode> GetDescendants()
    {
        return [];
    }
}

public class AABBNodeBranch : AABBNode
{
    public AABBNode LeftNode { get; init; }
    public AABBNode RightNode { get; init; }
    public Axis MostSignificantPlane { get; init; }

    public AABBNodeBranch(BoundingBox boundingBox, AABBNode left, AABBNode right, Axis mostSignificantPlane) : base(boundingBox)
    {
        LeftNode = left;
        RightNode = right;
        MostSignificantPlane = mostSignificantPlane;
    }

    public override IEnumerable<AABBNode> GetDescendants()
    {
        if (LeftNode != null)
        {
            yield return LeftNode;
            foreach (var descendant in LeftNode.GetDescendants())
            {
                yield return descendant;
            }
        }

        if (RightNode != null)
        {
            yield return RightNode;
            foreach (var descendant in RightNode.GetDescendants())
            {
                yield return descendant;
            }
        }
    }
}
