using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.OpenGL;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class AreaDesignerViewModel : ReactiveObject
{
    public GLEngine Engine { get; set => this.RaiseAndSetIfChanged(ref field, value); }

    public SceneMode SceneMode
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = SceneMode.None;

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
        SceneMode = SceneMode.AddRoom;
    }
    public void SetSceneMode_DeleteRoom()
    {
        SceneMode = SceneMode.DeleteRoom;
    }
    public void SetSceneMode_AddTile()
    {
        SceneMode = SceneMode.AddTile;
    }
    public void SetSceneMode_DeleteTile()
    {
        SceneMode = SceneMode.DeleteTile;
    }
    public void SetSceneMode_SwitchWall()
    {
        SceneMode = SceneMode.SwitchWall;
    }
}

public enum SceneMode
{
    None,
    AddRoom,
    DeleteRoom,
    AddTile,
    DeleteTile,
    SwitchWall,
}
