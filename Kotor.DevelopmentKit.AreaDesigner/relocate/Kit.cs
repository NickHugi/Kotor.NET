using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Kit
{
    public static KitManager Manager { get; } = new();

    public string Name { get; }
    public ICollection<FloorTemplate> Floors { get; } = [];
    public ICollection<TileTemplate> Tiles { get; } = [];
    public ICollection<WallTemplate> Walls { get; } = [];
    public ICollection<DoorFrameTemplate> DoorFrames { get; } = [];
    public ICollection<CeilingTemplate> Ceilings { get; } = [];
    public ICollection<CornerTemplate> InsideCorners { get; } = [];
    public ICollection<CornerTemplate> OutsideCorners { get; } = [];
    public ICollection<ObjectTemplate> Objects { get; } = [];

    public FloorTemplate Floor(string id) => Floors.Single(x => x.ID == id);
    public TileTemplate Tile(string id) => Tiles.Single(x => x.ID == id);
    public WallTemplate Wall(string id) => Walls.Single(x => x.ID == id);
    public DoorFrameTemplate DoorFrame(string id) => DoorFrames.Single(x => x.ID == id);
    public CeilingTemplate Ceiling(string id) => Ceilings.Single(x => x.ID == id);
    public CornerTemplate InsideCorner(string id) => InsideCorners.Single(x => x.ID == id);
    public CornerTemplate OutsideCorner(string id) => OutsideCorners.Single(x => x.ID == id);
    public ObjectTemplate Object(string id) => Objects.Single(x => x.ID == id);

    public Kit(string name)
    {
        Name = name;
    }   
}

public class KitManager
{
    public ICollection<Kit> Kits { get; } = [];

    public void Add(Kit kit) => Kits.Add(kit);
    public Kit Get(string name) => Kits.Single(x => x.Name == name);
}
