using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Vector3 = System.Numerics.Vector3;

namespace Kotor.NET.Graphics.Cameras;

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

    public float FOV
    {
        get;
        set => Math.Max(0, value);
    } = (float)(Math.PI / 3f);

    public float Near
    {
        get;
        set;
    } = 0.001f;

    public float Far
    {
        get;
        set;
    } = 1000.0f;

    public Vector3 Position
    {
        get
        {
            float cosPitch = MathF.Cos(Pitch);
            float sinPitch = MathF.Sin(Pitch);
            float cosYaw = MathF.Cos(Yaw);
            float sinYaw = MathF.Sin(Yaw);

            float x = Distance * cosPitch * cosYaw;
            float y = Distance * cosPitch * sinYaw;
            float z = Distance * sinPitch;

            return Target + new Vector3(x, y, z);
        }
    }

    public override Matrix4x4 GetViewTransform()
    {
        return Matrix4x4.CreateLookAt(Position, Target, Vector3.UnitZ);
    }

    public override Matrix4x4 GetProjectionTransform(uint width, uint height)
    {
        return Matrix4x4.CreatePerspectiveFieldOfView(FOV, width / (float)height, 0.001f, 1000);
    }

    public Vector2 WorldToScreen(
        Vector3 position,
        int screenWidth,
        int screenHeight)
    {
        var view = GetViewTransform();
        var projection = GetProjectionTransform((uint)screenWidth, (uint)screenHeight);
        var clip = Vector4.Transform(new Vector4(position, 1f), view * projection);

        var ndc = new Vector3(clip.X / clip.W, clip.Y / clip.W, clip.Z / clip.W);

        return new Vector2(
            (ndc.X * 0.5f + 0.5f) * screenWidth,
            (-ndc.Y * 0.5f + 0.5f) * screenHeight
        );
    }

    public Vector2 ProjectToScreen(uint width, uint height, Vector3 worldPos)
    {
        Matrix4x4 view = GetViewTransform();
        Matrix4x4 proj = GetProjectionTransform(width, height);

        Vector4 clip = Vector4.Transform(
            Vector4.Transform(new Vector4(worldPos, 1), view),
            proj);

        clip /= clip.W;

        return new Vector2(
            (clip.X * 0.5f + 0.5f) * width,
            (1 - (clip.Y * 0.5f + 0.5f)) * height);
    }

    public override Ray ProjectRay(int mouseX, int mouseY, uint screenWidth, uint screenHeight)
    {
        var x = (2.0f * (float)mouseX) / (float)screenWidth - 1.0f;
        var y = 1.0f - (2.0f * (float)mouseY) / (float)screenHeight;
        Vector4 rayClip = new Vector4(x, y, -1.0f, 1.0f);
        Vector4 rayEye = Vector4.Transform(rayClip, Matrix4x4.Invert(GetProjectionTransform(screenWidth, screenHeight), out var value) ? value : Matrix4x4.Identity);
        rayEye = new Vector4(rayEye.X, rayEye.Y, -1.0f, 0.0f);

        Vector3 rayWorld = Vector3.Normalize(
            Vector3.TransformNormal(new Vector3(rayEye.X, rayEye.Y, rayEye.Z), Matrix4x4.Invert(GetViewTransform(), out var value2) ? value2 : Matrix4x4.Identity)
        );

        return new Ray(Position, rayWorld);
    }
}
