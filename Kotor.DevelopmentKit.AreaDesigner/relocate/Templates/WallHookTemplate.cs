using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class WallHookTemplate : HookTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);

    public int[] AdjacentWalls { get; init; } = [];
}
