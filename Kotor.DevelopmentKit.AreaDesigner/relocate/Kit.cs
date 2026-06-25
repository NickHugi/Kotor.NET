using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Kit
{
    public static KitManager Manager { get; } = new();

    public string ID { get; }
    public string Name { get; }
    public string FilePath { get; }
    public int Version { get; }
    public ICollection<FloorTemplate> Floors { get; init; } = [];
    public ICollection<TileTemplate> Tiles { get; init; } = [];
    public ICollection<WallTemplate> Walls { get; init; } = [];
    public ICollection<DoorFrameTemplate> DoorFrames { get; init; } = [];
    public ICollection<CeilingTemplate> Ceilings { get; init; } = [];
    public ICollection<InnerCornerTemplate> InnerCorners { get; init; } = [];
    public ICollection<OuterCornerTemplate> OuterCorners { get; init; } = [];
    public ICollection<ObjectTemplate> Objects { get; init; } = [];

    public IReadOnlyCollection<ObjectTemplate> AllObjects =>
    [
        ..Tiles,
        ..Floors,
        ..Walls,
        ..DoorFrames,
        ..Ceilings,
        ..InnerCorners,
        ..OuterCorners,
        ..Objects
    ];

    public FloorTemplate Floor(string id) => Floors.Single(x => x.TemplateID == id);
    public TileTemplate Tile(string id) => Tiles.Single(x => x.TemplateID == id);
    public WallTemplate Wall(string id) => Walls.Single(x => x.TemplateID == id);
    public DoorFrameTemplate DoorFrame(string id) => DoorFrames.Single(x => x.TemplateID == id);
    public CeilingTemplate Ceiling(string id) => Ceilings.Single(x => x.TemplateID == id);
    public InnerCornerTemplate InnerCorner(string id) => InnerCorners.Single(x => x.TemplateID == id);
    public OuterCornerTemplate OuterCorner(string id) => OuterCorners.Single(x => x.TemplateID == id);
    public ObjectTemplate Object(string id) => Objects.Single(x => x.TemplateID == id);

    public Kit(string filepath, string id, int version, string name)
    {
        FilePath = filepath;
        ID = id;
        Name = name;
        Version = version;
    }   
}

public class KitManager
{
    public string ActiveDirectory = @"C:/Kits";
    public ICollection<Kit> Kits { get; } = [];

    public void Refresh()
    {
        Kits.Clear();
        Directory.GetFiles(Kit.Manager.ActiveDirectory)
            .Where(x => Path.GetExtension(x).ToLower() == ".kit")
            .Select(KitSerializer.Load)
            .ToList()
            .ForEach(Kits.Add);
    }
    public Kit Get(string id) => Kits.Single(x => x.ID == id);
}
