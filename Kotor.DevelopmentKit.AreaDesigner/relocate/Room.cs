using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DynamicData;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Area
{
    private List<Room> _rooms = new();
    public IReadOnlyList<Room> Rooms => _rooms.AsReadOnly();

    public void AddRoom(Room room)
    {
        _rooms.Add(room);
    }
}

public class Room
{
    public Tile Root { get; private set; }

    public Vector3 Position { get; set; } = new();
    public Quaternion Orientation { get; set; } = new();
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    public ICollection<Tile> Tiles => GetAllTiles();
    public ICollection<Wall> Walls => Tiles.SelectMany(x => x.Walls).ToList();
    public ICollection<Corner> Corners => Tiles.SelectMany(x => x.InnerCorners).Concat(Tiles.SelectMany(x => x.OuterCorners)).ToList();
    public ICollection<Corner> InnerCorners => Tiles.SelectMany(x => x.InnerCorners).ToList();
    public ICollection<Corner> OuterCorners => Tiles.SelectMany(x => x.OuterCorners).ToList();
    public ICollection<DoorFrame> DoorFrames => Walls.Select(x => x.DoorFrame).Where(x => x is not null).ToList();

    public Room(RoomTemplate template)
    {
        Root = new(this, TileTemplate.Sandral);
    }

    public ICollection<Tile> GetAllTiles()
    {
        List<Tile> found = [];
        List<Tile> scan = [Root];
        while (scan.Any())
        {
            var tile = scan.First();
            found.Add(tile);

            scan.RemoveAt(0);
            scan.AddRange(tile.Walls.Select(x => x.LinkedTile).Where(x => x is not null).Where(x => !found.Contains(x))!);
        }
        return found;
    }
}

public class Tile
{
    public Room Parent { get; }
    public TileTemplate Template { get; }
    public Floor Floor { get; }
    public IReadOnlyCollection<Wall> Walls { get; }
    public IReadOnlyCollection<Corner> InnerCorners { get; }
    public IReadOnlyCollection<Corner> OuterCorners { get; }

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation { get; set; } = new(0, 0, 0, 1);

    public Vector3 Position => Parent.Position + LocalPosition;
    public Quaternion Orientation => Parent.Orientation * LocalOrientation;
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition) * Parent.Transform;

    public Tile(Room parent, TileTemplate template)
    {
        Parent = parent;
        Template = template;
        Floor = new(template.DefaultFloorModel);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new Corner(this, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new Corner(this, x)).ToArray();
    }

    public Tile Extend(Wall wall)
    {
        var newTile = new Tile(Parent, TileTemplate.Sandral);

        // TODO - will need to handle this differently. only works for square rooms
        var adjacent = newTile.Walls.ElementAt((Walls.IndexOf(wall) + 2) % 4);
        newTile.LocalPosition = wall.LocalPosition - adjacent.Hook.Position;

        // Link the new tile to the old tile, as well as any other touching tiles
        foreach (var newWall in newTile.Walls)
        {
            foreach (var otherTileWall in Parent.GetAllTiles().Where(x => x != newTile).SelectMany(x => x.Walls))
            {
                if (Vector3.Distance(newWall.Position, otherTileWall.Position) < 0.01f)
                {
                    newWall.LinkedTile = this;
                    otherTileWall.LinkedTile = newTile; 
                }
            }
        }

        return newTile;
    }

    public void SwitchWall(Wall wall, WallTemplate template)
    {
        wall.Template = template;

        if (template.DoorFrame is not null)
        {
            wall.DoorFrame = new(wall, template.DoorFrame);
        }
        else
        {
            wall.DoorFrame = null;
        }
    }
}

public class Wall
{
    public Tile Parent { get; }
    public Room? LinkedRoom { get; set; }
    public Tile? LinkedTile { get; set; }
    public DoorFrame? DoorFrame { get; set; }
    public WallTemplate Template
    {
        get;
        set
        {
            field = value;
            DoorFrame = (value.DoorFrame is null) ? null : new(this, value.DoorFrame);
        }
    }
    public WallHook Hook { get; set; }
    

    public Vector3 LocalPosition => Parent.LocalPosition + Hook.Position;
    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Parent.Orientation * Hook.Orientation;
    public Matrix4x4 Transform => Hook.Transform * Parent.Transform;

    public Wall(Tile parent, WallTemplate template, WallHook hook)
    {
        Parent = parent;
        Template = template;
        Hook = hook;
    }
}

public class Floor
{
    public string Model { get; set; }

    public Floor(string model)
    {
        Model = model;
    }
}

public class Ceiling
{
    public string Model { get; set; }

    public Ceiling(string model)
    {
        Model = model;
    }
}

public class Corner
{
    public Tile Parent { get; }
    public CornerTemplate Template { get; set; }

    public Vector3 Position => Template.Position;
    public Quaternion Orientation => Template.Orientation;
    public Matrix4x4 Transform => Template.Transform * Parent.Transform;

    public Corner(Tile parent, CornerTemplate template)
    {
        Parent = parent;
        Template = template;
    }
}

public class DoorFrame
{
    public Wall Parent { get; }
    public DoorFrameTemplate Template { get; set; }
    public bool Enabled { get; set; } = true;

    public Vector3 LocalPosition => Template.Hooks.Last().Position;
    public Quaternion LocalOrientation => Template.Hooks.Last().Orientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    //public Quaternion Orientation => new();
    public Matrix4x4 Transform => LocalTransform * Parent.Transform;

    public DoorFrame(Wall parent, DoorFrameTemplate template)
    {
        Parent = parent;
        Template = template;
    }
}
