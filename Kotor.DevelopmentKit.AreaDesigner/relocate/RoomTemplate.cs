using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class RoomTemplate
{
}

public class TileTemplate
{
    public static TileTemplate Sandral = new TileTemplate
    {
        DefaultFloorModel = "sandral_floor_0",
        Walls =
        [
            new(WallTemplate.SandralWall0a, new( 0,  4, 0), new(0, 0, -0.707f, 0.707f)),
            new(WallTemplate.SandralWall0a, new( 4,  0, 0), new(0, 0, -1, 0)),
            new(WallTemplate.SandralWall0a, new( 0, -4, 0), new(0, 0, 0.707f, 0.707f)),
            new(WallTemplate.SandralWall0a, new(-4,  0, 0), new(0, 0, 0, 1)),
        ],
        InnerCorners =
        [
            new("sandral_icorner_0", new( 4,  4, 0), new(0, 0, 0.9238f, -0.382683f), (0, 1)),
            new("sandral_icorner_0", new( 4, -4, 0), new(0, 0, 0.9238f, 0.382683f), (1, 2)),
            new("sandral_icorner_0", new(-4, -4, 0), new(0, 0, -0.382683f, -0.92388f), (2, 3)),
            new("sandral_icorner_0", new(-4,  4, 0), new(0, 0, 0.382683f, -0.9238f), (3, 0)),
        ],
        OuterCorners =
        [
            new("sandral_ocorner_0", new( 4,  4, 0), new(0, 0, 0.9238f, -0.382683f), (0, 1)),
            new("sandral_ocorner_0", new( 4, -4, 0), new(0, 0, 0.9238f, 0.382683f), (1, 2)),
            new("sandral_ocorner_0", new(-4, -4, 0), new(0, 0, -0.382683f, -0.92388f), (2, 3)),
            new("sandral_ocorner_0", new(-4,  4, 0), new(0, 0, 0.382683f, -0.9238f), (3, 0)),
        ],
    };

    public string DefaultFloorModel { get; init; }
    public WallHook[] Walls = [];
    public CornerTemplate[] InnerCorners = [];
    public CornerTemplate[] OuterCorners = [];
    public Vector3[] CeilingHooks = [];
}

public class WallHook
{
    public WallTemplate DefaultTemplate { get; set; }

    public Vector3 Position { get; }
    public Quaternion Orientation { get; }
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    public WallHook(WallTemplate defaultTemplate, Vector3 position, Quaternion orientation)
    {
        DefaultTemplate = defaultTemplate;
        Position = position;
        Orientation = orientation;
    }
}
public class WallTemplate
{
    public static WallTemplate SandralWall0a = new("sandral_wall_0", null);
    public static WallTemplate SandralWall0b = new("sandral_wall_0_door_0", DoorFrameTemplate.SandralDoor0);
    public static WallTemplate SandralWall0c = new("sandral_wall_0_door_1", DoorFrameTemplate.SandralDoor0);

    public string Model { get; }
    public DoorFrameTemplate? DoorFrame { get; }
    public bool CanBeDoor => DoorFrame is not null;

    public WallTemplate(string model, DoorFrameTemplate? doorframe)
    {
        Model = model;
        DoorFrame = doorframe;
    }
}

public class DoorFrameTemplate
{
    public static DoorFrameTemplate SandralDoor0 = new("sandral_doorframe_0",
        [
            new(new(0.752678f, 0, 0), new(0, 0, 0, 1)),
            new(new(-0.334811f, 0, 0), new(0, 0, -1, 0)),
        ]);
    public static DoorFrameTemplate SandralDoor1 = new("sandral_doorframe_0",
        [ // todo
            new(new(0.752678f, 0, 0), new(0, 0, 0, 1)),
            new(new(-0.334811f, 0, 0), new(0, 0, -1, 0)),
        ]);

    public string Model { get; }
    public DoorFrameHook[] Hooks { get; }

    public DoorFrameTemplate(string model, DoorFrameHook[] hooks)
    {
        Model = model;
        Hooks = hooks;
    }
}
public class DoorFrameHook
{
    public Vector3 Position { get; }
    public Quaternion Orientation { get; }

    public DoorFrameHook(Vector3 position, Quaternion orientation)
    {
        Position = position;
        Orientation = orientation;
    }
}


public class CeilingTemplate
{
    public string DefaultModel { get; }

    public CeilingTemplate(string defaultModel)
    {
        DefaultModel = defaultModel;
    }
}

public class CornerTemplate
{
    public string Model { get; }
    public Vector3 Position { get; }
    public Quaternion Orientation { get; }
    public (int IndexA, int IndexB) Requires { get; }

    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    public CornerTemplate(string model, Vector3 position, Quaternion orientation, (int IndexA, int IndexB) requires)
    {
        Model = model;
        Position = position;
        Orientation = orientation;
        Requires = requires;
    }
}
