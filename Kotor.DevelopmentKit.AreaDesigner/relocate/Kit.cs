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
    public ICollection<CornerHookTemplate> InsideCorners { get; init; } = [];
    public ICollection<CornerHookTemplate> OutsideCorners { get; init; } = [];
    public ICollection<ObjectTemplate> Objects { get; init; } = [];

    public FloorTemplate Floor(string id) => Floors.Single(x => x.ID == id);
    public TileTemplate Tile(string id) => Tiles.Single(x => x.ID == id);
    public WallTemplate Wall(string id) => Walls.Single(x => x.ID == id);
    public DoorFrameTemplate DoorFrame(string id) => DoorFrames.Single(x => x.ID == id);
    public CeilingTemplate Ceiling(string id) => Ceilings.Single(x => x.ID == id);
    public CornerHookTemplate InsideCorner(string id) => InsideCorners.Single(x => x.ID == id);
    public CornerHookTemplate OutsideCorner(string id) => OutsideCorners.Single(x => x.ID == id);
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
    public ICollection<Kit> Kits { get; } = [];

    public void Refresh()
    {
        Kits.Clear();
        Directory.GetFiles(@"C:\Kits").Select(KitSerializer.Load).ToList().ForEach(Kits.Add);
    }
    public Kit Get(string id) => Kits.Single(x => x.ID == id);
}
