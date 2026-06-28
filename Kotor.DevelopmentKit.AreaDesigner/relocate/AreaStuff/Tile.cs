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
        get => VirtualObjects.SelectMany(x =>
        {
            IEnumerable<Magnet> magnets = [];

            if (x is Wall wall)
            {
                if (wall.DoorFrame is null)
                {
                    magnets = wall.Magnets;
                }
                else
                {
                    magnets = wall.DoorFrame.Magnets;
                }
            }

            return magnets.Where(x =>
                (x.Parent is DoorFrame doorframe)
                || (x.Parent is Wall wall && wall?.LinkedTile is null));
        }).ToList();
    }
    public WorldObjectType Type => WorldObjectType.Tile;
    public IReadOnlyCollection<IWorldObject> VirtualObjects { get; private set; } = [];

    public string? GroupID { get; set; }

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

    public Tile(Room parent)
    {
        Parent = parent;
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
        TemplateID = template.TemplateID;

        var kit = Kit.Manager.Get(KitID);

        var virtualObjects = new List<IWorldObject>();
        VirtualObjects = virtualObjects;

        virtualObjects.AddRange(
        [
            ..template.Hooks.OfType<FloorHookTemplate>().Select(x => new Floor(this, kit.Floor(x.TemplateID))),
            ..template.Hooks.OfType<CeilingHookTemplate>().Select(x => new Ceiling(this, kit.Ceiling(x.TemplateID))),
            ..template.Hooks.OfType<WallHookTemplate>().Select(x => new Wall(this, kit.Wall(x.TemplateID), x)),
            ..template.Hooks.OfType<CornerHookTemplate>().Select(x => new InnerCorner(this, kit.InnerCorner(x.InnerTemplateID), x)),
            ..template.Hooks.OfType<CornerHookTemplate>().Select(x => new OuterCorner(this, kit.OuterCorner(x.OuterTemplateID), x)),
        ]);
    }

    public void Delete()
    {
        Parent.DeleteTile(this);
    }
}
