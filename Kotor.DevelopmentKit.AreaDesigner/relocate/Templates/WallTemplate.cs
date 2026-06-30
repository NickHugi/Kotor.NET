namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class WallTemplate : WorldObjectTemplate
{
    public required string DoorframeKitID { get; init; }
    public required string DoorframeTemplateID { get; init; }
    public required string DoorframeClassID { get; init; }

    public DoorFrameTemplate? DoorFrame => DoorframeTemplateID is not null ? Kit.Manager.Get(DoorframeKitID).DoorFrame(DoorframeTemplateID) : null;
    public bool CanBeDoor => DoorFrame is not null;
}
