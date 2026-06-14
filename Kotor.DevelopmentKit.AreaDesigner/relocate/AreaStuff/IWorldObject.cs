using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public interface IWorldObject
{
    public Room Parent { get; }

    public ObjectTemplate Template { get; }

    public string? GroupID { get; set; }

    public Vector3 LocalPosition { get; set; }
    public Quaternion LocalOrientation { get; set; }
    public Matrix4x4 LocalTransform { get; }

    public Vector3 GlobalPosition { get; set; }
    public Quaternion GlobalOrientation { get; set; }
    public Matrix4x4 GlobalTransform { get; }

    public void SwitchTemplate(ObjectTemplate template);
}
