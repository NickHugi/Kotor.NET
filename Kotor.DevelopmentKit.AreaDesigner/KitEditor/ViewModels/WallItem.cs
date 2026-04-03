using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WallItem : ReactiveObject
{
    public string ID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Model
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string DoorFrameID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallItem()
    {
        ID = "";
        Name = "";
        Model = "";
        DoorFrameID = "";
    }
    public WallItem(WallTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Model = template.Model;
        DoorFrameID = template.DoorFrameID;
    }

    public WallTemplate ToModel()
    {
        return new WallTemplate
        {
            ID = ID,
            Name = Name,
            Model = Model,
            DoorFrameID = DoorFrameID,
        };
    }
}
