using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrame : IWorldObject
{
    public Wall Parent { get; }

    public List<Magnet> Magnets => new();
    public WorldObjectType Type => WorldObjectType.Prop;

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public DoorFrameTemplate Template => Kit.Manager.Get(KitID).DoorFrame(TemplateID);

    public string? GroupID { get; set; }

    public bool Enabled { get; set; } = true;

    public IEnumerable<DoorFrameHook> Hooks => Template.Hooks.Select(x => new DoorFrameHook(this, x));

    public Vector3 LocalPosition
    {
        get => Template.Hooks.Last().Position;
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion LocalOrientation
    {
        get => Template.Hooks.Last().Orientation;
        set => throw new NotImplementedException(); // TODO
    }
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
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public bool Visible => Enabled;

    public DoorFrame(Wall parent, DoorFrameTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(DoorFrameTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.ObjectID;
    }
}
