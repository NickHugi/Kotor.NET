using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class InnerCorner : IWorldObject
{
    public Tile Parent { get; }

    public List<Magnet> Magnets => new();
    public WorldObjectType Type => WorldObjectType.InnerCorner;

    public InnerCornerHookTemplate Hook { get; }

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
    public Matrix4x4 GlobalTransform => Hook.LocalTransform * Parent.GlobalTransform;

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
