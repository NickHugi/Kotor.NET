using System;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class OuterCorner
{
    public Tile Parent { get; }
    public WorldObjectType Type => WorldObjectType.OuterCorner;

    public OuterCornerHookTemplate Hook { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public OuterCornerTemplate Template => Kit.Manager.Get(KitID).OuterCorner(TemplateID);

    public string? GroupID { get; set; }

    public Vector3 Position => Hook.LocalPosition;
    public Quaternion Orientation => Hook.LocalOrientation;
    public Matrix4x4 Transform => Hook.LocalTransform * Parent.Transform;

    public bool Visible
    {
        get
        {
            if (Hook.Adjacent.Count() != 2)
                return false;
            if (Hook.Adjacent.Any(x => Parent.Walls.ElementAt(x).LinkedTile is null))
                return false;

            var a = Parent.Walls.ElementAt(Hook.Adjacent[0]).LinkedTile!.Walls.Select(x => x.LinkedTile).Where(x => x != Parent);
            var b = Parent.Walls.ElementAt(Hook.Adjacent[1]).LinkedTile!.Walls.Select(x => x.LinkedTile).Where(x => x != Parent);

            var circuit = a.Intersect(b).Any();
            return !circuit;
        }
    }

    public OuterCorner(Tile parent, OuterCornerTemplate template, OuterCornerHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(OuterCornerTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ObjectID;
    }
}
