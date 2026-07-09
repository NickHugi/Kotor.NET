using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class UltimateWorldObjectTemplate
{
    public required string KitID { get; init; }
    public required string TemplateID { get; init; }
    public required WorldObjectType Type { get; init; }

    public required string ClassID { get; init; }

    public required string Name { get; init; }

    public required string Model { get; init; }

    public required UltimateMagnetTemplate[] Magnets { get; init; }

    public string DoorframeKitID { get; init; }
    public string DoorframeTemplateID { get; init; }
    public string DoorframeClassID { get; init; }
    public bool CanBeDoor => !string.IsNullOrEmpty(DoorframeTemplateID);
    public UltimateWorldObjectTemplate? DoorFrame => string.IsNullOrEmpty(DoorframeTemplateID) ? null : Kit.Manager.Get(DoorframeKitID).Object(DoorframeTemplateID);
}
