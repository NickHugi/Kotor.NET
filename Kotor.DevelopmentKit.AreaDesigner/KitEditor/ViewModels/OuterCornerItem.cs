using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
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
        ID = template.TemplateID;
        Name = template.Name;
        Group = template.ClassID;
        Model = template.Model;
    }

    public OuterCornerTemplate ToModel(string kitID)
    {
        return new OuterCornerTemplate
        {
            KitID = kitID,
            TemplateID = ID,
            Name = Name,
            ClassID = Group,
            Model = Model,
        };
    }
}
