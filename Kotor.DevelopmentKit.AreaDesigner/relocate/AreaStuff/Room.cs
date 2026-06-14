using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Avalonia.Markup.Xaml.Templates;
using DynamicData;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Room
{
    public Area Parent { get; }

    public Vector3 Position { get; set; } = new();
    public Quaternion Orientation { get; set; } = new();
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    private List<Tile> _tiles = new();
    public IReadOnlyCollection<Tile> Tiles => new ReadOnlyCollection<Tile>(_tiles);

    public ICollection<Wall> Walls => Tiles.SelectMany(x => x.Walls).ToList();
    public ICollection<Floor> Floors => Tiles.Select(x => x.Floor).ToList();
    public ICollection<Ceiling> Ceilings => Tiles.Select(x => x.Ceiling).ToList();
    public ICollection<InnerCorner> InnerCorners => Tiles.SelectMany(x => x.InnerCorners).ToList();
    public ICollection<OuterCorner> OuterCorners => Tiles.SelectMany(x => x.OuterCorners).ToList();
    public ICollection<DoorFrame> DoorFrames => Walls.Select(x => x.DoorFrame).Where(x => x is not null).ToList();
    public ICollection<Object> Objects = [];

    public Room(Area parent)
    {
        Parent = parent;
    }
    public Room(Area parent, TileTemplate template) : this(parent)
    {
        var tile = new Tile(this, template);
        AddTile(tile);
    }

    public void AddObject(Object @object)
    {
        Objects.Add(@object);
    }
    public void AddTile(Tile tile)
    {
        _tiles.Add(tile);
        FixWalls();
    }
    public void DeleteTile(Tile tile)
    {
        _tiles.Remove(tile);
        FixWalls();

        if (Tiles.Count() == 0)
        {
            Delete();
        }
    }

    public void Delete()
    {
        Parent.DeleteRoom(this);
    }

    public List<Magnet> GetMagnets()
    {
        var magnets = new List<Magnet>();

        foreach (var wall in Walls)
        {
            magnets.Add(new()
            {
                Position = wall.Position,
                Orientation = wall.Orientation,
            });
        }

        return magnets;
    }

    private void FixWalls()
    {
        foreach (var wall in Tiles.SelectMany(x => x.Walls))
        {
            wall.LinkedTile = null;
        }

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
