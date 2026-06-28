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

    public void DeleteSelectedMagnet()
    {
        if (SelectedHook is null)
            return;

        Hooks.Remove(SelectedHook);
    }

    public void AddMagnet()
    {
        Hooks.Add(new MagnetItem());
    }
    public void AddBasicHook()
    {
        Hooks.Add(new HookItem());
    }
    public void AddFloorHook()
    {
        Hooks.Add(new FloorHookItem()
        {
            KitID = KitID
        });
    }
    public void AddCeilingHook()
    {
        Hooks.Add(new CeilingHookItem()
        {
            KitID = KitID
        });
    }
    public void AddWallHook()
    {
        Hooks.Add(new WallHookItem()
        {
            KitID = KitID
        });
    }
    public void AddDoorframeHook()
    {
        Hooks.Add(new DoorFrameHookItem());
    }
    public void AddCornerHook()
    {
        Hooks.Add(new CornerHookItem()
        {
            InnerKitID = KitID,
            InnerTemplateID = KitID,
        });
    }
}
