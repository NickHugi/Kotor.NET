using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Tile : IDeleteable, IWorldObject
{
    public Room Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets
    {
        get => Walls.SelectMany(x =>
        {
            IEnumerable<Magnet> magnets;
            if (x.DoorFrame is null)
            {
                magnets = x.Magnets;
            }
            else
            {
                magnets = x.DoorFrame.Magnets;
            }
            return magnets.Where(x =>
                (x.Parent is DoorFrame doorframe)
                || (x.Parent is Wall wall && wall?.LinkedTile is null));
        }).ToList();
    }
    public WorldObjectType Type => WorldObjectType.Tile;

    public string? GroupID { get; set; }

    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }
    public IReadOnlyCollection<Wall> Walls { get; private set; }
    public IReadOnlyCollection<InnerCorner> InnerCorners { get; private set; }
    public IReadOnlyCollection<OuterCorner> OuterCorners { get; private set; }

    public IReadOnlyCollection<IWorldObject> VirtualObjects =>
    [
        Floor,
        Ceiling,
        ..Walls,
        ..InnerCorners,
        ..OuterCorners
    ];

    public string KitID { get; private set; }
    public string TemplateID { get; private set; }
    public TileTemplate Template => Kit.Manager.Get(KitID).Tile(TemplateID);

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation
    {
        get;
        set => field = Quaternion.Normalize(value);
    } = Quaternion.Identity;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateTranslation(LocalPosition) * Matrix4x4.CreateFromQuaternion(LocalOrientation);

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(LocalPosition, Parent.Orientation) + Parent.Position;
        set => LocalPosition = Vector3.Transform(value - Parent.Position, Quaternion.Inverse(Parent.Orientation));
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * Parent.Orientation);
        set => LocalOrientation = value * Quaternion.Inverse(Parent.Orientation);
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.Transform;

    public Tile(Room parent, TileTemplate template)
    {
        Parent = parent;
        KitID = template.KitID;
        TemplateID = template.ID;
        Floor = new(this, template.Floor);
        Ceiling = new(this, template.Ceiling);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new InnerCorner(this, x.DefaultTemplate, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new OuterCorner(this, x.DefaultTemplate, x)).ToArray();
    }

    public void SwitchTemplate(ObjectTemplate template)
    {
        //if (template is not TileTemplate tileTemplate)
            throw new ArgumentException();

        //SwitchTemplate(tileTemplate);
    }
    public void SwitchTemplate(TileTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
        Floor = new(this, template.Floor);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new InnerCorner(this, x.DefaultTemplate, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new OuterCorner(this, x.DefaultTemplate, x)).ToArray();
    }

    public void Delete()
    {
        Parent.DeleteTile(this);
    }
}
