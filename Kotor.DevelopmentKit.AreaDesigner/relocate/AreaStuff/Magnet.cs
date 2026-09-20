using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml.Templates;
using Kotor.NET.Extensions;
using Kotor.NET.Graphics.Extensions;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

[DebuggerDisplay("localXYZ: {LocalPosition} child: {Child}")]
public class Magnet
{
    public Area Area => Room.Area;
    public Room Room => Parent.Room;
    public WorldObject Child => Room.AllObjects.SingleOrDefault(x => x.ParentMagnet == this);
    public WorldObject Parent { get; }
    public MagnetTemplate MagnetTemplate { get; }
    public WorldObjectTemplate? WorldObjectTemplate => Child?.Template;
    public MagnetType Type { get; set; }

    public bool Dirty { get; set; } = true;

    public bool Visible
    {
        get
        {
            if (Dirty)
            {
                var magnets = Area.AllMagnets.AsEnumerable();
                magnets = magnets.Where(x => x != this);

                //if (MagnetTemplate.MagnetType == MagnetType.WallCentre)
                if (WorldObjectTemplate.TemplateID.Contains("_wall_"))
                {
                    magnets = magnets.Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.01f));
                    field = magnets.Count() == 0;
                }
                else if (WorldObjectTemplate.TemplateID.Contains("_walledge"))
                {
                    magnets = magnets
                        .Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 2))
                        .Where(x => x.WorldObjectTemplate.Model.Contains("_walledge"));
                    field = magnets.Count() < 4;
                }
                else if (WorldObjectTemplate.TemplateID.Contains("_wallcorner"))
                {
                    magnets = magnets
                        .Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 2))
                        .Where(x => x.WorldObjectTemplate.Model.Contains("_walledge"));
                    field = magnets.Count() >= 4;
                }
                else if (WorldObjectTemplate.TemplateID.Contains("_corner"))
                {
                    magnets = magnets.Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, 0.01f));
                    field = magnets.Count() == 0;
                }

                else if (WorldObjectTemplate.TemplateID.Contains("_floor_"))
                {
                    field = true;
                }
                else if (WorldObjectTemplate.TemplateID.Contains("_ceiling_"))
                {
                    field = true;
                }

                Dirty = false;
            }

            return field;

                
            //if (!MagnetTemplate.ConditionOverlapWillDisable)
            //    return true;

            //var magnets = (MagnetTemplate.ConditionCheckLocalMagnetsOnly ? Room.AllMagnets : Area.AllMagnets).AsEnumerable();

            //magnets = magnets.Where(x => x.GlobalPosition.ApproximatelyEquals(GlobalPosition, MagnetTemplate.ConditionOverlapDistance));
            //magnets = magnets.Where(x => x != this);

            //if (MagnetTemplate.ConditionMustHaveTemplate)
            //{
            //    magnets = magnets.Where(x => !string.IsNullOrWhiteSpace(x.MagnetTemplate.KitID) && !string.IsNullOrEmpty(x.MagnetTemplate.TemplateID));
            //}

            //if (MagnetTemplate.ConditionOverlapOnlySameClass)
            //{
            //    magnets = magnets.Where(x => x.WorldObjectTemplate?.ClassID == WorldObjectTemplate?.ClassID);
            //}
            //if (MagnetTemplate.ConditionOverlapOnlySameTemplate)
            //{
            //    magnets = magnets.Where(x => x.MagnetTemplate?.Template == MagnetTemplate?.Template);
            //}
            //if (MagnetTemplate.ConditionOverlapOnlySameType)
            //{
            //    magnets = magnets.Where(x => x.MagnetTemplate?.Template?.Type == MagnetTemplate?.Template?.Type);
            //}

            //if (MagnetTemplate.ConditionOverlapOnlySameRotation)
            //{
            //    magnets = magnets.Where(x => x.GlobalOrientation.ApproximatelyEquals(GlobalOrientation));
            //}

            //if (MagnetTemplate.ConditionOverlapOnlyEnableMiddle && magnets.Count() == 2)
            //{
            //    var middle = MiddleMostMagnet(this, magnets.ElementAt(0), magnets.ElementAt(1));
            //    return this == middle;
            //}

            //if (MagnetTemplate.ConditionOverlapOnlySpecificTypes is not null)
            //{
            //    magnets = magnets.Where(x => (x.IsHook && MagnetTemplate.ConditionOverlapOnlySpecificTypes.Contains(x.MagnetTemplate.Template.Type)) || (!x.IsHook && MagnetTemplate.ConditionOverlapOnlySpecificTypes.Contains(null)));
            //}

            //if (WorldObjectTemplate?.Type == WorldObjectType.DoorFrame)
            //{
            //    magnets = magnets.Where(x => x.Parent?.Type == WorldObjectType.DoorFrame).Select(x => x.Parent.ParentMagnet);
            //}

            //var visible = MagnetTemplate.ConditionOverlapType switch
            //{
            //    OverlapCountType.EqualTo => magnets.Count() == MagnetTemplate.ConditionOverlapCheckCount,
            //    OverlapCountType.NotEqualTo => magnets.Count() != MagnetTemplate.ConditionOverlapCheckCount,
            //    OverlapCountType.LessThan => magnets.Count() < MagnetTemplate.ConditionOverlapCheckCount,
            //    OverlapCountType.GreaterThan => magnets.Count() > MagnetTemplate.ConditionOverlapCheckCount,
            //    _ => true
            //};

            //if (MagnetTemplate.ConditionOverlapOnlyEnableFirst && visible)
            //{
            //    var check = magnets.Append(this).Where(x => x.Parent.Visible).ToList();
            //    var lowestGuid = check.DefaultIfEmpty().Min(x => x?.Child?.ID);
            //    visible = visible && (lowestGuid == Child.ID);
            //}

            //return visible;
        }
    } 

    public bool IsHook => !string.IsNullOrWhiteSpace(MagnetTemplate.TemplateID) && !string.IsNullOrWhiteSpace(MagnetTemplate.KitID);
    public bool IsTileMagnet => (IsHook && MagnetTemplate.Template?.Type == WorldObjectType.Wall) || Parent.Type == WorldObjectType.DoorFrame;
    public string WallClassID => (IsHook && MagnetTemplate.Template.Type == WorldObjectType.Wall)
        ? MagnetTemplate.Template.ClassID
        : Parent.TemplateID;

    public Vector3 LocalPosition => MagnetTemplate.LocalPosition;
    public Quaternion LocalOrientation => MagnetTemplate.LocalOrientation;
    public Vector3 LocalScale => MagnetTemplate.LocalScale;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateScale(LocalScale) * Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(Vector3.Zero, GlobalTransform);
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * Parent.GlobalOrientation);
    }
    public Vector3 GlobalScale
    {
        get => Vector3.Transform(Vector3.Zero, GlobalTransform);
    }
    public Matrix4x4 GlobalTransform => LocalTransform * Parent.GlobalTransform;

    public Magnet(WorldObject parent, MagnetTemplate template)
    {
        Parent = parent;
        MagnetTemplate = template;
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
