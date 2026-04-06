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
    public Vector3 Position { get; set; } = new();
    public Quaternion Orientation { get; set; } = new();
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    public ICollection<Tile> Tiles { get; } = new List<Tile>();
    public ICollection<Wall> Walls => Tiles.SelectMany(x => x.Walls).ToList();
    public ICollection<Corner> Corners => Tiles.SelectMany(x => x.InnerCorners).Concat(Tiles.SelectMany(x => x.OuterCorners)).ToList();
    public ICollection<Corner> InnerCorners => Tiles.SelectMany(x => x.InnerCorners).ToList();
    public ICollection<Corner> OuterCorners => Tiles.SelectMany(x => x.OuterCorners).ToList();
    public ICollection<DoorFrame> DoorFrames => Walls.Select(x => x.DoorFrame).Where(x => x is not null).ToList();
    public ICollection<Object> Objects = [];

    public Room()
    {
    }
    public Room(RoomTemplate template)
    {
        Tiles.Add(new(this, Kit.Manager.Get("sandral").Tiles.ElementAt(0)));
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

    public void AddObject(Object @object)
    {
        Objects.Add(@object);
    }

    // todo ienumerable extension
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

    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }
    public IReadOnlyCollection<Wall> Walls { get; private set; }
    public IReadOnlyCollection<Corner> InnerCorners { get; private set; }
    public IReadOnlyCollection<Corner> OuterCorners { get; private set; }

    public string KitID { get; private set; }
    public string TemplateID { get; private set; }
    public TileTemplate Template => Kit.Manager.Get(KitID).Tile(TemplateID);

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation { get; set; } = new(0, 0, 0, 1);
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => LocalTransform * Parent.Transform;

    public Tile(Room parent, TileTemplate template)
    {
        Parent = parent;
        KitID = template.KitID;
        TemplateID = template.ID;
        Floor = new(this, template.Floor);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new Corner(this, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new Corner(this, x)).ToArray();
    }

    public Tile Extend(Wall wall, TileTemplate template)
    {
        var newTile = new Tile(Parent, template);

        // todo - first compatible
        var adjacent = newTile.Walls
            .Where(x => x.Template.ID == wall.Template.ID)
            //.OrderBy(x => x.LocalOrientaiton == wall.LocalOrientaiton)
            .First();

        newTile.LocalOrientation = wall.LocalOrientation
            / adjacent.Hook.LocalOrientation
            * Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI)
            * Orientation
            / Parent.Orientation;

        newTile.LocalPosition = LocalPosition
            + Vector3.Transform(wall.LocalPosition, LocalOrientation)
            - Vector3.Transform(adjacent.LocalPosition, newTile.LocalOrientation);

        Parent.Tiles.Add(newTile);

        // Link the new tile to the old tile, as well as any other touching tiles
        Parent.FixWalls();

        return newTile;
    }

    public void SwitchTemplate(TileTemplate template)
    {
        //Template = template;
        KitID = template.KitID;
        TemplateID = template.ID;
        Floor = new(this, template.Floor);
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
    public WallHookTemplate Hook { get; set; }

    public string KitID { get; private set;}
    public string TemplateID { get; private set; }
    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);

    public Vector3 LocalPosition => Hook.LocalPosition;
    public Quaternion LocalOrientation => Hook.LocalOrientation;

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Hook.LocalTransform * Parent.Transform;

    public bool Visible => LinkedTile is null;

    public Wall(Tile parent, WallTemplate template, WallHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        KitID = template.KitID;
        TemplateID = template.ID;
    }

    public Tile Extend(TileTemplate template)
    {
        return Parent.Extend(this, template);
    }

    public void SwitchTemplate(WallTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;

        if (template.DoorFrame is not null)
        {
            DoorFrame = new(this, template.DoorFrame);
        }
        else
        {
            DoorFrame = null;
        }
    }
}

public class Floor
{
    public Tile Parent { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public FloorTemplate Template => Kit.Manager.Get(KitID).Floor(TemplateID);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Parent.Transform;

    public Floor(Tile parent, FloorTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(FloorTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}

public class Ceiling
{
    public Tile Parent { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public CeilingTemplate Template => Kit.Manager.Get(KitID).Ceiling(TemplateID);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Parent.Transform;

    public Ceiling(Tile parent, CeilingTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(CeilingTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}

public class Corner
{
    public Tile Parent { get; }
    public CornerHookTemplate Template { get; set; }

    public Vector3 Position => Template.Position;
    public Quaternion Orientation => Template.Orientation;
    public Matrix4x4 Transform => Template.Transform * Parent.Transform;

    public bool VisibleInner
    {
        get
        {
            return Template.Adjacent.Any() && Template.Adjacent.All(x => Parent.Walls.ElementAt(x).LinkedTile is null);
        }
    }
    public bool VisibleOuter
    {
        get
        {
            if(Template.Adjacent.Count() != 2)
                return false;
            if(Template.Adjacent.Any(x => Parent.Walls.ElementAt(x).LinkedTile is null))
                return false;

            var a = Parent.Walls.ElementAt(Template.Adjacent[0]).LinkedTile!.Walls.Select(x => x.LinkedTile).Where(x => x != Parent);
            var b = Parent.Walls.ElementAt(Template.Adjacent[1]).LinkedTile!.Walls.Select(x => x.LinkedTile).Where(x => x != Parent);

            var circuit = a.Intersect(b).Any();
            return !circuit;
        }
    }

    public Corner(Tile parent, CornerHookTemplate template)
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

public class Object
{
    public string TemplateID { get; set; }
    public ObjectTemplate Template => Kit.Manager.Get("sandral").Object(TemplateID);

    public Vector3 Position { get; set; }
    public Quaternion Orientation { get; set; }
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

}
