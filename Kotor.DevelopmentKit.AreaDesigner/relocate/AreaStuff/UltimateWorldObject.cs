using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Magnets;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class UltimateWorldObject
{
    public Room Room { get; }

    public Guid ID { get; }

    public string KitID { get; protected set; }
    public string TemplateID { get; protected set; }

    public WorldObjectType Type { get; protected set; }

    public string? GroupID { get; init; }

    public bool Visible { get; set; } = true;

    public UltimateWorldObjectAttachment Attachment;
    public IReadOnlyCollection<UltimateWorldObject> AttachedObjects { get; set; } //_attachedObjects.AsReadOnly();
    private List<UltimateWorldObject> _attachedObjects = [];

    public Vector3 LocalPosition
    {
        get;
        set;
    }
    public Quaternion LocalOrientation
    {
        get;
        set;
    } = Quaternion.Identity;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    private Vector3 ParentPosition
    {
        get => (Attachment is null) ? Room.Position : Attachment.Parent.GlobalPosition;
    }
    private Quaternion ParentOrientation
    {
        get => (Attachment is null) ? Room.Orientation : Attachment.Parent.GlobalOrientation;
    }
    private Matrix4x4 ParentTransform
    {
        get => (Attachment is null) ? Room.Transform : Attachment.Parent.GlobalTransform;
    }

    public Vector3 GlobalPosition
    {
        get => Vector3.Transform(LocalPosition, ParentOrientation) + ParentPosition;
        set => LocalPosition = Vector3.Transform(value - ParentPosition, Quaternion.Inverse(ParentOrientation));
    }
    public Quaternion GlobalOrientation
    {
        get => Quaternion.Normalize(LocalOrientation * ParentOrientation);
        set => LocalOrientation = value * Quaternion.Inverse(ParentOrientation);
    }
    public Matrix4x4 GlobalTransform => LocalTransform * ParentTransform;

    public UltimateWorldObject(Room room, UltimateWorldObjectTemplate template, Guid id)
    {
        Room = room;
        ID = id;
        SwitchTemplate(template);
    }

    public void SwitchTemplate(UltimateWorldObjectTemplate template)
    {
        KitID = template.KitID;
        TemplateID = template.TemplateID;
        // TODO
        //Type = template.Type;
    }
}

public class UltimateWorldObjectAttachment
{
    public required UltimateWorldObject Parent { get; init; }
    public required UltimateWorldObject Child { get; init; }
    public required UltimateMagnetTemplate ParentMagnet { get; init; }
}
