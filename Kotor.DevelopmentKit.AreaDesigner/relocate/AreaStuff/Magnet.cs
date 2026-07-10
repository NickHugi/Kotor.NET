using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Extensions;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public enum HideNumberCondition
{
    NotEqualTo,
    EqualTo,
    LessThan,
    GreaterThan,
}

public class Magnet
{
    public Room Room => Parent.Room;
    public UltimateWorldObject Parent { get; }
    public UltimateMagnetTemplate Template { get; }
    public MagnetType Type { get; set; }


    public int HideNumber
    {
        get
        {
            return Template.Template.Type switch
            {
                WorldObjectType.Wall => 1,
                WorldObjectType.InnerCorner => 0,
                WorldObjectType.OuterCorner => -1,
                _ => 0
            };
        }
    }
    public HideNumberCondition HideNumberCondition
    {
        get
        {
            return Template.Template.Type switch
            {
                WorldObjectType.Wall => HideNumberCondition.LessThan,
                WorldObjectType.InnerCorner => HideNumberCondition.EqualTo,
                WorldObjectType.OuterCorner => HideNumberCondition.EqualTo,
                _ => HideNumberCondition.EqualTo
            };
        }
    }
    public bool HideOverlapping
    {
        get
        {
            return true;
        }
    }
    public bool HideOnlySameTemplate
    {
        get
        {
            return false;
        }
    }
    public bool HideOnlySameType
    {
        get
        {
            return true;
        }
    }
    public bool HidePickClosestToCenter
    {
        get
        {
            return Template.Template.Type == WorldObjectType.OuterCorner;
        }
    }

    public bool Visible
    {
        get
        {
            if (!HideOverlapping)
                return true;

            var magnets = Room.AllMagnets.Where(x => x != this && x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));

            if (HideOnlySameTemplate)
            {
                magnets = magnets.Where(x => x.Template.Template == Template.Template);
            }
            if (HideOnlySameType)
            {
                magnets = magnets.Where(x => x.Template.Template.Type == Template.Template.Type);
            }

            if (HidePickClosestToCenter && magnets.Count() == 2)
            {
                var middle = MiddleMostMagnet(this, magnets.ElementAt(0), magnets.ElementAt(1));
                return this == middle;
            }

            return HideNumberCondition switch
            {
                HideNumberCondition.EqualTo => magnets.Count() == HideNumber,
                HideNumberCondition.NotEqualTo => magnets.Count() != HideNumber,
                HideNumberCondition.LessThan => magnets.Count() < HideNumber,
                HideNumberCondition.GreaterThan => magnets.Count() > HideNumber,
                _ => true
            };
        }
        set;
    } = true;

    public Vector3 LocalPosition => Template.LocalPosition;
    public Quaternion LocalOrientation => Template.LocalOrientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(LocalPosition, Parent.GlobalOrientation) + Parent.GlobalPosition;
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * Parent.GlobalOrientation);
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public Magnet(UltimateWorldObject parent, UltimateMagnetTemplate template)
    {
        Parent = parent;
        Template = template;
    }

    private static Magnet MiddleMostMagnet(Magnet a, Magnet b, Magnet c)
    {
        var a2b = Vector3.Distance(a.Parent.GlobalPosition, b.Parent.GlobalPosition);
        var a2c = Vector3.Distance(a.Parent.GlobalPosition, c.Parent.GlobalPosition);
        var c2b = Vector3.Distance(c.Parent.GlobalPosition, b.Parent.GlobalPosition);

        if (a2b.Equals(a2c, 0.001f))
        {
            return a;
        }
        else if (a2b.Equals(c2b, 0.001f))
        {
            return b;
        }
        else if (a2c.Equals(c2b, 0.001f))
        {
            return c;
        }
        else
        {
            return null;
        }
    }
}
