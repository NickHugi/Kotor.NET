using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public abstract class WorldObjectTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
    public required string ClassID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
    public required BaseMagnetTemplate[] Magnets { get; init; }
}
