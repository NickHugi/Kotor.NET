using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class DoorFrameItem : ReactiveObject
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

    public ObservableCollection<DoorFrameHookItem> Hooks
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public DoorFrameItem()
    {
        ID = "";
        Name = "";
        Model = "";
        Group = "";
        Hooks = [new(), new()];
    }
    public DoorFrameItem(DoorFrameTemplate template)
    {
        ID = template.ID;
        Name = template.Name;
        Model = template.Model;
        Group = template.Group;
        Hooks = new(template.Hooks.Select(x => new DoorFrameHookItem(x)));
    }

    public DoorFrameTemplate ToModel(string kitID)
    {
        return new DoorFrameTemplate
        {
            KitID = kitID,
            ID = ID,
            Name = Name,
            Group = Group,
            Model = Model,
            Hooks = Hooks.Select(x => x.ToModel()).ToArray(),
        };
    }
}
