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
    public Area Area { get; }

    public Vector3 Position { get; set; } = new();
    public Quaternion Orientation
    {
        get;
        set => field = Quaternion.Normalize(value);
    }
    public Matrix4x4 Transform => Matrix4x4.CreateFromQuaternion(Orientation) * Matrix4x4.CreateTranslation(Position);

    public ICollection<WorldObject> Objects = [];

    public ICollection<WorldObject> AllObjects
    {
        get
        {
            var search = Objects.ToList();
            var result = new List<WorldObject>();

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
        Area = parent;
    }
    public Room(Area parent, WorldObjectTemplate template) : this(parent)
    {
        var tile = new WorldObject(this, null, template, Guid.NewGuid(), WorldObjectType.Tile);
        tile.SwitchTemplate(template);
        AddTile(tile);
    }

    public void AddObject(WorldObject @object)
    {
        Objects.Add(@object);
    }
    public void AddTile(WorldObject tile)
    {
        Objects.Add(tile);
    }
    public void DeleteTile(WorldObject tile)
    {
        Objects.Remove(tile);

        if (Objects.Count() == 0)
        {
            Delete();
        }
    }

    public void Delete()
    {
        Area.DeleteRoom(this);
    }
}
