using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate;

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
        TemplateID = template.ObjectID;
    }

    public Tile Extend(TileTemplate template)
    {
        return Parent.Extend(this, template);
    }

    public void SwitchTemplate(WallTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ObjectID;

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
