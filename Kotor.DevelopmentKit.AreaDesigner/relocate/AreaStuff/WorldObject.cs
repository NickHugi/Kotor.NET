using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

[DebuggerDisplay("globalXYZ: {ParentPosition} template: {Template}")]
public class WorldObject
{
    // TODO Doorframes (hooked)

    public Room Room { get; }
    public Magnet? ParentMagnet { get; }

    public Guid ID { get; }
    public WorldObjectType Type { get; protected set; }

    public string? GroupID { get; init; }

    public bool Visible
    {
        get => (ParentMagnet is null) ? field : ParentMagnet.Visible;
        set;
    } = true;

    public IReadOnlyCollection<Magnet> Magnets { get; private set; }
    public IReadOnlyCollection<WorldObject> AttachedObjects { get; set; } = [];

    public string KitID { get; protected set; }
    public string TemplateID { get; protected set; }
    public WorldObjectTemplate Template => Kit.Manager.Get(KitID).Object(TemplateID);

    public Vector3 LocalPosition
    {
        get => (ParentMagnet is null) ? field : ParentMagnet.LocalPosition;
        set;
    }
    public Quaternion LocalOrientation
    {
        get => (ParentMagnet is null) ? field : ParentMagnet.LocalOrientation;
        set;
    } = Quaternion.Identity;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 ParentPosition
    {
        get => (ParentMagnet is null) ? Room.Position : ParentMagnet.GlobalPosition;
    }
    public Quaternion ParentOrientation
    {
        get => (ParentMagnet is null) ? Room.Orientation : ParentMagnet.GlobalOrientation;
    }
    public Matrix4x4 ParentTransform
    {
        get => (ParentMagnet is null) ? Room.Transform : ParentMagnet.LocalTransform;
    }
        
    public Vector3 GlobalPosition
    {
        get => (ParentMagnet is null)
            ? Vector3.Transform(LocalPosition, Room.Orientation) + Room.Position
            : ParentMagnet.GlobalPosition;
        set => _ = (ParentMagnet is null)
            ? LocalPosition = Vector3.Transform(value - ParentPosition, Quaternion.Inverse(ParentOrientation))
            : Vector3.Zero;
    }
    public Quaternion GlobalOrientation
    {
        get => (ParentMagnet is null)
            ? Quaternion.Normalize(LocalOrientation * Room.Orientation)
            : ParentMagnet.GlobalOrientation;
        set => _ = (ParentMagnet is null)
            ? LocalOrientation = value * Quaternion.Inverse(Room.Orientation)
            : Quaternion.Identity;
    }
    public Matrix4x4 GlobalTransform => (ParentMagnet is null)
        ? LocalTransform * Room.Transform
        : ParentMagnet.GlobalTransform;

    public WorldObject(Room room, Magnet? parent, WorldObjectTemplate template, Guid id, WorldObjectType type)
    {
        Room = room;
        ParentMagnet = parent;
        ID = id;
        Type = type;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(WorldObjectTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        Type = template.Type;

        var magnets = Template.Magnets.ToList();
        if (!string.IsNullOrWhiteSpace(template.DoorframeTemplateID) && !string.IsNullOrWhiteSpace(template.DoorframeClassID))
        {
            magnets.Add(new MagnetTemplate
            {
                LocalPosition = Vector3.Zero,
                LocalOrientation = Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI),
                KitID = template.DoorframeKitID,
                TemplateID = template.DoorframeTemplateID,
            });
        }
        Magnets = magnets.Select(x => new Magnet(this, x)).ToArray();
        

        var attachedObjects = new List<WorldObject>();
        AttachedObjects = attachedObjects;
        attachedObjects.AddRange(
        [
            .. Magnets
                .Where(x => !string.IsNullOrWhiteSpace(x.MagnetTemplate.KitID) && !string.IsNullOrWhiteSpace(x.MagnetTemplate.TemplateID))
                .Where(x => x.MagnetTemplate is MagnetTemplate)
                .Select(x => new WorldObject(Room, x, x.MagnetTemplate.Template, Guid.NewGuid(), x.MagnetTemplate.Template.Type)),
        ]);
    }
}
