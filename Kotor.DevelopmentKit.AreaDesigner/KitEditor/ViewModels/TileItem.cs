using System.Collections.ObjectModel;
using System.Linq;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

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

    public ObservableCollection<CornerHookItem> InnerCorners
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<CornerHookItem> OuterCorners
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
        InnerCorners = new(tile.InnerCorners.Select(x => new CornerHookItem(x)));
        OuterCorners = new(tile.OuterCorners.Select(x => new CornerHookItem(x)));
    }

    public TileTemplate ToModel()
    {
        return new TileTemplate
        {
            ID = ID,
            Name = Name,
            DefaultFloorID = DefaultFloorID,
            DefaultCeilingID = DefaultCeilingID,
            Walls = Walls.Select(x => x.ToModel()).ToArray(),
            InnerCorners = InnerCorners.Select(x => x.ToModel()).ToArray(),
            OuterCorners = OuterCorners.Select(x => x.ToModel()).ToArray(),
            CeilingHooks = [], // TODO
        };
    }
}
