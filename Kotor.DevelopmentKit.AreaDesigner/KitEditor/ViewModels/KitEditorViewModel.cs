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
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
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
    public InnerCornerTabViewModel InnerCornerTab { get; }
    public OuterCornerTabViewModel OuterCornerTab { get; }

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
        InnerCornerTab = new();
        OuterCornerTab = new();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        FilePath = kit.FilePath;
        Name = kit.Name;
        KitID = kit.ID;
        Version = kit.Version;

        TileTab = new(kit);
        FloorTab = new(kit);
        WallTab = new(kit);
        DoorFrameTab = new(kit);
        CeilingTab = new(kit);
        ObjectTab = new(kit);
        InnerCornerTab = new(kit);
        OuterCornerTab = new(kit);
    }

    public Kit ToModel()
    {
        return new Kit(FilePath, KitID, Version, Name)
        {
            Tiles = TileTab.TileItems.Select(x => x.ToModel(KitID)).ToList(),
            Floors = FloorTab.FloorItems.Select(x => x.ToModel(KitID)).ToList(),
            Walls = WallTab.WallItems.Select(x => x.ToModel(KitID)).ToList(),
            DoorFrames = DoorFrameTab.DoorFrameItems.Select(x => x.ToModel(KitID)).ToList(),
            InnerCorners = InnerCornerTab.InnerCornerItems.Select(x => x.ToModel(KitID)).ToList(),
            OuterCorners = OuterCornerTab.OuterCornerItems.Select(x => x.ToModel(KitID)).ToList(),
            Ceilings = CeilingTab.CeilingItems.Select(x => x.ToModel(KitID)).ToList(),
            Objects = ObjectTab.ObjectItems.Select(x => x.ToModel(KitID)).ToList(),
        };
    }

    public void Save()
    {
        if (!File.Exists(FilePath))
            return;

        Version++;
        KitSerializer.Save(FilePath, ToModel());
    }

    public async Task SaveAs()
    {
        var filepath = await SelectKitSaveFile.Handle(Unit.Default);

        if (string.IsNullOrEmpty(filepath))
            return;

        FilePath = filepath;
        KitID = Path.GetFileNameWithoutExtension(filepath);
        Version++;

        KitSerializer.Save(filepath, ToModel());
    }
}
