using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class InnerCornerItem : ReactiveObject
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

    public InnerCornerItem()
    {
        ID = "";
        Name = "";
        Group = "";
        Model = "";
    }
    public InnerCornerItem(InnerCornerTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Group = template.Group;
        Model = template.Model;
    }

    public InnerCornerTemplate ToModel(string kitID)
    {
        return new InnerCornerTemplate
        {
            KitID = kitID,
            ID = ID,
            Name = Name,
            Group = Group,
            Model = Model,
        };
    }
}
