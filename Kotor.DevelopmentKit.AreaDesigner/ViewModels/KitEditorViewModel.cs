using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    public ObservableCollection<TileItem> TileItems { get; }
    public TileItem? SelectedTileItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public WallHookItem? SelectedWallHookItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public KitEditorViewModel()
    {
        TileItems = [];
    }
    public KitEditorViewModel(Kit kit)
    {
        TileItems = new ObservableCollection<TileItem>(kit.Tiles.Select(x => new TileItem(x)));
    }

    public void DeleteSelectedTile()
    {
        if (SelectedTileItem is null)
            return;

        TileItems.Remove(SelectedTileItem);
    }

    public void DeleteSelectedWallHook()
    {
        if (SelectedTileItem is null)
            return;
        if (SelectedWallHookItem is null)
            return;

        SelectedTileItem.Walls.Remove(SelectedWallHookItem);
    }
}

public class TileItem : ReactiveObject
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

    public string DefaultFloorID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string DefaultCeilingID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ObservableCollection<WallHookItem> Walls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public TileItem()
    {
        ID = "";
        Name = "";
        DefaultFloorID = "";
        DefaultCeilingID = "";
        Walls = [];
    }
    public TileItem(TileTemplate tile)
    {
        ID = tile.ID;
        Name = tile.Name;
        DefaultFloorID = tile.DefaultFloorID;
        DefaultCeilingID = tile.DefaultCeilingID;
        Walls = new(tile.Walls.Select(x => new WallHookItem(x)));
    }
}

public class WallHookItem : ReactiveObject
{
    public string Name => "Hook";

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
