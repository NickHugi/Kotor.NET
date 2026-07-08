using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrame : UltimateWorldObject
{
    public Wall Parent { get; }
    public Wall? AdjacentWall
    {
        get
        {
            var area = Parent.Parent.Parent.Parent;
            var walls = area.Rooms
                .SelectMany(x => x.Objects.OfType<Tile>())
                .SelectMany(x => x.AttachedObjects.OfType<Wall>())
                .ToList();

            return walls.FirstOrDefault(x => x != Parent && AllMagnets.Any(y => y.GlobalPosition == x.GlobalPosition));
        }
    }

    private IReadOnlyCollection<Magnet> AllMagnets
    {
        get
        {
            return Template.Magnets.Select(x => new Magnet(this)
            {
                Type = MagnetType.Hook,
                LocalPosition = x.LocalPosition,
                LocalOrientation = x.LocalOrientation,
            }).ToList();
        }
    }
    public IReadOnlyCollection<Magnet> Magnets
    {
        get => AllMagnets.Where(x => x.GlobalPosition != Parent.GlobalPosition).ToList();
    }

    public DoorFrameTemplate Template => Kit.Manager.Get(KitID).DoorFrame(TemplateID);

    public bool Enabled { get; set; } = true;

    public IEnumerable<DoorFrameHook> Hooks => Template.Magnets.OfType<DoorFrameHookTemplate>().Select(x => new DoorFrameHook(this, x));

    public bool Visible => Enabled;

    public DoorFrame(Wall parent, DoorFrameTemplate template) : base(parent.Parent.Parent, template, Guid.NewGuid())
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
