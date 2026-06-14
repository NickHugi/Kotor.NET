using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class InnerCornerHookTemplate
{
    public required string DefaultCornerID { get; init; }
    public InnerCornerTemplate DefaultTemplate => Kit.Manager.Get("sandral").InnerCorner(DefaultCornerID); // todo - remove hardcoding

    public required int[] Adjacent { get; init; }

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

}
