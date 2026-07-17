using System.Numerics;

namespace Kotor.NET.Graphics.Cameras;

public abstract class Camera
{
    public abstract Matrix4x4 GetViewTransform();

    public abstract Matrix4x4 GetProjectionTransform(uint width, uint height);

    public abstract Ray ProjectRay(int mouseX, int mouseY, uint screenWidth, uint screenHeight);

    public abstract Vector3 GetForward();

    public abstract void Move(Vector3 offset);

    public abstract void Zoom(float distance);

    public abstract void Rotate(float yaw, float pitch);
}
