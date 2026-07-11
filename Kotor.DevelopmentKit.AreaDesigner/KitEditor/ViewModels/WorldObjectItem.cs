using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WorldObjectItem : ReactiveObject
{
    public IEnumerable<WorldObjectType> WorldObjectTypes { get; } = Enum.GetValues<WorldObjectType>();
    public WorldObjectType Type
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string KitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string TemplateID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public string ClassID
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

    public ObservableCollection<MagnetItem> Magnets { get; init; }
    public MagnetItem? SelectedMagnet
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WorldObjectItem()
    {
        KitID = "";
        TemplateID = "";
        Name = "";
        ClassID = "";
        Model = "";
        Magnets = [];
    }
    public WorldObjectItem(WorldObjectTemplate template) : this()
    {
        Type = template.Type;
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Name = template.Name;
        ClassID = template.ClassID;
        Model = template.Model;
        Magnets =
        [
            ..template.Magnets.Select(x => new MagnetItem(x)),
        ];
    }

    public WorldObjectTemplate ToModel()
    {
        return new WorldObjectTemplate
        {
            Type = Type,
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Magnets = Magnets.Select(x => x.ToModel()).ToArray(),
        };
    }

    public void DeleteSelectedMagnet()
    {
        if (SelectedMagnet is null)
            return;

        Magnets.Remove(SelectedMagnet);
    }
    public void AddMagnet()
    {
        Magnets.Add(new MagnetItem());
    }
}
