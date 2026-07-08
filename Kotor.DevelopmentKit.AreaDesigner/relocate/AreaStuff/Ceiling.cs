using System;
using System.Collections.Generic;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class Ceiling : UltimateWorldObject
{
    public Tile Parent { get; }

    public CeilingTemplate Template => Kit.Manager.Get(KitID).Ceiling(TemplateID);

    public Ceiling(Tile parent, CeilingTemplate template) : base(parent.Parent, template, Guid.NewGuid())
    {
        Parent = parent;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
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
