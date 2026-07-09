using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;

public abstract class WorldObjectItem : ReactiveObject
{
    public abstract WorldObjectType WorldObjectType { get; }

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

    public ObservableCollection<BaseMagnetItem> Hooks { get; init; }

    public BaseMagnetItem? SelectedHook
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
        Hooks = [];
    }
    public WorldObjectItem(UltimateWorldObjectTemplate template) : this()
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Name = template.Name;
        ClassID = template.ClassID;
        Model = template.Model;
        Hooks =
        [
            // TODO
            //..template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new WallHookItem(x)),
            //..template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new CeilingHookItem(x)),
            //..template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new FloorHookItem(x)),
            //..template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new CornerHookItem(x)),
            //..template.Magnets.OfType<UltimateMagnetTemplate>().Select(x => new DoorFrameHookItem(x)),
        ];
    }

    public abstract UltimateWorldObjectTemplate ToModel();

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
        Hooks.Add(new MagnetItem());
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
            KitID = KitID,
            TemplateID = KitID,
        });
    }
}
