using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;

public class WallHookTemplate : BaseMagnetTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);

    public int[] AdjacentWalls { get; init; } = [];
}
