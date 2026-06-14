using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Floor
{
    public Tile Parent { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public FloorTemplate Template => Kit.Manager.Get(KitID).Floor(TemplateID);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Parent.Transform;

    public Floor(Tile parent, FloorTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(FloorTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}
