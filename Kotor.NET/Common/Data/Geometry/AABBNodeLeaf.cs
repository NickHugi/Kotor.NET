using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Common.Data.Geometry;

public class AABBNodeLeaf : AABBNode
{
    public BWMFace Face { get; init; }

    public AABBNodeLeaf(BoundingBox boundingBox, BWMFace face) : base(boundingBox)
    {
        Face = face;
    }

    public override IEnumerable<AABBNode> GetDescendants()
    {
        return [];
    }
}
