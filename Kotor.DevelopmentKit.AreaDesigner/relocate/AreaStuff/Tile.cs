using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Tile : IDeleteable
{
    public Room Parent { get; }
    public WorldObjectType Type => WorldObjectType.Tile;

    public Floor Floor { get; private set; }
    public Ceiling Ceiling { get; private set; }
    public IReadOnlyCollection<Wall> Walls { get; private set; }
    public IReadOnlyCollection<InnerCorner> InnerCorners { get; private set; }
    public IReadOnlyCollection<OuterCorner> OuterCorners { get; private set; }

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
        Ceiling = new(this, template.Ceiling);
        Walls = template.Walls.Select(x => new Wall(this, x.DefaultTemplate, x)).ToArray();
        InnerCorners = template.InnerCorners.Select(x => new InnerCorner(this, x.DefaultTemplate, x)).ToArray();
        OuterCorners = template.OuterCorners.Select(x => new OuterCorner(this, x.DefaultTemplate, x)).ToArray();
    }

    public Tile Extend(Wall wall, TileTemplate template)
    {
        var newTile = new Tile(Parent, template);

        // todo - first compatible
        var adjacent = newTile.Walls
            .Where(x => x.Template.ObjectID == wall.Template.ObjectID)
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

        Parent.AddTile(newTile);

        return newTile;
    }

    public void SwitchTemplate(TileTemplate template)
    {
        //Template = template;
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
