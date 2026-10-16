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
    public SourceHookTemplate? SourceHook => MagnetTemplate as SourceHookTemplate;
    public MagnetType Type { get; set; }

    public bool Dirty { get; set; } = true;

    public bool Visible
    {
        get
        {
            if (Dirty)
            {
                if (WorldObjectTemplate is null)
                {
                    field = true;
                    Dirty = false;
                    return field;
                }

                var magnets = Area.AllMagnets.AsEnumerable();
                magnets = magnets.Where(x => x != this);

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

        }
    } 

    public bool IsHook => MagnetTemplate is TargetHookTemplate or SourceHookTemplate;
    public bool IsTileMagnet => true;//TODO SourceHook?.Template?.Type == WorldObjectType.Wall || Parent.Type == WorldObjectType.DoorFrame;
    //public string WallClassID => SourceHook?.Template?.Type == WorldObjectType.Wall
    //    ? SourceHook.Template.ClassID
    //    : Parent.TemplateID;

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
}
