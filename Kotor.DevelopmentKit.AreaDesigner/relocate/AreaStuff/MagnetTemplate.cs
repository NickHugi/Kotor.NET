using System.Collections.Generic;
using System.Numerics;
using Avalonia.Markup.Xaml.Templates;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.AreaStuff;

public class MagnetTemplate
{
    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public required Vector3 LocalScale { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateScale(LocalScale) * Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public required string Tag { get; init; }

    public MagnetType MagnetType { get; init; }
}

public class TargetHookTemplate : MagnetTemplate
{
}

public class SourceHookTemplate : MagnetTemplate
{
    public required string KitID { get; init; }
    public required string C1TemplateID { get; init; }
    public required string C2TemplateID { get; init; }
    public required string C3TemplateID { get; init; }
    public required string C4TemplateID { get; init; }

    public IEnumerable<WorldObjectTemplate?> Templates =>
    [
        Kit.Manager.Get(KitID)?.Object(C1TemplateID),
        Kit.Manager.Get(KitID)?.Object(C2TemplateID),
        Kit.Manager.Get(KitID)?.Object(C3TemplateID),
        Kit.Manager.Get(KitID)?.Object(C4TemplateID),
    ];
}
