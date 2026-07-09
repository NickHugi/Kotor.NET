using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrame : UltimateWorldObject
{
    public UltimateWorldObject Parent { get; }
    public UltimateWorldObject? AdjacentWall
    {
        get
        {
            var area = Room.Parent;
            var walls = area.Rooms
                .SelectMany(x => x.Objects.OfType<Tile>())
                .SelectMany(x => x.AttachedObjects.Where(x => x.Template.Type == WorldObjectType.Wall))
                .ToList();

            return walls.FirstOrDefault(x => x != Parent && Magnets.Any(y => y.GlobalPosition == x.GlobalPosition));
        }
    }

    public IReadOnlyCollection<Magnet> Magnets
    {
        get => base.Magnets.Where(x => x.GlobalPosition != Parent.GlobalPosition).ToList();
    }

    public DoorFrameTemplate Template => Kit.Manager.Get(KitID).DoorFrame(TemplateID);

    public bool Enabled { get; set; } = true;

    public IEnumerable<DoorFrameHook> Hooks => Template.Magnets.OfType<DoorFrameHookTemplate>().Select(x => new DoorFrameHook(this, x));

    public bool Visible => Enabled;

    public DoorFrame(UltimateWorldObject parent, Magnet parentMagnet, DoorFrameTemplate template) : base(parent.Room, parentMagnet, template, Guid.NewGuid())
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        if (template is not DoorFrameTemplate doorframeTemplate)
            throw new ArgumentException();

        SwitchTemplate(doorframeTemplate);
    }
    public void SwitchTemplate(DoorFrameTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
