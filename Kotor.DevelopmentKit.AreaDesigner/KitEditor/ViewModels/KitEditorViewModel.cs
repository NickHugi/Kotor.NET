using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
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

    private SourceList<ObjectItem> _objectTemplatesSource = new();
    private readonly ReadOnlyObservableCollection<ObjectItem> _objectTemplates;
    public ReadOnlyObservableCollection<ObjectItem> ObjectTemplateItems => _objectTemplates;

    public ObjectItem? SelectedObjectTemplateItem
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public List<WorldObjectTypeItem> WorldObjectTypeItems { get; } =
    [
        WorldObjectTypeItem.All,
        new(WorldObjectType.Basic),
        new(WorldObjectType.Tile),
        new(WorldObjectType.Floor),
        new(WorldObjectType.Wall),
        new(WorldObjectType.DoorFrame),
        new(WorldObjectType.Ceiling),
        new(WorldObjectType.InnerCorner),
        new(WorldObjectType.OuterCorner),
    ];
    public WorldObjectTypeItem? SelectedWorldObjectTypeItem
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    } = WorldObjectTypeItem.All;

    public KitEditorViewModel()
    {

        Name = "New Kit";
        KitID = "";
        FilePath = "";

        IObservable<Func<ObjectItem, bool>> filter =
            this.WhenAnyValue(x => x.SelectedWorldObjectTypeItem)
                .Select(filterWorldObjectType => new Func<ObjectItem, bool>(
                    x => filterWorldObjectType.Value is null
                    || x.WorldObjectType == filterWorldObjectType.Value));

        _objectTemplatesSource.Connect()
            .Filter(filter)
            .Sort(SortExpressionComparer<ObjectItem>.Ascending(x => x.ClassID).ThenByAscending(x => x.Name))
            .Bind(out _objectTemplates)
            .Subscribe();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        FilePath = kit.FilePath;
        Name = kit.Name;
        KitID = kit.ID;
        Version = kit.Version;

        _objectTemplatesSource.AddRange(
        [
            .. kit.Objects.Select(x => new ObjectItem(x)),
            .. kit.Tiles.Select(x => new TileItem(x)),
            .. kit.Floors.Select(x => new FloorItem(x)),
            .. kit.Ceilings.Select(x => new CeilingItem(x)),
            .. kit.Walls.Select(x => new WallItem(x)),
            .. kit.DoorFrames.Select(x => new DoorFrameItem(x)),
            .. kit.InnerCorners.Select(x => new InnerCornerItem(x)),
            .. kit.OuterCorners.Select(x => new OuterCornerItem(x)),
        ]);
    }

    public Kit ToModel()
    {
        return new Kit(FilePath, KitID, Version, Name)
        {
            //TODO
            //Tiles = TileTab.TileItems.Select(x => x.ToModel(KitID)).ToList(),
            //Floors = FloorTab.FloorItems.Select(x => x.ToModel(KitID)).ToList(),
            //Walls = WallTab.WallItems.Select(x => x.ToModel(KitID)).ToList(),
            //DoorFrames = DoorFrameTab.DoorFrameItems.Select(x => x.ToModel(KitID)).ToList(),
            //InnerCorners = InnerCornerTab.InnerCornerItems.Select(x => x.ToModel(KitID)).ToList(),
            //OuterCorners = OuterCornerTab.OuterCornerItems.Select(x => x.ToModel(KitID)).ToList(),
            //Ceilings = CeilingTab.CeilingItems.Select(x => x.ToModel(KitID)).ToList(),
            //Objects = ObjectTab.ObjectItems.Select(x => x.ToModel(KitID)).ToList(),
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

    public void AddBasicWorldObject()
    {
        _objectTemplatesSource.Add(new ObjectItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_object_",
            Model = $"{KitID}_object_",
            Name = "New Object",
        });
    }
    public void AddTileWorldObject()
    {
        _objectTemplatesSource.Add(new TileItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_tile_",
            Model = $"{KitID}_tile_",
            Name = "New Tile",
        });
    }
    public void AddWallWorldObject()
    {
        _objectTemplatesSource.Add(new WallItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_wall_",
            Model = $"{KitID}_wall_",
            Name = "New Wall",
        });
    }
    public void AddDoorframeWorldObject()
    {
        _objectTemplatesSource.Add(new WallItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_doorframe_",
            Model = $"{KitID}_doorframe_",
            Name = "New Doorframe",
        });
    }
    public void AddCeilingWorldObject()
    {
        _objectTemplatesSource.Add(new CeilingItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_ceiling_",
            Model = $"{KitID}_ceiling_",
            Name = "New Ceiling",
        });
    }
    public void AddFloorWorldObject()
    {
        _objectTemplatesSource.Add(new FloorItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_floor_",
            Model = $"{KitID}_floor_",
            Name = "New Floor",
        });
    }
    public void AddInnerCornerWorldObject()
    {
        _objectTemplatesSource.Add(new InnerCornerItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_icorner_",
            Model = $"{KitID}_icorner_",
            Name = "New Inner Corner",
        });
    }
    public void AddOuterCornerWorldObject()
    {
        _objectTemplatesSource.Add(new OuterCornerItem
        {
            KitID = KitID,
            TemplateID = $"{KitID}_ocorner_",
            Model = $"{KitID}_ocorner_",
            Name = "New Outer Corner",
        });
    }

    public void DeleteSelectedObject()
    {
        if (SelectedObjectTemplateItem is null)
            return;

        _objectTemplatesSource.Remove(SelectedObjectTemplateItem);
    }
}
