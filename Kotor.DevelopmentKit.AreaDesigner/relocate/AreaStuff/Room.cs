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
    public Quaternion Orientation
    {
        get;
        set => field = Quaternion.Normalize(value);
    }
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    //private List<Tile> _tiles = new();
    //public IReadOnlyCollection<Tile> Tiles => new ReadOnlyCollection<Tile>(_tiles);

    //public ICollection<Wall> Walls => Tiles.SelectMany(x => x.Walls).ToList();
    //public ICollection<Floor> Floors => Tiles.Select(x => x.Floor).ToList();
    //public ICollection<Ceiling> Ceilings => Tiles.Select(x => x.Ceiling).ToList();
    //public ICollection<InnerCorner> InnerCorners => Tiles.SelectMany(x => x.InnerCorners).ToList();
    //public ICollection<OuterCorner> OuterCorners => Tiles.SelectMany(x => x.OuterCorners).ToList();
    //public ICollection<DoorFrame> DoorFrames => Walls.Select(x => x.DoorFrame).Where(x => x is not null).ToList();
    public ICollection<IWorldObject> Objects = [];

    public Room(Area parent)
    {
        Parent = parent;
    }
    public Room(Area parent, TileTemplate template) : this(parent)
    {
        var tile = new Tile(this);
        tile.SwitchTemplate(template);
        AddTile(tile);
    }

    public void AddObject(IWorldObject @object)
    {
        Objects.Add(@object);
    }
    public void AddTile(Tile tile)
    {
        Objects.Add(tile);
        FixWalls();
    }
    public void DeleteTile(Tile tile)
    {
        Objects.Remove(tile);
        FixWalls();

        if (Objects.Count() == 0)
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
        return Objects.SelectMany(x => x.Magnets).ToList();
    }

    private void FixWalls()
    {
        foreach (var wall in Objects.OfType<Tile>().SelectMany(x => x.VirtualObjects.OfType<Wall>()))
        {
            wall.LinkedTile = null;
        }

        foreach (var tileA in Objects.OfType<Tile>())
        {
            foreach (var tileB in Objects.OfType<Tile>())
            {
                if (tileA == tileB)
                    continue;

                foreach (var adjacent in GetCombinations(tileA.VirtualObjects.OfType<Wall>(), tileB.VirtualObjects.OfType<Wall>()))
                {
                    if (Vector3.Distance(adjacent.Item1.GlobalPosition, adjacent.Item2.GlobalPosition) < 0.01f)
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
