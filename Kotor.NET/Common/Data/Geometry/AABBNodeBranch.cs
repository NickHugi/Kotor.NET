namespace Kotor.NET.Common.Data.Geometry;

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

    public AABBNodeBranch(BoundingBox boundingBox) : base(boundingBox)
    {
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
