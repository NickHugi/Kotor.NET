using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class TileTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string DefaultFloorID { get; init; }
    public required string DefaultCeilingID { get; init; }
    public required WallHookTemplate[] Walls { get; init; }
    public required InnerCornerHookTemplate[] InnerCorners { get; init; }
    public required OuterCornerHookTemplate[] OuterCorners { get; init; }
    public required Vector3[] CeilingHooks { get; init; }

    public FloorTemplate Floor => Kit.Manager.Get(KitID).Floor(DefaultFloorID);
    public CeilingTemplate Ceiling => Kit.Manager.Get(KitID).Ceiling(DefaultCeilingID);
}
