using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Templates
{
    public static Templates Store { get; } = new();
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
}

public class RoomTemplate
{
}

public class FloorTemplate
{
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
}

public class TileTemplate
{
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string FloorID { get; init; }
    public required WallHook[] Walls { get; init; }
    public required CornerTemplate[] InnerCorners { get; init; }
    public required CornerTemplate[] OuterCorners { get; init; }
    public required Vector3[] CeilingHooks { get; init; }

    public FloorTemplate Floor => Templates.Store.Floor(FloorID);
}

public class WallHook
{
    public required string DefaultWallID { get; init; }
    public WallTemplate DefaultTemplate => Templates.Store.Wall(DefaultWallID);

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public int[] AdjacentWalls { get; init; } = [];

    //public ICollection<string> CompatibleWallTemplates { get; }
    //public ICollection<string> CompatibleTileTemplates { get; }
}
public class WallTemplate
{
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
    public required string DoorFrameID { get; init; }

    public DoorFrameTemplate? DoorFrame => (DoorFrameID is not null) ? Templates.Store.DoorFrame(DoorFrameID) : null;
    public bool CanBeDoor => DoorFrame is not null;
}

public class DoorFrameTemplate
{
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
    public required DoorFrameHookTemplate[] Hooks { get; init; }
}
public class DoorFrameHookTemplate
{
    public required Vector3 Position { get; init; }
    public required Quaternion Orientation { get; init; }
}


public class CeilingTemplate
{
    public string ID { get; }
    public string Model { get; }

    public CeilingTemplate(string model)
    {
        Model = model;
    }
}

public class CornerTemplate
{
    public required string ID { get; init; }
    public string Model => ID; // todo
    public required Vector3 Position { get; init; }
    public required Quaternion Orientation { get; init; }
    public required int[] Adjacent { get; init; }

    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

}

public class ObjectTemplate
{
    public required string ID { get; init; }
    public required string Model { get; init; }
}
