using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Graphics.OpenGL.Factories;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL;

public class OpenGLSceneWrapper
{
    public required GL GL { get; init; }
    public required Scene Scene { get; init; }
    public required AssetManager AssetManager { get; init; }

    public uint Width { get; set; }
    public uint Height { get; set; }

    public OrbitCamera Camera = new() { Distance = 1 }; // todo: move to scene

    public void Init()
    {
        GL.Enable(EnableCap.DepthTest);

        var shader1 = new ShaderFactory(GL).FromFile("Assets/standard/vertex.glsl", "Assets/standard/fragment.glsl");
        AssetManager.AddShader("basic", shader1);

        var shader2 = new ShaderFactory(GL).FromFile("Assets/picker/vertex.glsl", "Assets/picker/fragment.glsl");
        AssetManager.AddShader("picker", shader2);

        var placeholderTexture = new TPCTextureFactory(GL).FromPlaceholder();
        AssetManager.AddTexture("placeholder", placeholderTexture);
    }

    public void Deinit()
    {

    }

    public void Render(float delta)
    {
        GL.Viewport(0, 0, Width, Height);

        GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, Width / (float)Height, 0.001f, 1000);
        var view = Camera.GetViewTransform();
        AssetManager.GetShader("basic").Activate();
        AssetManager.GetShader("basic").SetMatrix4x4("projection", projection);
        AssetManager.GetShader("basic").SetMatrix4x4("view", view);
        AssetManager.GetShader("basic").SetMatrix4x4("mesh", Matrix4x4.Identity);
        AssetManager.GetShader("basic").SetUniform1("texture1", 0);

        Scene.Render(AssetManager);
    }

    public void Update(float timestep)
    {
        Scene.Update(AssetManager, timestep);
    }
}
