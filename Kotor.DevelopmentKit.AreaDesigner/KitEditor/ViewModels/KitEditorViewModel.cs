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
using System.Xml.Linq;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using DynamicData;
using DynamicData.Binding;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels.Templates;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.DevelopmentKit.AreaDesigner.Views;
using Kotor.DevelopmentKit.Base.ReactiveObjects;
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

    private SourceList<WorldObjectItem> _objectTemplatesSource = new();
    private readonly ReadOnlyObservableCollection<WorldObjectItem> _objectTemplates;
    public ReadOnlyObservableCollection<WorldObjectItem> ObjectTemplateItems => _objectTemplates;

    public WorldObjectItem? SelectedObjectTemplateItem
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public List<WorldObjectTypeItem> WorldObjectTypeItems { get; } =
    [
        WorldObjectTypeItem.All,
        new(WorldObjectType.Prop),
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

        IObservable<Func<WorldObjectItem, bool>> filter =
            this.WhenAnyValue(x => x.SelectedWorldObjectTypeItem)
                .Select(filterWorldObjectType => new Func<WorldObjectItem, bool>(
                    x => filterWorldObjectType.Value is null
                    || x.WorldObjectType == filterWorldObjectType.Value));

        _objectTemplatesSource.Connect()
            .Filter(filter)
            .Sort(SortExpressionComparer<WorldObjectItem>.Ascending(x => x.ClassID).ThenByAscending(x => x.Name))
            .Bind(out _objectTemplates)
            .Subscribe();
    }
    public KitEditorViewModel(Kit kit) : this()
    {
        FilePath = kit.FilePath;
        Name = kit.Name;
        KitID = kit.KitID;
        Version = kit.Version;

        _objectTemplatesSource.AddRange(
        [
            //.. kit.Objects.OfType<UltimateWorldObject>().Select(x => new FloorItem(x)),
            //.. kit.Objects.OfType<CeilingTemplate>().Select(x => new CeilingItem(x)),
            //.. kit.Objects.OfType<WallTemplate>().Select(x => new WallItem(x)),
            //.. kit.Objects.OfType<InnerCornerTemplate>().Select(x => new InnerCornerItem(x)),
            //.. kit.Objects.OfType<OuterCornerTemplate>().Select(x => new OuterCornerItem(x)),

            .. kit.Objects.OfType<PropTemplate>().Select(x => new PropItem(x)),
            .. kit.Objects.OfType<TileTemplate>().Select(x => new TileItem(x)),
            .. kit.Objects.OfType<DoorFrameTemplate>().Select(x => new DoorFrameItem(x)),
        ]);
    }

    public Kit ToModel()
    {
        return new Kit(FilePath, KitID, Version, Name)
        {
            Objects = ObjectTemplateItems.Select(x => x.ToModel()).ToList()
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

            var existing = ObjectTemplateItems.Where(x => x.TemplateID == template.TemplateID);
            _objectTemplatesSource.RemoveMany(existing);

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
        _objectTemplatesSource.Add(new PropItem
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

    private WorldObjectItem TemplateFromMDL(string filename, MDL mdl)
    {
        if (filename.Contains("floor"))
        {
            return new FloorItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
            };
        }
        else if (filename.Contains("ceiling"))
        {
            return new CeilingItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
            };
        }
        else if (filename.Contains("wall"))
        {
            return new WallItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
                DoorframeKitID = DoorframeKitIDFromMDL(mdl),
                DoorframeTemplateID = DoorframeTemplateIDFromMDL(mdl),
                DoorframeClassID = DoorframeClassIDFromMDL(mdl)
            };
        }
        else if (filename.Contains("object"))
        {
            return new PropItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
            };
        }
        else if (filename.Contains("icorner"))
        {
            return new InnerCornerItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
            };
        }
        else if (filename.Contains("ocorner"))
        {
            return new OuterCornerItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
            };
        }
        else if (filename.Contains("doorframe"))
        {
            var magnetsNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet."));

            return new DoorFrameItem()
            {
                KitID = KitID,
                TemplateID = filename,
                ClassID = ClassIDFromMDL(mdl),
                Name = NameFromMDL(mdl),
                Model = filename,
                Hooks =
                [
                    ..magnetsNodes.Select(x => new DoorFrameHookItem()
                    {
                        Position = PositionFromNode(x),
                        Orientation = OrientationFromNode(x),
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
            ClassID = ClassIDFromMDL(mdl),
            Name = NameFromMDL(mdl).Replace("Floor", "Tile"),
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
                    Position = PositionFromNode(x),
                    Orientation = OrientationFromNode(x),
                }),
                ..cornerNodes.Select(x => new CornerHookItem()
                {
                    KitID = KitID,
                    TemplateID = $"Xcorner_{x.Name.Split('.').Last()}",
                    Position = PositionFromNode(x),
                    Orientation = OrientationFromNode(x),
                }),
            ]
        };
    }
    private ReactiveVector3 PositionFromNode(MDLNode node)
    {
        return new(node.GetController<MDLControllerDataPosition>().First().Data[0].ToVector3());
    }
    private ReactiveQuaternion OrientationFromNode(MDLNode node)
    {
        return new(node.GetController<MDLControllerDataOrientation>().First().Data[0].ToQuaternion());
    }
    private string NameFromMDL(MDL mdl)
    {
        var node = mdl.Root.Children.SingleOrDefault(x => x.Name.StartsWith("_Name="));
        return node?.Name.Replace("_Name=", "") ?? mdl.Name;
    }
    private string ClassIDFromMDL(MDL mdl)
    {
        var node = mdl.Root.Children.SingleOrDefault(x => x.Name.StartsWith("_ClassID="));
        return node?.Name.Replace("_ClassID=", "") ?? "";
    }
    private string DoorframeTemplateIDFromMDL(MDL mdl)
    {
        var node = mdl.Root.Children.SingleOrDefault(x => x.Name.StartsWith("_DoorframeTemplateID="));
        return node?.Name.Replace("_DoorframeTemplateID=", "") ?? "";
    }
    private string DoorframeKitIDFromMDL(MDL mdl)
    {
        var node = mdl.Root.Children.SingleOrDefault(x => x.Name.StartsWith("_DoorframeKitID="));
        var fallbackKitID = (DoorframeTemplateIDFromMDL(mdl) is null) ? "" : KitID;
        return node?.Name.Replace("_DoorframeKitID=", "") ?? fallbackKitID;
    }
    private string DoorframeClassIDFromMDL(MDL mdl)
    {
        var node = mdl.Root.Children.SingleOrDefault(x => x.Name.StartsWith("_DoorframeClassID="));
        return node?.Name.Replace("_DoorframeClassID=", "") ?? "";
    }
}
