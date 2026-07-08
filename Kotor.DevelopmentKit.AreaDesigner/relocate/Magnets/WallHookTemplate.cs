using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;

public class WallHookTemplate : UltimateMagnetTemplate
{
    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);
}
