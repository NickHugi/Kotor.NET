using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private object lol;

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
    public CornerItem? SelectedCornerItem
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public int LeftPanelSelectedIndex
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public KitEditorViewModel()
    {
        TileItems = [];

        this.WhenAnyValue(x => x.SelectedWallHookItem).Subscribe(value => LeftPanelSelectedIndex = 0);
        this.WhenAnyValue(x => x.SelectedCornerItem).Subscribe(value => LeftPanelSelectedIndex = 1);
    }
    public KitEditorViewModel(Kit kit) : this()
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

    public ObservableCollection<CornerItem> InnerCorners
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<CornerItem> OuterCorners
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
        InnerCorners = [];
        OuterCorners = [];
    }
    public TileItem(TileTemplate tile)
    {
        ID = tile.ID;
        Name = tile.Name;
        DefaultFloorID = tile.DefaultFloorID;
        DefaultCeilingID = tile.DefaultCeilingID;
        Walls = new(tile.Walls.Select(x => new WallHookItem(x)));
        InnerCorners = new(tile.InnerCorners.Select(x => new CornerItem(x)));
        OuterCorners = new(tile.OuterCorners.Select(x => new CornerItem(x)));
    }
}

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

public class CornerItem : ReactiveObject
{
    public string Name => $"CHook ({Position.Z:F2}, {Position.Y:F2}, {Position.Z:F2})";

    public string DefaultCornerID
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

    public CornerItem()
    {
        DefaultCornerID = "";
        Position = new();
        Orientation = new();
        AdjacentWalls = [];
    }
    public CornerItem(CornerTemplate template)
    {
        DefaultCornerID = template.ID;
        Position = new(template.Position);
        Orientation = new(template.Orientation);
        AdjacentWalls = new(template.Adjacent);
    }
}
