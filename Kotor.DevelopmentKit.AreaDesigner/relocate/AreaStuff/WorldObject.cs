using System;
using System.Collections.Generic;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class WorldObject : UltimateWorldObject
{
    public Room Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets => [];

    public UltimateWorldObjectTemplate Template => Kit.Manager.Get(KitID).Object(TemplateID);

    public WorldObject(Room parent, Magnet parentMagnet, UltimateWorldObjectTemplate template) : base(parent, parentMagnet, template, Guid.NewGuid())
    {
        Parent = parent;

        KitID = default!;
        TemplateID = default!;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
