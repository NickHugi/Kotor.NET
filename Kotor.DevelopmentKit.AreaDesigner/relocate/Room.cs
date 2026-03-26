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

    public ICollection<Tile> Tiles { get; } = new List<Tile>();//GetAllTiles();
    public ICollection<Wall> Walls => Tiles.SelectMany(x => x.Walls).ToList();
    public ICollection<Corner> Corners => Tiles.SelectMany(x => x.InnerCorners).Concat(Tiles.SelectMany(x => x.OuterCorners)).ToList();
    public ICollection<Corner> InnerCorners => Tiles.SelectMany(x => x.InnerCorners).ToList();
    public ICollection<Corner> OuterCorners => Tiles.SelectMany(x => x.OuterCorners).ToList();
    public ICollection<DoorFrame> DoorFrames => Walls.Select(x => x.DoorFrame).Where(x => x is not null).ToList();

    public Room(RoomTemplate template)
    {
        //Root = new(this, TileTemplate.Sandral8x8);
        Tiles.Add(new(this, TileTemplate.Sandral8x8));
    }

    private ICollection<Tile> GetAllTiles()
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



    public void FixWalls()
    {
        foreach (var tileA in Tiles)
        {
            foreach (var tileB in Tiles)
            {
                if (tileA == tileB)
                    continue;

                foreach (var adjacent in GetCombinations(tileA.Walls, tileB.Walls))
                {
                    if (Vector3.Distance(adjacent.Item1.Position, adjacent.Item2.Position) < 0.01f)
                    {
                        adjacent.Item1.LinkedTile = tileB;
                        adjacent.Item2.LinkedTile = tileA;
                    }
                }
            }
        }
    }

    private List<(T Item1, T Item2)> GetCombinations<T>(IEnumerable<T> listA, IEnumerable<T> listB)
    {
        // TODO convert to list extensions method?

        List<(T A, T B)> combinations = new();

        foreach (var a in listA)
        {
            foreach (var b in listB)
            {
                var tuple = (a, b);
                if (!combinations.Contains(tuple))
                    combinations.Add(tuple);
            }
        }

        return combinations;
    }
}

public class Tile
{
    public Room Parent { get; }
    public TileTemplate Template { get; private set; }
    public Floor Floor { get; private set; }
    public IReadOnlyCollection<Wall> Walls { get; private set; }
    public IReadOnlyCollection<Corner> InnerCorners { get; private set; }
    public IReadOnlyCollection<Corner> OuterCorners { get; private set; }

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation { get; set; } = new(0, 0, 0, 1);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
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

    public Tile Extend(Wall wall, TileTemplate template)
    {
        var newTile = new Tile(Parent, template);

        var adjacent = newTile.Walls.ElementAt(0);
        var rotate = Quaternion.Identity;
        newTile.LocalOrientation = wall.Orientation * adjacent.Hook.LocalOrientation;
        newTile.LocalPosition = (wall.Position - Parent.Position);

        var pos = Vector3.Transform(adjacent.LocalPosition, Quaternion.Inverse(wall.Hook.LocalOrientation));

        newTile.LocalPosition -= Vector3.Transform(adjacent.Hook.LocalPosition, newTile.LocalOrientation);

        Parent.Tiles.Add(newTile);

        // Link the new tile to the old tile, as well as any other touching tiles
        Parent.FixWalls();

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

    public void SwitchTemplate(TileTemplate template)
    {
        Template = template;
        Floor = new(template.DefaultFloorModel);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new Corner(this, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new Corner(this, x)).ToArray();
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

    public Vector3 LocalPosition => Hook.LocalPosition;
    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Hook.LocalTransform * Parent.Transform;

    public bool Visible => LinkedTile is null;

    public Wall(Tile parent, WallTemplate template, WallHook hook)
    {
        Parent = parent;
        Template = template;
        Hook = hook;
    }

    public Tile Extend(TileTemplate template)
    {
        return Parent.Extend(this, template);
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

    public bool VisibleInner
    {
        get
        {
            if (Parent.Walls.ElementAt(Template.Requires.IndexA).LinkedTile is not null)
                return false;
            if (Parent.Walls.ElementAt(Template.Requires.IndexB).LinkedTile is not null)
                return false;

            return true;
        }
    }
    public bool VisibleOuter
    {
        get
        {
            return false;

            var linkedTileA = Parent.Walls.ElementAt(Template.Requires.IndexA).LinkedTile;
            var linkedTileB = Parent.Walls.ElementAt(Template.Requires.IndexB).LinkedTile;
            if (linkedTileA is null)
                return false;
            if (linkedTileB is null)
                return false;

            // TODO - logic will break for non-square rooms
            if (linkedTileA.Walls.ElementAt(Template.Requires.IndexB).LinkedTile is not null)
                return false;
            if (linkedTileB.Walls.ElementAt(Template.Requires.IndexA).LinkedTile is not null)
                return false;

            return true;
        }
    }

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

    public IEnumerable<DoorFrameHook> Hooks => Template.Hooks.Select(x => new DoorFrameHook(this, x));

    public Vector3 LocalPosition => Template.Hooks.Last().Position;
    public Quaternion LocalOrientation => Template.Hooks.Last().Orientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => LocalTransform * Parent.Transform;

    public bool Visible => Enabled;

    public DoorFrame(Wall parent, DoorFrameTemplate template)
    {
        Parent = parent;
        Template = template;
    }
}

public class DoorFrameHook
{
    public DoorFrame Parent { get; }
    public DoorFrameHookTemplate Template { get; }

    public Vector3 LocalPosition => Template.Position;
    public Quaternion LocalOrientation => Template.Orientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => LocalTransform * Parent.Transform;

    public DoorFrameHook(DoorFrame parent, DoorFrameHookTemplate template)
    {
        Parent = parent;
        Template = template;
    }
}
