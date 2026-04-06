using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.KitSerialization;

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

    public FloorTemplate Floor(string id) => Floors.Single(x => x.ID == id);
    public TileTemplate Tile(string id) => Tiles.Single(x => x.ID == id);
    public WallTemplate Wall(string id) => Walls.Single(x => x.ID == id);
    public DoorFrameTemplate DoorFrame(string id) => DoorFrames.Single(x => x.ID == id);
    public CeilingTemplate Ceiling(string id) => Ceilings.Single(x => x.ID == id);
    public InnerCornerTemplate InnerCorner(string id) => InnerCorners.Single(x => x.ID == id);
    public OuterCornerTemplate OuterCorner(string id) => OuterCorners.Single(x => x.ID == id);
    public ObjectTemplate Object(string id) => Objects.Single(x => x.ID == id);

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
