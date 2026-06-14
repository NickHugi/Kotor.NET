using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Ceiling
{
    public Tile Parent { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public CeilingTemplate Template => Kit.Manager.Get(KitID).Ceiling(TemplateID);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => Parent.Transform;

    public Ceiling(Tile parent, CeilingTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(CeilingTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}
