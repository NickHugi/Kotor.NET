using System.Numerics;

namespace Kotor.DevelopmentKit.ViewerMDL;

public abstract class Camera
{
    public abstract Matrix4x4 GetViewTransform();
}
