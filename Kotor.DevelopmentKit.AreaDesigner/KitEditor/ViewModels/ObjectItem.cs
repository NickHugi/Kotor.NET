using System;
using System.Collections.Generic;
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

    public HookItem SelectedHook
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
    }
    public ObjectItem(ObjectTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Name = template.Name;
        ClassID = template.ClassID;
        Model = template.Model;
    }

    public ObjectTemplate ToModel(string kitID)
    {
        return new ObjectTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            ClassID = ClassID,
            Model = Model,
        };
    }
}
