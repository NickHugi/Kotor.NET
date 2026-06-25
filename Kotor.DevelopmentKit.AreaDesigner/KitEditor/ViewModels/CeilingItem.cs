using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class CeilingItem : ReactiveObject
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

    public CeilingItem()
    {
        ID = "";
        Name = "";
        Group = "";
        Model = "";
    }
    public CeilingItem(CeilingTemplate template)
    {
        ID = template.TemplateID;
        Name = template.Name;
        Group = template.ClassID;
        Model = template.Model;
    }

    public CeilingTemplate ToModel(string kitID)
    {
        return new CeilingTemplate
        {
            KitID = kitID,
            TemplateID = ID,
            Name = Name,
            ClassID = Group,
            Model = Model
        };
    }
}
