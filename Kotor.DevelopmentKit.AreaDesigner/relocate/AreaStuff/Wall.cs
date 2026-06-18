using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Wall : IWorldObject
{
    public Tile Parent { get; }
    
    public IReadOnlyCollection<Magnet> Magnets
    {
        get =>
        [
            new Magnet(this)
            {
                LocalPosition = new(),
                LocalOrientation = new(),
                Type = MagnetType.Wall
            }
        ];
    }
    public WorldObjectType Type => WorldObjectType.Basic;

    public string? GroupID { get; set; }

    public Room? LinkedRoom { get; set; }
    public Tile? LinkedTile { get; set; }
    public DoorFrame? DoorFrame { get; set; }
    public WallHookTemplate Hook { get; set; }

    public string KitID { get; private set;}
    public string TemplateID { get; private set; }
    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);

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

    public bool Visible => LinkedTile is null;

    public Wall(Tile parent, WallTemplate template, WallHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        KitID = template.KitID;
        TemplateID = template.ObjectID;
    }

    public void SwitchTemplate(ObjectTemplate template)
    {
        if (template is not WallTemplate wallTemplate)
            throw new ArgumentException();

        SwitchTemplate(wallTemplate);
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
