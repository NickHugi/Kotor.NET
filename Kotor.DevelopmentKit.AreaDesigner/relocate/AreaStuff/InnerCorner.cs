using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class InnerCorner
{
    public Tile Parent { get; }
    public InnerCornerHookTemplate Hook { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public InnerCornerTemplate Template => Kit.Manager.Get(KitID).InnerCorner(TemplateID);

    public string? GroupID { get; set; }

    public Vector3 LocalPosition => Hook.LocalPosition;
    public Quaternion LocalOrientation => Hook.LocalOrientation;

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Hook.LocalTransform * Parent.Transform;

    public bool Visible
    {
        get
        {
            return Hook.Adjacent.Any() && Hook.Adjacent.All(x => Parent.Walls.ElementAt(x).LinkedTile is null);
        }
    }
    
    public InnerCorner(Tile parent, InnerCornerTemplate template, InnerCornerHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(InnerCornerTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ObjectID;
    }
}
