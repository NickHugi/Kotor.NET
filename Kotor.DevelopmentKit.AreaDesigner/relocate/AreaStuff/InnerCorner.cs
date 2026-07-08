using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.NET.Extensions;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class InnerCorner : UltimateWorldObject
{
    public Tile Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets => [];
    public WorldObjectType Type => WorldObjectType.InnerCorner;

    public CornerHookTemplate Hook { get; }

    public InnerCornerTemplate Template => Kit.Manager.Get(KitID).InnerCorner(TemplateID);

    public bool Visible
    {
        get
        {
            var count = Parent.Parent.Objects.OfType<Tile>().SelectMany(x => x.AttachedObjects).OfType<InnerCorner>().Count(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));
            return count == 1;
        }
    }
    
    public InnerCorner(Tile parent, Magnet parentMaget, InnerCornerTemplate template, CornerHookTemplate hook) : base(parent.Parent, parentMaget, template, Guid.NewGuid())
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
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
