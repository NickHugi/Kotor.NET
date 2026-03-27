using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.NET.Graphics.OpenGL;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaDesignerViewModel : ReactiveObject
{
    public Interaction<Unit, Point> GetMousePoint = new();
    public Interaction<Unit, WallTemplate?> SelectWallTemplate = new();
    public Interaction<Unit, TileTemplate?> SelectTileTemplate = new();

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
}
