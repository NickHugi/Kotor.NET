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
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
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

    public WorldObjectItem? SelectedWorldObject
    {
        get;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public List<WorldObjectTypeItem> WorldObjectTypeItems { get; } =
    [
        WorldObjectTypeItem.All,
        new(WorldObjectType.Generic),
        new(WorldObjectType.Tile),
        new(WorldObjectType.Floor),
        new(WorldObjectType.Wall),
        new(WorldObjectType.DoorFrame),
        new(WorldObjectType.Ceiling),
        new(WorldObjectType.InnerCorner),
        new(WorldObjectType.OuterCorner),
    ];
    public WorldObjectTypeItem? SelectedWorldObjectType
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
            this.WhenAnyValue(x => x.SelectedWorldObjectType)
                .Select(filterWorldObjectType => new Func<WorldObjectItem, bool>(
                    x => filterWorldObjectType.Value is null
                    || x.Type == filterWorldObjectType.Value));

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
            .. kit.Objects.Select(x => new WorldObjectItem(x)),
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

            if (template.Type == WorldObjectType.Floor)
            {
                var tileTemplate = TileFromFloor(filename, template, mdl);
                _objectTemplatesSource.Add(tileTemplate);
            }
        }
    }
    public void AddTemplate()
    {
        _objectTemplatesSource.Add(new WorldObjectItem
        {
            KitID = KitID,
            TemplateID = $"object_",
            Model = $"object_",
            Name = "New Object",
        });
    }
    public void DeleteSelectedTemplate()
    {
        if (SelectedWorldObject is null)
            return;

        _objectTemplatesSource.Remove(SelectedWorldObject);
    }

    private WorldObjectItem TemplateFromMDL(string filename, MDL mdl)
    {
        var magnetsNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet."));

        return new WorldObjectItem()
        {
            Type = WorldObjectTypeFromFilename(filename),
            KitID = KitID,
            TemplateID = filename,
            ClassID = ClassIDFromMDL(mdl),
            Name = NameFromMDL(mdl),
            Model = filename,
            Magnets =
            [
                ..magnetsNodes.Where(x => !mdl.Name.Contains("floor_")).Select(x => new MagnetItem()
                {
                    KitID = HookKitIDFromNode(x),
                    TemplateID = TemplateIDFromNode(x),
                    Position = PositionFromNode(x),
                    Orientation = OrientationFromNode(x),
                    ConditionCheckLocalMagnetsOnly = GetBool(x, "CheckLocalMagnetsOnly"),
                    ConditionMustHaveTemplate = GetBool(x, "MustHaveTemplate"),
                    ConditionOverlapWillDisable = GetBool(x, "OverlapWillDisable"),
                    ConditionOverlapCheckCount = GetInt(x, "OverlapCheckCount"),
                    ConditionOverlapType = GetOverlapCountType(x, "OverlapType"),
                    ConditionOverlapOnlySameRotation = GetBool(x, "OverlapOnlySameRotation"),
                    ConditionOverlapOnlyEnableFirst = GetBool(x, "OverlapOnlyEnableFirst"),
                    ConditionOverlapOnlyEnableMiddle = GetBool(x, "OverlapOnlyEnableMiddle"),
                    ConditionOverlapOnlySameTemplate = GetBool(x, "OverlapOnlySameTemplate"),
                    ConditionOverlapOnlySameClass = GetBool(x, "OverlapOnlySameClass"),
                    ConditionOverlapOnlySameType = GetBool(x, "OverlapOnlySameType"),
                }),
            ]
        };
    }
    private WorldObjectType WorldObjectTypeFromFilename(string filename)
    {
        if (filename.Contains("floor"))
            return WorldObjectType.Floor;
        else if (filename.Contains("ceiling"))
            return WorldObjectType.Ceiling;
        else if (filename.Contains("wall"))
            return WorldObjectType.Wall;
        else if (filename.Contains("generic"))
            return WorldObjectType.Generic;
        else if (filename.Contains("icorner"))
            return WorldObjectType.InnerCorner;
        else if (filename.Contains("ocorner"))
            return WorldObjectType.OuterCorner;
        else if (filename.Contains("doorframe"))
            return WorldObjectType.DoorFrame;
        else
            return WorldObjectType.Generic;
    }
    private WorldObjectItem TileFromFloor(string filename, WorldObjectItem floor, MDL mdl)
    {
        var wallNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.wall."));
        var cornerNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.corner."));
        var outerCornerNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.ocorner."));
        var innerCornerNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.icorner."));
        var floorNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.floor."));
        var ceilingNodes = mdl.Root.GetAllDescendants().Where(x => x.Name.Contains("magnet.ceiling."));

        return new WorldObjectItem()
        {
            Type = WorldObjectType.Tile,
            KitID = KitID,
            TemplateID = filename.Replace("floor_", "tile_"),
            ClassID = ClassIDFromMDL(mdl),
            Name = NameFromMDL(mdl).Replace("Floor", "Tile"),
            Magnets =
            [
                new MagnetItem()
                {
                    KitID = KitID,
                    TemplateID = $"floor_{filename.Replace("floor_", "")}",
                    Position = new(Vector3.Zero),
                    Orientation = new(Quaternion.Identity),
                    
                },
                new MagnetItem()
                {
                    KitID = KitID,
                    TemplateID = $"ceiling_{filename.Replace("floor_", "")}",
                    Position = new(Vector3.Zero),
                    Orientation = new(Quaternion.Identity),
                },
                ..wallNodes.Select(x => new MagnetItem()
                {
                    KitID = KitID,
                    TemplateID = $"wall_{x.Name.Split('.').Last()}",
                    Position = PositionFromNode(x),
                    Orientation = OrientationFromNode(x),
                }),
                ..cornerNodes.Concat(innerCornerNodes).Select(x => new MagnetItem()
                {
                    KitID = KitID,
                    TemplateID = $"icorner_{x.Name.Split('.').Last()}",
                    Position = PositionFromNode(x),
                    Orientation = OrientationFromNode(x),
                }),
                ..cornerNodes.Concat(outerCornerNodes).Select(x => new MagnetItem()
                {
                    KitID = KitID,
                    TemplateID = $"ocorner_{x.Name.Split('.').Last()}",
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
    private string HookKitIDFromNode(MDLNode node)
    {
        const string searchTerm = "_KitID=";
        var target = node.Children.SingleOrDefault(x => x.Name.Contains(searchTerm));
        return (target is null) ? "" : target.Name.Substring(target.Name.IndexOf(searchTerm)+searchTerm.Length);
    }
    private string TemplateIDFromNode(MDLNode node)
    {
        const string searchTerm = "_TemplateID=";
        var target = node.Children.SingleOrDefault(x => x.Name.Contains(searchTerm));
        return (target is null) ? "" : target.Name.Substring(target.Name.IndexOf(searchTerm) + searchTerm.Length);
    }
    private string GetString(MDLNode node, string property)
    {
        var searchTerm = $"_{property}=";
        var target = node.Children.SingleOrDefault(x => x.Name.Contains(searchTerm));
        return (target is null) ? "" : target.Name.Substring(target.Name.IndexOf(searchTerm) + searchTerm.Length);
    }
    private bool GetBool(MDLNode node, string property)
    {
        return GetString(node, property) == "true";
    }
    private int GetInt(MDLNode node, string property)
    {
        return int.TryParse(GetString(node, property), out var result) ? result : 0;
    }
    private OverlapCountType GetOverlapCountType(MDLNode node, string property)
    {
        return GetString(node, property) switch
        {
            "equal" => OverlapCountType.EqualTo,
            "notequal" => OverlapCountType.NotEqualTo,
            "lessthan" => OverlapCountType.LessThan,
            "greaterthan" => OverlapCountType.GreaterThan,
            "ignore" => OverlapCountType.Ignore,
            _ => OverlapCountType.Ignore
        };  
    }
}
