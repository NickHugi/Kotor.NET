using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
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

    public string Model
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public CeilingItem()
    {
        ID = "";
        Name = "";
        Model = "";
    }
    public CeilingItem(CeilingTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Model = template.Model;
    }
}
