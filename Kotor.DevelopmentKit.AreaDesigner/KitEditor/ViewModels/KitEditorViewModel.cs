using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerializer;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    public Interaction<Unit, string?> SelectKitSaveFile { get; } = new();

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

    public int Version
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
        Name = kit.Name;
        KitID = kit.ID;

        TileTab = new(kit);
        FloorTab = new(kit);
        WallTab = new(kit);
        DoorFrameTab = new(kit);
        CeilingTab = new(kit);
        ObjectTab = new(kit);
    }

    public Kit ToModel()
    {
        return new Kit(FilePath, KitID, Version, Name)
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

    public async Task SaveAs()
    {
        var filepath = await SelectKitSaveFile.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath) == true)
            return;

        FilePath = filepath;
        KitID = Path.GetFileNameWithoutExtension(filepath);
        Version++;

        KitSerializer.Save(filepath, ToModel());
    }
}
