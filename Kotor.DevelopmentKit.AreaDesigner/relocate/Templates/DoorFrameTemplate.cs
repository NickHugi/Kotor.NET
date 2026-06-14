namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class DoorFrameTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Group { get; init; }
    public required string Model { get; init; }
    public required DoorFrameHookTemplate[] Hooks { get; init; }
}
