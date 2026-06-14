using System.Numerics;

namespace Kotor.DevelopmentKit.AreaDesigner.relocate.Templates;

public class WallHookTemplate
{
    public required string DefaultWallID { get; init; }
    public WallTemplate DefaultTemplate => Kit.Manager.Get("sandral").Wall(DefaultWallID); // todo - remove hardcoding

    public required Vector3 LocalPosition { get; init; }
    public required Quaternion LocalOrientation { get; init; }
    public Matrix4x4 LocalTransform => Matrix4x4.CreateFromQuaternion(LocalOrientation) * Matrix4x4.CreateTranslation(LocalPosition);

    public int[] AdjacentWalls { get; init; } = [];

    //public ICollection<string> CompatibleWallTemplates { get; }
    //public ICollection<string> CompatibleTileTemplates { get; }
}
