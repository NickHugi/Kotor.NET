using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    public string Name
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string KitID
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public string FilePath
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public TileTabViewModel TileTab { get; }
    public FloorTabViewModel FloorTab { get; }
    public WallTabViewModel WallTab { get; }
    public DoorFrameTabViewModel DoorFrameTab { get; }
    public CeilingTabViewModel CeilingTab { get; }
    public ObjectTabViewModel ObjectTab { get; }

    public KitEditorViewModel()
    {
        Name = "New Kit";
        KitID = "";
        FilePath = "";

        TileTab = new();
        FloorTab = new();
        WallTab = new();
        DoorFrameTab = new();
        CeilingTab = new();
        ObjectTab = new();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        TileTab = new(kit);
        FloorTab = new(kit);
        WallTab = new(kit);
        DoorFrameTab = new(kit);
        CeilingTab = new(kit);
        ObjectTab = new(kit);
    }

    public Kit ToModel()
    {
        return new Kit(KitID, Name)
        {
            Tiles = TileTab.TileItems.Select(x => x.ToModel()).ToList(),
            Floors = FloorTab.FloorItems.Select(x => x.ToModel()).ToList(),
            Walls = WallTab.WallItems.Select(x => x.ToModel()).ToList(),
            DoorFrames = DoorFrameTab.DoorFrameItems.Select(x => x.ToModel()).ToList(),
            //InsideCorners = .TileItems.Select(x => x.ToModel()).ToList(),
            //OutsideCorners = OuterCornerTab.TileItems.Select(x => x.ToModel()).ToList(),
            Ceilings = CeilingTab.CeilingItems.Select(x => x.ToModel()).ToList(),
            Objects = ObjectTab.ObjectItems.Select(x => x.ToModel()).ToList(),
        };
    }

    public void Save()
    {

    }

    public void SaveAs()
    {

    }
}
