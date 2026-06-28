using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class ObjectItem : ReactiveObject
{
    public virtual WorldObjectType WorldObjectType => WorldObjectType.Basic;

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

    public string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string ClassID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string Model
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<HookItem> Hooks { get; }

    public HookItem? SelectedHook
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObjectItem()
    {
        KitID = "";
        TemplateID = "";
        Name = "";
        ClassID = "";
        Model = "";
        Hooks = [];
    }
    public ObjectItem(ObjectTemplate template) : this()
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Name = template.Name;
        ClassID = template.ClassID;
        Model = template.Model;
    }

    public virtual ObjectTemplate ToModel()
    {
        return new ObjectTemplate
        {
            KitID = KitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
            Hooks = Hooks.Select(x => x.ToModel()).ToArray(),
        };
    }
}
