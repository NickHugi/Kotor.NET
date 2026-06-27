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

    public string DoorFrameID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallItem() : base()
    {
        DoorFrameID = "";
    }
    public WallItem(WallTemplate template) : base(template)
    {
        DoorFrameID = template.DoorFrameID;
    }

    public WallTemplate ToModel(string kitID)
    {
        return new WallTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            Model = Model,
            ClassID = ClassID,
            DoorFrameID = DoorFrameID,
        };
    }
}
