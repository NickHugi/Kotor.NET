using System;
using System.Collections.Generic;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Ceiling : IWorldObject
{
    public Tile Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets => [];
    public WorldObjectType Type => WorldObjectType.Ceiling;

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public CeilingTemplate Template => Kit.Manager.Get(KitID).Ceiling(TemplateID);

    public string? GroupID { get; set; }

    public Vector3 LocalPosition
    {
        get => throw new NotImplementedException(); // TODO
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion LocalOrientation
    {
        get => throw new NotImplementedException(); // TODO
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 LocalTransform => throw new NotImplementedException(); // TODO

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
    public Matrix4x4 GlobalTransform => Parent.GlobalTransform;

    public Ceiling(Tile parent, CeilingTemplate template)
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(WorldObjectTemplate template)
    {
        if (template is not CeilingTemplate ceilingTemplate)
            throw new ArgumentException();

        SwitchTemplate(ceilingTemplate);
    }
    public void SwitchTemplate(CeilingTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
