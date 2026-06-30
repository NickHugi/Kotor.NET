using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WallItem : ObjectItem
{
    public override WorldObjectType WorldObjectType => WorldObjectType.Wall;

    public string DoorframeTemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string DoorframeKitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string DoorframeClassID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallItem() : base()
    {
        DoorframeKitID = "";
        DoorframeTemplateID = "";
        DoorframeClassID = "";
    }
    public WallItem(WallTemplate template) : base(template)
    {
        DoorframeKitID = template.DoorframeKitID;
        DoorframeTemplateID = template.DoorframeTemplateID;
        DoorframeClassID = template.DoorframeClassID;
    }

    public override WallTemplate ToModel()
    {
        return new WallTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            Model = Model,
            ClassID = ClassID,
            DoorframeKitID = DoorframeKitID,
            DoorframeTemplateID = DoorframeTemplateID,
            DoorframeClassID = DoorframeClassID,
            Hooks = []
        };
    }
}
