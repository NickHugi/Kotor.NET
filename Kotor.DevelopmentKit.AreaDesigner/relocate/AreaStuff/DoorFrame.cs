using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrame
{
    public Wall Parent { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public DoorFrameTemplate Template => Kit.Manager.Get(KitID).DoorFrame(TemplateID);

    public bool Enabled { get; set; } = true;

    public IEnumerable<DoorFrameHook> Hooks => Template.Hooks.Select(x => new DoorFrameHook(this, x));

    public Vector3 LocalPosition => Template.Hooks.Last().Position;
    public Quaternion LocalOrientation => Template.Hooks.Last().Orientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => LocalTransform * Parent.Transform;

    public bool Visible => Enabled;

    public DoorFrame(Wall parent, DoorFrameTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(DoorFrameTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}
