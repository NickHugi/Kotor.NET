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
            new("sandral_wall_0", new( 0,  4, 0), new(0, 0, -0.707f, 0.707f)),
            new("sandral_wall_0", new( 4,  0, 0), new(0, 0, -1, 0)),
            new("sandral_wall_0", new( 0, -4, 0), new(0, 0, 0.707f, 0.707f)),
            new("sandral_wall_0", new(-4,  0, 0), new(0, 0, 0, 1)),
        ],
        CornerHooks =
        [
            new( 4,  4, 0),
            new( 4, -4, 0),
            new(-4, -4, 0),
            new(-4, -4, 0)
        ]
    };

    public string DefaultFloorModel { get; init; }
    public WallTemplate[] Walls = [];
    public Vector3[] CornerHooks = [];
    public Vector3[] CeilingHooks = [];
}

public class WallTemplate
{
    public string DefaultModel { get; }
    public Vector3 Position { get; }
    public Quaternion Orientation { get; }

    public WallTemplate(string model, Vector3 position, Quaternion orientation)
    {
        DefaultModel = model;
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
