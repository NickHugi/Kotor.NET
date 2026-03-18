using System.Numerics;

namespace Kotor.NET.Graphics.Cameras;

public abstract class Camera
{
    public abstract Matrix4x4 GetViewTransform();

    public abstract Matrix4x4 GetProjectionTransform(uint width, uint height);

    public abstract Ray ProjectRay(int mouseX, int mouseY, uint screenWidth, uint screenHeight);
}
