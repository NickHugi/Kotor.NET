using System;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;

namespace Kotor.DevelopmentKit.ViewerMDL;

public class OrbitCamera : Camera
{
    public Vector3 Target
    {
        get; set;
    }

    public float Yaw
    {
        get; set;
    }

    public float Pitch
    {
        get;
        set => field = Math.Clamp(value, -1.55f, 1.55f);
    }

    public float Distance
    {
        get;
        set => field = Math.Max(0, value);
    }

    public override Matrix4x4 GetViewTransform()
    {
        float cosPitch = MathF.Cos(Pitch);
        float sinPitch = MathF.Sin(Pitch);
        float cosYaw = MathF.Cos(Yaw);
        float sinYaw = MathF.Sin(Yaw);

        float x = Distance * cosPitch * cosYaw;
        float y = Distance * cosPitch * sinYaw;
        float z = Distance * sinPitch;

        Vector3 cameraPosition = Target + new Vector3(x, y, z);

        return Matrix4x4.CreateLookAt(cameraPosition, Target, Vector3.UnitZ);
    }
}
