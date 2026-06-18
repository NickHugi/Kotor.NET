namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class WallTemplate : ObjectTemplate
{
    public required string DoorFrameID { get; init; }

    public DoorFrameTemplate? DoorFrame => DoorFrameID is not null ? Kit.Manager.Get(KitID).DoorFrame(DoorFrameID) : null;
    public bool CanBeDoor => DoorFrame is not null;
}
