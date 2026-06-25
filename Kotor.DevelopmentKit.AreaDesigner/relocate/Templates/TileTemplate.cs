using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class TileTemplate : ObjectTemplate
{
    public required FloorHookTemplate[] Floors { get; init; }
    public required CeilingHookTemplate[] Ceilings { get; init; }
    public required WallHookTemplate[] Walls { get; init; }
    public required InnerCornerHookTemplate[] InnerCorners { get; init; }
    public required OuterCornerHookTemplate[] OuterCorners { get; init; }
}

public class TileTemplateHook
{
    public required string DefaultTemplateID { get; init; }

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);
}
