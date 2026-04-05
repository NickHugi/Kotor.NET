using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class FloorItem : ReactiveObject
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

    public FloorItem()
    {
        ID = "";
        Name = "";
        Model = "";
    }
    public FloorItem(FloorTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Model = template.Model;
    }

    public FloorTemplate ToModel(string kitID)
    {
        return new FloorTemplate
        {
            KitID = kitID,
            ID = ID,
            Name = Name,
            Model = Model,
        };
    }
}
