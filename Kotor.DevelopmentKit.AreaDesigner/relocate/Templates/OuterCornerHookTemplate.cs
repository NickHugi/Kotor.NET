using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class OuterCornerHookTemplate
{
    public required string DefaultTemplateID { get; init; }
    public OuterCornerTemplate DefaultTemplate => Kit.Manager.Get("sandral").OuterCorner(DefaultTemplateID); // todo - remove hardcoding

    public required int[] Adjacent { get; init; }

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

}
