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
using Avalonia.Markup.Xaml.Templates;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Resources.KotorMDL.Controllers;
using Kotor.NET.Resources.KotorMDL.Nodes;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

public class KitEditorViewModel : ReactiveObject
{
    public Interaction<Unit, string?> SelectKitSaveFile { get; } = new();
    public Interaction<Unit, string[]> ImportTemplateFiles { get; } = new();

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

    public async Task ImportTemplates()
    {
        var filepaths = await ImportTemplateFiles.Handle(Unit.Default);

        foreach (var filepath in filepaths)
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);
            var mdl = MDL.FromFile(filepath);
            var template = TemplateFromMDL(filename, mdl);
            _objectTemplatesSource.Add(template);

            if (template is FloorItem floorTemplate)
            {
                var tileTemplate = TileFromFloor(filename, floorTemplate, mdl);
                _objectTemplatesSource.Add(tileTemplate);
            }
        }
    }
    public void AddBasicTemplate()
    {
        _objectTemplatesSource.Add(new ObjectItem
        {
            KitID = KitID,
            TemplateID = $"object_",
            Model = $"object_",
            Name = "New Object",
        });
    }
    public void AddTileTemplate()
    {
        _objectTemplatesSource.Add(new TileItem
        {
            KitID = KitID,
            TemplateID = $"tile_",
            Model = $"tile_",
            Name = "New Tile",
        });
    }
    public void AddWallTemplate()
    {
        _objectTemplatesSource.Add(new WallItem
        {
            KitID = KitID,
            TemplateID = $"wall_",
            Model = $"wall_",
            Name = "New Wall",
        });
    }
    public void AddDoorframeTemplate()
    {
        _objectTemplatesSource.Add(new WallItem
        {
            KitID = KitID,
            TemplateID = $"doorframe_",
            Model = $"doorframe_",
            Name = "New Doorframe",
        });
    }
    public void AddCeilingTemplate()
    {
        _objectTemplatesSource.Add(new CeilingItem
        {
            KitID = KitID,
            TemplateID = $"ceiling_",
            Model = $"ceiling_",
            Name = "New Ceiling",
        });
    }
    public void AddFloorTemplate()
    {
        _objectTemplatesSource.Add(new FloorItem
        {
            KitID = KitID,
            TemplateID = $"floor_",
            Model = $"floor_",
            Name = "New Floor",
        });
    }
    public void AddInnerCornerTemplate()
    {
        _objectTemplatesSource.Add(new InnerCornerItem
        {
            KitID = KitID,
            TemplateID = $"icorner_",
            Model = $"icorner_",
            Name = "New Inner Corner",
        });
    }
    public void AddOuterCornerTemplate()
    {
        _objectTemplatesSource.Add(new OuterCornerItem
        {
            KitID = KitID,
            TemplateID = $"ocorner_",
            Model = $"ocorner_",
            Name = "New Outer Corner",
        });
    }

    public void DeleteSelectedTemplate()
    {
        if (SelectedObjectTemplateItem is null)
            return;

        _objectTemplatesSource.Remove(SelectedObjectTemplateItem);
    }

    private ObjectItem TemplateFromMDL(string filename, MDL mdl)
    {
        var name = mdl.Root.Name;

        if (filename.Contains("floor"))
        {
            return new FloorItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("ceiling"))
        {
            return new CeilingItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("wall"))
        {

            return new WallItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("object"))
        {
            return new ObjectItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("icorner"))
        {
            return new InnerCornerItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("ocorner"))
        {
            return new OuterCornerItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
            };
        }
        else if (filename.Contains("doorframe"))
        {
            var magnetsNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet."));

            return new DoorFrameItem()
            {
                KitID = KitID,
                TemplateID = filename,
                Name = mdl.Root.Name,
                Model = filename,
                ClassID = "",
                Hooks =
                [
                    ..magnetsNodes.Select(x => new DoorFrameHookItem()
                    {
                        Position = new(x.GetController<MDLControllerDataPosition>().First().Data[0].ToVector3()),
                        Orientation = new(x.GetController<MDLControllerDataOrientation>().First().Data[0].ToQuaternion()),
                    }),
                ]
            };
        }
        else
        {
            return null;
        }
    }
    private TileItem TileFromFloor(string filename, FloorItem floor, MDL mdl)
    {
        var wallNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.wall."));
        var cornerNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.corner."));
        var floorNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.floor."));
        var ceilingNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.ceiling."));

        return new TileItem()
        {
            KitID = KitID,
            TemplateID = filename.Replace("floor_", "tile_"),
            Name = mdl.Root.Name,
            ClassID = "",
            Hooks =
            [
                new FloorHookItem()
                {
                    KitID = KitID,
                    TemplateID = $"floor_{filename.Replace("floor_", "")}",
                    Position = new(Vector3.Zero),
                    Orientation = new(Quaternion.Identity),
                },
                new CeilingHookItem()
                {
                    KitID = KitID,
                    TemplateID = $"ceiling_{filename.Replace("floor_", "")}",
                    Position = new(Vector3.Zero),
                    Orientation = new(Quaternion.Identity),
                },
                ..wallNodes.Select(x => new WallHookItem()
                {
                    KitID = KitID,
                    TemplateID = $"wall_{x.Name.Split('.').Last()}",
                    Position = new(x.GetController<MDLControllerDataPosition>().First().Data[0].ToVector3()),
                    Orientation = new(x.GetController<MDLControllerDataOrientation>().First().Data[0].ToQuaternion()),
                }),
                ..cornerNodes.Select(x => new CornerHookItem()
                {
                    InnerKitID = KitID,
                    InnerTemplateID = $"icorner_{x.Name.Split('.').Last()}",
                    OuterKitID = KitID,
                    OuterTemplateID = $"ocorner_{x.Name.Split('.').Last()}",
                    Position = new(x.GetController<MDLControllerDataPosition>().First().Data[0].ToVector3()),
                    Orientation = new(x.GetController<MDLControllerDataOrientation>().First().Data[0].ToQuaternion()),
                }),
            ]
        };
    }
}
