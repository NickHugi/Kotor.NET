using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

public class Room
{
    public Tile Root { get; private set; } = new(TileTemplate.Sandral);
    public Vector3 Position { get; set; } = new();
    public Quaternion Orientation { get; set; } = new();

    public Room(RoomTemplate template)
    {

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
    public TileTemplate Template { get; }
    public Floor Floor { get; }
    public IReadOnlyCollection<Wall> Walls { get; }
    public IReadOnlyCollection<Corner> InnerCorners { get; }
    public IReadOnlyCollection<Corner> OuterCorners { get; }
    public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

    public Tile(TileTemplate template)
    {
        Template = template;
        Floor = new(template.DefaultFloorModel);
        Walls = template.Walls.Select(x => new Wall(x)).ToArray();
        InnerCorners = template.InnerCorners;
        OuterCorners = template.OuterCorners;
    }

    public Tile Extend(Wall wall)
    {
        var newTile = new Tile(TileTemplate.Sandral);
        wall.LinkedTile = newTile;

        var adjacent = newTile.Walls.ElementAt((Walls.IndexOf(wall) + 2) % 4);
        adjacent.LinkedTile = this;
        newTile.Transform = Matrix4x4.CreateTranslation(wall.Template.Position - adjacent.Template.Position);
        return newTile;
    }

    public void SwitchWall(Wall wall, string model)
    {
        wall.Model = model;
    }
}

public class Wall
{
    public string Model { get; set; }
    public Room? LinkedRoom { get; set; }
    public Tile? LinkedTile { get; set; }
    public WallTemplate Template { get; set; }

    public Wall(WallTemplate template)
    {
        Template = template;
        Model = template.DefaultModel;
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

public class RoomTile
{
    public string FloorModel { get; set; } = "sandral_floor_0";
    public TileTemplate TileTemplate { get; set; } = TileTemplate.Sandral;
}

public class Corner
{
    public string Model { get; set; }
    public Matrix4x4 Transform { get; }
    public (int IndexA, int IndexB) Requires { get; }

    public Corner(string defaultModel, Vector3 position, Quaternion orientation, (int IndexA, int IndexB) requires)
    {
        Model = defaultModel;
        Transform = Matrix4x4.CreateFromQuaternion(orientation) * Matrix4x4.CreateTranslation(position);
        Requires = requires;
    }
}
