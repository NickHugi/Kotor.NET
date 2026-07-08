using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Wall : UltimateWorldObject
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
                    Type = MagnetType.Hook
                }];
            }
            else
            {
                return DoorFrame.Magnets;
            }
        }
    }

    public Room? LinkedRoom { get; set; }
    public Tile? LinkedTile { get; set; }
    public DoorFrame? DoorFrame { get; set; }
    public WallHookTemplate Hook { get; set; }

    public WallTemplate Template => Kit.Manager.Get(KitID).Wall(TemplateID);

    public bool Visible => LinkedTile is null;

    public Wall(Tile parent, WallTemplate template, WallHookTemplate hook) : base(parent.Parent, template, Guid.NewGuid())
    {
        Parent = parent;
        Hook = hook;
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
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
