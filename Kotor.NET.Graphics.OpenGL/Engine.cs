using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Tests.Encapsulation;
using Silk.NET.OpenGL;

namespace Kotor.NET.Graphics.OpenGL;

public class Engine
{
    public required GL GL { get; init; }
    public required Scene Scene { get; init; }
    public required AssetManager AssetManager { get; init; }

    public uint Width { get; set; }
    public uint Height { get; set; }

    public IEncapsulation Source { get; set; }

    private readonly Queue<Action> _glQueue = new();

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

    public void Render(Camera camera)
    {
        while (_glQueue.Count > 0)
        {
            var action = _glQueue.Dequeue();
            action();
        }

        GL.Viewport(0, 0, Width, Height);

        GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        AssetManager.GetShader("basic").Activate();
        AssetManager.GetShader("basic").SetMatrix4x4("projection", camera.GetProjectionTransform(Width, Height));
        AssetManager.GetShader("basic").SetMatrix4x4("view", camera.GetViewTransform());
        AssetManager.GetShader("basic").SetMatrix4x4("mesh", Matrix4x4.Identity);
        AssetManager.GetShader("basic").SetUniform1("texture1", 0);

        Scene.Render(AssetManager);
    }

    public void Update(float timestep)
    {
        Scene.Update(AssetManager, timestep);
    }

    public async Task<Entity?> Pick(int x, int y, Camera camera)
    {
        return await RunOnGLThread(() =>
        {
            GL.Viewport(0, 0, Width, Height);

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, Width / (float)Height, 0.001f, 1000);
            var view = camera.GetViewTransform();
            AssetManager.GetShader("picker").Activate();
            AssetManager.GetShader("picker").SetMatrix4x4("projection", projection);
            AssetManager.GetShader("picker").SetMatrix4x4("view", view);
            AssetManager.GetShader("picker").SetMatrix4x4("mesh", Matrix4x4.Identity);

            Scene.PickRender(AssetManager);

            Span<byte> bytes = new byte[4];
            GL.ReadPixels(x, (int)Height - y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
            var id = bytes[3] + (bytes[2] << 8) + (bytes[1] << 16) + (bytes[0] << 24);

            return Scene.Entities.FirstOrDefault(x => x.ID == id);
        });
    }

    public async Task LoadTexture(string name, byte[] data)
    {
        await RunOnGLThread(() =>
        {
            if (AssetManager.HasTexture(name))
                AssetManager.RemoveTexture(name);

            using var stream = new MemoryStream(data);
            var texture = new TPCTextureFactory(GL).FromStream(stream);
            AssetManager.AddTexture(name, texture);
        });
    }

    public async Task LoadModel(string name, byte[] mdlData, byte[] mdxData)
    {
        await RunOnGLThread(async () =>
        {
            if (AssetManager.HasModel(name))
                AssetManager.RemoveModel(name);

            var model = new ModelLoader().LoadModel(GL, mdlData, mdxData);
            AssetManager.AddModel(name, model);

            var check = new List<BaseNode>() { model.Root };
            while (check.Any())
            {
                var node = check.First();
                check.RemoveAt(0);
                check.AddRange(node.Nodes);

                if (node is MeshNode mesh)
                {
                    var hasTexture1 = !string.IsNullOrEmpty(mesh.Texture1) && string.Equals(mesh.Texture1, "NULL", StringComparison.OrdinalIgnoreCase);
                    if (!hasTexture1)
                    {
                        var textureName = mesh.Texture1;
                        var textureResource = Source.Find(mesh.Texture1, ResourceType.TPC);
                        var textureData = File.ReadAllBytes(textureResource.FilePath);
                        await LoadTexture(textureName, textureData);
                    }
                }
            }
        });
    }

    public Task RunOnGLThread(Action action)
    {
        var tcs = new TaskCompletionSource();

        _glQueue.Enqueue(() =>
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        //RequestNextFrameRendering();
        return tcs.Task;
    }
    public Task<T> RunOnGLThread<T>(Func<T> action)
    {
        var tcs = new TaskCompletionSource<T>();

        _glQueue.Enqueue(() =>
        {
            try
            {
                var result = action();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        //RequestNextFrameRendering();
        return tcs.Task;
    }
}
