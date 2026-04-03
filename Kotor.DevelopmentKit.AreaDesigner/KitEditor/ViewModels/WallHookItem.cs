using System.Collections.ObjectModel;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class WallHookItem : ReactiveObject
{
    public string Name => $"Hook ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public string DefaultWallID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveVector3 Position
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ReactiveQuaternion Orientation
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<int> AdjacentWalls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public WallHookItem()
    {
        DefaultWallID = "";
        Position = new();
        Orientation = new();
        AdjacentWalls = [];
    }
    public WallHookItem(WallHookTemplate wallHook)
    {
        DefaultWallID = wallHook.DefaultWallID;
        Position = new(wallHook.LocalPosition);
        Orientation = new(wallHook.LocalOrientation);
        AdjacentWalls = new(wallHook.AdjacentWalls);
    }
}
