using System.Numerics;
using Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class DoorFrameHook
{
    public DoorFrame Parent { get; }
    public DoorFrameHookTemplate Template { get; }

    public Vector3 LocalPosition => Template.Position;
    public Quaternion LocalOrientation => Template.Orientation;
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public Vector3 Position => Matrix4x4.Decompose(Transform, out _, out _, out var value) ? value : new();
    public Quaternion Orientation => Matrix4x4.Decompose(Transform, out _, out var value, out _) ? value : new();
    public Matrix4x4 Transform => LocalTransform * Parent.GlobalTransform;

    public DoorFrameHook(DoorFrame parent, DoorFrameHookTemplate template)
    {
        Parent = parent;
        Template = template;
    }
}
