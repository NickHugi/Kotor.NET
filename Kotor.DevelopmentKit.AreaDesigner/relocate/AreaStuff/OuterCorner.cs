using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Hooks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;
using Kotor.NET.Extensions;
using Kotor.NET.Graphics.Extensions;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class OuterCorner : UltimateWorldObject
{
    public Tile Parent { get; }

    public CornerHookTemplate Hook { get; }

    public OuterCornerTemplate Template => Kit.Manager.Get(KitID).OuterCorner(TemplateID);

    public bool Visible
    {
        get
        {
            var at = Parent.Parent.Objects.OfType<Tile>().SelectMany(x => x.AttachedObjects).OfType<OuterCorner>().Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));
            var count = at.Count();

            if (count == 3)
            {
                var a = at.ElementAt(0).Parent;
                var b = at.ElementAt(1).Parent;
                var c = at.ElementAt(2).Parent;
                var a2b = Vector3.Distance(a.GlobalPosition, b.GlobalPosition);
                var a2c = Vector3.Distance(a.GlobalPosition, c.GlobalPosition);
                var c2b = Vector3.Distance(c.GlobalPosition, b.GlobalPosition);

                if (a2b.Equals(a2c, 0.001f))
                {
                    return this.Parent == a;
                }
                else if (a2b.Equals(c2b, 0.001f))
                {
                    return this.Parent == b;
                }
                else if (a2c.Equals(c2b, 0.001f))
                {
                    return this.Parent == c;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public OuterCorner(Tile parent, OuterCornerTemplate template, CornerHookTemplate hook) : base(parent.Parent, template, Guid.NewGuid())
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        if (template is not OuterCornerTemplate outerCornerTemplate)
            throw new ArgumentException();

        SwitchTemplate(outerCornerTemplate);
    }
    public void SwitchTemplate(OuterCornerTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
    }
}
