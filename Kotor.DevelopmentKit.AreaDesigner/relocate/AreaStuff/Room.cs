using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Avalonia.Markup.Xaml.Templates;
using DynamicData;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Mode;

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

    public ICollection<UltimateWorldObject> Objects = [];

    public ICollection<UltimateWorldObject> AllObjects
    {
        get
        {
            var search = Objects.ToList();
            var result = new List<UltimateWorldObject>();

            while (search.Any())
            {
                var next = search.First();
                search.Remove(next);
                search.AddRange(next.AttachedObjects);
                result.Add(next);
            }

            return result;
        }
    }
    public ICollection<Magnet> AllMagnets
    {
        get => AllObjects.SelectMany(x => x.Magnets).ToList();
    }

    public Room(Area parent)
    {
        Parent = parent;
    }
    public Room(Area parent, UltimateWorldObjectTemplate template) : this(parent)
    {
        var tile = new UltimateWorldObject(this, null, template, Guid.NewGuid(), WorldObjectType.Tile);
        tile.SwitchTemplate(template);
        AddTile(tile);
    }

    public void AddObject(UltimateWorldObject @object)
    {
        Objects.Add(@object);
    }
    public void AddTile(UltimateWorldObject tile)
    {
        Objects.Add(tile);
        FixWalls();
    }
    public void DeleteTile(UltimateWorldObject tile)
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
        // TODO
        return [];
        //return Objects.SelectMany(x => x.Magnets).ToList();
    }

    private void FixWalls()
    {
        // TODO
        //foreach (var wall in Objects.OfType<Tile>().SelectMany(x => x.AttachedObjects.OfType<UltimateWorldObject>()))
        //{
        //    wall.LinkedTile = null;
        //}

        //foreach (var tileA in Objects.OfType<Tile>())
        //{
        //    foreach (var tileB in Objects.OfType<Tile>())
        //    {
        //        if (tileA == tileB)
        //            continue;

        //        foreach (var adjacent in GetCombinations(tileA.AttachedObjects.OfType<UltimateWorldObject>(), tileB.AttachedObjects.OfType<UltimateWorldObject>()))
        //        {
        //            if (Vector3.Distance(adjacent.Item1.GlobalPosition, adjacent.Item2.GlobalPosition) < 0.01f)
        //            {
        //                adjacent.Item1.LinkedTile = tileB;
        //                adjacent.Item2.LinkedTile = tileA;
        //            }
        //        }
        //    }
        //}
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
