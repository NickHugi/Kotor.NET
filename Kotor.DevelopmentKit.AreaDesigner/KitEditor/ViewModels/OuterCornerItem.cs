using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class OuterCornerItem : ReactiveObject
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

    public string Group
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Model
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public OuterCornerItem()
    {
        ID = "";
        Name = "";
        Group = "";
        Model = "";
    }
    public OuterCornerItem(OuterCornerTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Group = template.Group;
        Model = template.Model;
    }

    public OuterCornerTemplate ToModel(string kitID)
    {
        return new OuterCornerTemplate
        {
            KitID = kitID,
            ID = ID,
            Name = Name,
            Group = Group,
            Model = Model,
        };
    }
}
