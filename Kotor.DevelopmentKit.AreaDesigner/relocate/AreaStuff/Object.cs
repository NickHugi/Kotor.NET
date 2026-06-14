using System;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Object : IObject
{
    public Room Parent { get; }

    public string KitID { get; private set; }
    public string TemplateID { get; private set; }
    public ObjectTemplate Template => Kit.Manager.Get(KitID).Object(TemplateID);

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation { get; set; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    { 
        get => Matrix4x4.Decompose(GlobalTransform, out _, out _, out var value) ? value : new();
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion GlobalOrientation
    {
        get => Matrix4x4.Decompose(GlobalTransform, out _, out var value, out _) ? value : new();
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.Transform;

    public Object(Room parent, ObjectTemplate template)
    {
        Parent = parent;

        KitID = default!;
        TemplateID = default!;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(ObjectTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ID;
    }
}
