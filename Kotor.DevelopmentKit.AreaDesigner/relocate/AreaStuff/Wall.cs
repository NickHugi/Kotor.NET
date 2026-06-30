using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Wall : IWorldObject
{
    public Tile Parent { get; }
    
    public IReadOnlyCollection<Magnet> Magnets
    {
        get
        {
            if (LinkedTile is not null)
            {
                return [];
            }
            else if (DoorFrame is null)
            {
                return [new Magnet(this)
                {
                    LocalPosition = new(),
                    LocalOrientation = Quaternion.Identity,
                    Type = MagnetType.Wall
                }];
            }
            else
            {
                return DoorFrame.Magnets;
            }
        }
    }
    public WorldObjectType Type => WorldObjectType.Prop;

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
        get => Quaternion.Normalize(Hook.LocalOrientation);
        set => throw new NotImplementedException(); // TODO
    }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateTranslation(LocalPosition) * Matrix4x4.CreateFromQuaternion(LocalOrientation);

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
    public Matrix4x4 GlobalTransform => Hook.LocalTransform * Parent.GlobalTransform;

    public bool Visible => LinkedTile is null;

    public Wall(Tile parent, WallTemplate template, WallHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }

    public void SwitchTemplate(WorldObjectTemplate template)
    {
        if (template is not WallTemplate wallTemplate)
            throw new ArgumentException();

        SwitchTemplate(wallTemplate);
    }
    public void SwitchTemplate(WallTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;

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
