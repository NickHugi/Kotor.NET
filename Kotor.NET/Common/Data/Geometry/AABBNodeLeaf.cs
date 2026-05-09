namespace Kotor.NET.Common.Data.Geometry;

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
