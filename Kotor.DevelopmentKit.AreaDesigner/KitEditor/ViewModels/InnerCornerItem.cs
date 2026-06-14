using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
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
        ID = template.ObjectID;
        Name = template.Name;
        Group = template.ClassID;
        Model = template.Model;
    }

    public InnerCornerTemplate ToModel(string kitID)
    {
        return new InnerCornerTemplate
        {
            KitID = kitID,
            ObjectID = ID,
            Name = Name,
            ClassID = Group,
            Model = Model,
        };
    }
}
