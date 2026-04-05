using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class RoomTemplate
{
}

public class FloorTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
}

public class TileTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string DefaultFloorID { get; init; }
    public required string DefaultCeilingID { get; init; }
    public required WallHookTemplate[] Walls { get; init; }
    public required CornerHookTemplate[] InnerCorners { get; init; }
    public required CornerHookTemplate[] OuterCorners { get; init; }
    public required Vector3[] CeilingHooks { get; init; }

    public FloorTemplate Floor => Kit.Manager.Get(KitID).Floor(DefaultFloorID);
}


public class WallTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
    public required string DoorFrameID { get; init; }

    public DoorFrameTemplate? DoorFrame => (DoorFrameID is not null) ? Kit.Manager.Get(KitID).DoorFrame(DoorFrameID) : null;
    public bool CanBeDoor => DoorFrame is not null;
}
public class WallHookTemplate
{
    public required string DefaultWallID { get; init; }
    public WallTemplate DefaultTemplate => Kit.Manager.Get("sandral").Wall(DefaultWallID);

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public int[] AdjacentWalls { get; init; } = [];

    //public ICollection<string> CompatibleWallTemplates { get; }
    //public ICollection<string> CompatibleTileTemplates { get; }
}
public class CornerHookTemplate
{
    public required string ID { get; init; }
    public string Model => ID; // todo
    public required Vector3 Position { get; init; }
    public required Quaternion Orientation { get; init; }
    public required int[] Adjacent { get; init; }

    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

}

public class DoorFrameTemplate
{
    public required string KitID { get; init; }
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
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
}

public class ObjectTemplate
{
    public required string KitID { get; init; }
    public required string ID { get; init; }
    public required string Name { get; init; }
    public required string Model { get; init; }
}
