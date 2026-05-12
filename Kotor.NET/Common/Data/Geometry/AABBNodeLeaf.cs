using Kotor.NET.Resources.KotorBWM;

namespace Kotor.NET.Common.Data.Geometry;

public class AABBNodeLeaf : AABBNode
{
    public IFace Face { get; init; }

    public AABBNodeLeaf(BoundingBox boundingBox, IFace face) : base(boundingBox)
    {
        Face = face;
    }

    public override IEnumerable<AABBNode> GetDescendants()
    {
        return [];
    }
}
