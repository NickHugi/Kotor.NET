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

public class OuterCorner : IWorldObject
{
    public Tile Parent { get; }

    public IReadOnlyCollection<Magnet> Magnets => [];
    public WorldObjectType Type => WorldObjectType.OuterCorner;

    public CornerHookTemplate Hook { get; }

    public string KitID { get; private set; } = "";
    public string TemplateID { get; private set; } = "";
    public OuterCornerTemplate Template => Kit.Manager.Get(KitID).OuterCorner(TemplateID);

    public string? GroupID { get; set; }

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
    public Matrix4x4 LocalTransform => Hook.LocalTransform;

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
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public bool Visible
    {
        get
        {
            var at = Parent.Parent.Objects.OfType<Tile>().SelectMany(x => x.VirtualObjects).OfType<OuterCorner>().Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));
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

    public OuterCorner(Tile parent, OuterCornerTemplate template, CornerHookTemplate hook)
    {
        Parent = parent;
        Hook = hook;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(WorldObjectTemplate template)
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
