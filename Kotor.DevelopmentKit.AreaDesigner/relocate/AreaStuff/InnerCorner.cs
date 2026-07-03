using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.NET.Extensions;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class InnerCorner : IWorldObject
{
    public Tile Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets => [];
    public WorldObjectType Type => WorldObjectType.InnerCorner;

    public CornerHookTemplate Hook { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public InnerCornerTemplate Template => Kit.Manager.Get(KitID).InnerCorner(TemplateID);

    public string? GroupID { get; set; }

    public Vector3 LocalPosition
    {
        get => Hook.LocalPosition;
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion LocalOrientation
    {
        get => Hook.LocalOrientation;
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 LocalTransform => Hook.LocalTransform;

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(LocalPosition, Parent.GlobalOrientation) + Parent.GlobalPosition;
        set => throw new NotImplementedException(); // TODO
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * Parent.GlobalOrientation);
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public bool Visible
    {
        get
        {
            var count = Parent.Parent.Objects.OfType<Tile>().SelectMany(x => x.VirtualObjects).OfType<InnerCorner>().Count(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));
            return count == 1;
        }
    }
    
    public InnerCorner(Tile parent, InnerCornerTemplate template, CornerHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(WorldObjectTemplate template)
    {
        if (template is not InnerCornerTemplate innerCornerTemplate)
            throw new ArgumentException();

        SwitchTemplate(innerCornerTemplate);
    }
    public void SwitchTemplate(InnerCornerTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
