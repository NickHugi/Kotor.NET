using System.Numerics;

namespace Kotor.NET.Graphics;

public abstract class Camera
{
    public abstract Matrix4x4 GetViewTransform();
}
