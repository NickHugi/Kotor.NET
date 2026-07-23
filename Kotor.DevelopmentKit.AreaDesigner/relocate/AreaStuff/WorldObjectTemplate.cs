using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

[DebuggerDisplay("{KitID}.{TemplateID} type({Type})")]
public class WorldObjectTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
    public required WorldObjectType Type { get; init; }

    public required string ClassID { get; init; }

    public required string Name { get; init; }

    public required string Model { get; init; }

    public required MagnetTemplate[] Magnets { get; init; }

    public string DoorframeKitID { get; init; }
    public string DoorframeTemplateID { get; init; }
    public string DoorframeClassID { get; init; }
    public bool CanBeDoor => !string.IsNullOrEmpty(DoorframeTemplateID);
    public WorldObjectTemplate? DoorFrame => string.IsNullOrEmpty(DoorframeTemplateID) ? null : Kit.Manager.Get(DoorframeKitID).Object(DoorframeTemplateID);
}
