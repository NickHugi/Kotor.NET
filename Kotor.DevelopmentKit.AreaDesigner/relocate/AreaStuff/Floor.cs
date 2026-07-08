using System;
using System.Collections.Generic;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Floor : UltimateWorldObject
{
    public Tile Parent { get; }

    public FloorTemplate Template => Kit.Manager.Get(KitID).Floor(TemplateID);

    public Floor(Tile parent, FloorTemplate template) : base(parent.Parent, template, Guid.NewGuid())
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        if (template is not FloorTemplate floorTemplate)
            throw new ArgumentException();

        SwitchTemplate(floorTemplate);
    }
    public void SwitchTemplate(FloorTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
