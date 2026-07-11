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
    Ignore
}

public class Magnet
{
    public Area Area => Room.Area;
    public Room Room => Parent.Room;
    public WorldObject Child => Room.AllObjects.Single(x => x.ParentMagnet == this);
    public WorldObject Parent { get; }
    public MagnetTemplate Template { get; }
    public MagnetType Type { get; set; }

    #region Hide Settings
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
                WorldObjectType.DoorFrame => HideNumberCondition.Ignore,
                _ => HideNumberCondition.Ignore
            };
        }
    }

    public bool HideUseLocalMagnets => false;

    public bool HideOverlapping
    {
        get
        {
            return true;
        }
    }
    public bool HidePickFirstOverlap
    {
        get => Template.Template.Type == WorldObjectType.DoorFrame;
    }
    public bool HideMustHaveHooks
    {
        get => Template.Template.Type != WorldObjectType.DoorFrame;
    }

    public bool HideOnlySameTemplate
    {
        get
        {
            return false;
        }
    }
    public bool HideOnlySameClassID
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
            return Template.Template.Type != WorldObjectType.DoorFrame;
        }
    }
    public WorldObjectType?[]? HideSpecificTypes
    {
        get => Template?.Template?.Type switch
        {
            WorldObjectType.DoorFrame => [null],
            _ => null
        };
    }

    public bool HidePickClosestToCenter
    {
        get
        {
            return Template.Template.Type == WorldObjectType.OuterCorner;
        }
    }
    #endregion

    public bool Visible
    {
        get
        {
            if (!HideOverlapping)
                return true;

            var magnets = (HideUseLocalMagnets ? Room.AllMagnets : Area.AllMagnets).Where(x => x != this);

            magnets = magnets.Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.1f));

            if (HideMustHaveHooks)
            {
                magnets = magnets.Where(x => !string.IsNullOrWhiteSpace(x.Template.KitID) && !string.IsNullOrEmpty(x.Template.TemplateID));
            }

            if (HideOnlySameTemplate)
            {
                magnets = magnets.Where(x => x.Template?.Template == Template?.Template);
            }
            if (HideOnlySameType)
            {
                magnets = magnets.Where(x => x.Template?.Template?.Type == Template?.Template?.Type);
            }

            if (HidePickClosestToCenter && magnets.Count() == 2)
            {
                var middle = MiddleMostMagnet(this, magnets.ElementAt(0), magnets.ElementAt(1));
                return this == middle;
            }

            if (HideSpecificTypes is not null)
            {
                magnets = magnets.Where(x => (x.IsHook && HideSpecificTypes.Contains(x.Template.Template.Type)) || (!x.IsHook && HideSpecificTypes.Contains(null)));
            }

            var visible = HideNumberCondition switch
            {
                HideNumberCondition.EqualTo => magnets.Count() == HideNumber,
                HideNumberCondition.NotEqualTo => magnets.Count() != HideNumber,
                HideNumberCondition.LessThan => magnets.Count() < HideNumber,
                HideNumberCondition.GreaterThan => magnets.Count() > HideNumber,
                _ => true
            };

            if (HidePickFirstOverlap)
            {
                var lowestGuid = magnets.Min(x => x.Parent.ID);
                visible = visible && (lowestGuid == Child.ID);
            }

            return visible;
        }
        set;
    } = true;

    public bool IsHook => !string.IsNullOrWhiteSpace(Template.TemplateID) && !string.IsNullOrWhiteSpace(Template.KitID);
    public bool IsTileMagnet => (IsHook && Template.Template.Type == WorldObjectType.Wall) || Parent.Type == WorldObjectType.DoorFrame;
    public string WallClassID => (IsHook && Template.Template.Type == WorldObjectType.Wall)
        ? Template.Template.ClassID
        : Parent.TemplateID;

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

    public Magnet(WorldObject parent, MagnetTemplate template)
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
