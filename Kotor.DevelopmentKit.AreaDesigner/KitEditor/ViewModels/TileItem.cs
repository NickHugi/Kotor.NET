using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class TileItem : ObjectItem
{
    public ObservableCollection<WallHookItem> Walls
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<FloorHookItem> Floors
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<CeilingHookItem> Ceilings
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<InnerCornerHookItem> InnerCorners
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ObservableCollection<OuterCornerHookItem> OuterCorners
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public HookItem? SelectedHook { get; set; }

    public TileItem() : base()
    {
        Walls = [];
        Floors = [];
        Ceilings = [];
        InnerCorners = [];
        OuterCorners = [];
    }
    public TileItem(TileTemplate tile)
    {
        TemplateID = tile.TemplateID;
        Name = tile.Name;
        Floors = []; // TODO new(tile.Floors.Select(x => new WallHookItem(x)))
        Ceilings = [];
        Walls = new(tile.Walls.Select(x => new WallHookItem(x)));
        InnerCorners = new(tile.InnerCorners.Select(x => new InnerCornerHookItem(x)));
        OuterCorners = new(tile.OuterCorners.Select(x => new OuterCornerHookItem(x)));
    }

    public TileTemplate ToModel(string kitID)
    {
        return new TileTemplate
        {
            KitID = kitID,
            TemplateID = TemplateID,
            Name = Name,
            Model = null,
            ClassID = null,
            Floors = Floors.Select(x => x.ToModel()).ToArray(),
            Ceilings = Ceilings.Select(x => x.ToModel()).ToArray(),
            Walls = Walls.Select(x => x.ToModel()).ToArray(),
            InnerCorners = InnerCorners.Select(x => x.ToModel()).ToArray(),
            OuterCorners = OuterCorners.Select(x => x.ToModel()).ToArray(),
        };
    }

    public void AddWallHook()
    {
        Walls.Add(new());
    }
    public void DeleteSelectedWallHook()
    {
        if (SelectedHook is WallHookItem wallHook && wallHook is not null)
        {
            Walls.Remove(wallHook);
        }
    }

    public void AddInnerCorner()
    {
        InnerCorners.Add(new());
    }
    public void DeleteSelectedInnerCorner()
    {
        if (SelectedHook is InnerCornerHookItem corner && corner is not null)
        {
            InnerCorners.Remove(corner);
        }
    }

    public void AddOuterCorner()
    {
        OuterCorners.Add(new());
    }
    public void DeleteSelectedOuterCorner()
    {
        if (SelectedHook is OuterCornerHookItem corner && corner is not null)
        {
            OuterCorners.Remove(corner);
        }
    }
}
