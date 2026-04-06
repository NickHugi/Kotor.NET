using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.NET.Common;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Resources.KotorMDL;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaDesignerViewModel : ReactiveObject
{
    public Interaction<Unit, Point> GetMousePoint = new();
    public Interaction<Unit, WallTemplate?> SelectWallTemplate = new();
    public Interaction<Unit, TileTemplate?> SelectTileTemplate = new();
    public Interaction<Unit, string?> SelectSaveFilepathForArea = new();
    public Interaction<Unit, string?> SelectLoadFilepathForArea = new();

    public ObservableCollection<Kit> Kits { get; } = new();
    public Kit? SelectedKit
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public Area Area
    {
        get => Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        set
        {
            Engine.Scene.Entities.OfType<AreaEntity>().Single().Area = value;
            this.RaisePropertyChanged(nameof(Area));
        }
    }

    public GLEngine Engine { get; set => this.RaiseAndSetIfChanged(ref field, value); }

    public BaseMode Mode { get; private set => this.RaiseAndSetIfChanged(ref field, value); }

    public bool ShowWalls
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true;

    public bool ShowDoors
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true; 

    public bool ShowCorners
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = true;

    public bool ShowCeilings
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = false;

    public void SetSceneMode_AddRoom()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new AddRoomMode(Engine, area)
        {

        };
    }
    public void SetSceneMode_DeleteRoom()
    {

    }
    public void SetSceneMode_AddTile()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new ExtendRoomMode(Engine, area)
        {
            GetMousePoint = GetMousePoint,
            SelectTileTemplate = SelectTileTemplate,
        };
    }
    public void SetSceneMode_DeleteTile()
    {

    }
    public void SetSceneMode_SwitchWall()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new SwitchWallMode(Engine, area)
        {
            GetMousePoint = GetMousePoint,
            SelectWallTemplate = SelectWallTemplate,
        };
    }
    public void SetSceneMode_AddObject()
    {
        var area = Engine.Scene.Entities.OfType<AreaEntity>().Single().Area;
        Mode = new AddObjectMode(Engine, area);
    }

    public void ReloadKit(string filepath)
    {
        
    }

    public async Task SaveAreaAs()
    {
        var filepath = await SelectSaveFilepathForArea.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath))
            return;

        AreaSerializer.Save(filepath, Area);
    }

    public async Task LoadArea()
    {
        var filepath = await SelectLoadFilepathForArea.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath))
            return;

        Area = AreaSerializer.Load(filepath);
    }

    public AreaDesignerViewModel()
    {
        Kit.Manager.Refresh();
        Kits = new(Kit.Manager.Kits);
    }

    public async Task Export()
    {
        var mdl = AreaExporter.RoomToMDL(Area.Rooms.First());
        MDL.ToFile(mdl, $"{Kit.Manager.ActiveDirectory}/test.mdl", GameEngine.K1, Platform.Windows);
    }
}
