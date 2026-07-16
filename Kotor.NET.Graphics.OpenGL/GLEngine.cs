using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Kotor.NET.Common.Data;
using Kotor.NET.Formats.BinaryTPC.Serialisation;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Graphics.Renderers;
using Kotor.NET.Graphics.Renderers.Descriptors;
using Kotor.NET.Tests.Encapsulation;
using Silk.NET.OpenGL;
using static System.Formats.Asn1.AsnWriter;

namespace Kotor.NET.Graphics.OpenGL;

public class GLEngine
{
    public required GL GL { get; init; }
    public required IScene Scene { get; init; }
    public required AssetManager AssetManager { get; init; }
    public Action<List<IDrawCallDescriptor>>? RenderInterceptor { get; set;  }

    public uint Width { get; set; }
    public uint Height { get; set; }
    public float RunningTime { get; private set; }

    public IEncapsulation Source { get; set; }

    private readonly Queue<Action> _glQueue = new();

    public void Init()
    {
        GL.Enable(EnableCap.DepthTest);

        AssetManager.AddShader("basic", new ShaderFactory(GL).FromFile("Assets/standard/vertex.glsl", "Assets/standard/fragment.glsl"));
        AssetManager.AddShader("line", new ShaderFactory(GL).FromFile("Assets/line/vertex.glsl", "Assets/line/fragment.glsl"));
        AssetManager.AddShader("billboard", new ShaderFactory(GL).FromFile("Assets/billboard/vertex.glsl", "Assets/billboard/fragment.glsl"));
        AssetManager.AddShader("picker", new ShaderFactory(GL).FromFile("Assets/picker/vertex.glsl", "Assets/picker/fragment.glsl"));
        AssetManager.AddShader("image", new ShaderFactory(GL).FromFile("Assets/image/vertex.glsl", "Assets/image/fragment.glsl"));

        var placeholderTexture = new TPCTextureFactory(GL).FromPlaceholder();
        AssetManager.AddTexture("placeholder", placeholderTexture);

        using var magnetTextureStream = File.OpenRead("Assets/Textures/magnet.tga");
        var magnetTPC = new TGABinaryDeserializer(new(magnetTextureStream)).Deserialize();
        var magnetTexture = new TPCTextureFactory(GL).FromTPC(magnetTPC);
        AssetManager.AddTexture("magnet", magnetTexture);

        AssetManager.Quad = new VertexArrayObjectFactory().NewQuad(GL);
        AssetManager.Billboard = new VertexArrayObjectFactory().NewBillBoard(GL);
        AssetManager.Line = new VertexArrayObjectFactory().GetLineQuad(GL);
    }

    public void Deinit()
    {
        AssetManager.Dispose();
    }

    public void Render()
    {
        while (_glQueue.Count > 0)
        {
            var action = _glQueue.Dequeue();
            action();
        }

        var viewport = new Vector2(Width, Height);
        GL.Viewport(0, 0, Width, Height);

        GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var descriptors = Scene.Render(AssetManager);

        new LineRenderer().Render(AssetManager, descriptors, Scene.ActiveCamera, viewport);
        new GeometryRenderer().Render(AssetManager, descriptors, Scene.ActiveCamera, viewport);
        new BillboardRenderer().Render(AssetManager, descriptors, Scene.ActiveCamera, viewport);
        new ImageRenderer().Render(AssetManager, descriptors, Scene.ActiveCamera, viewport);
    }

    public void Update(float timestep)
    {
        RunningTime += timestep;
        Scene.Update(AssetManager, timestep);
    }

    public async Task<int> Pick(int x, int y, Camera camera)
    {
        return await RunOnGLThread(() =>
        {
            GL.Viewport(0, 0, Width, Height);

            GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

            //new PickRenderer().Render(AssetManager, Scene, camera, Width, Height);

            Span<byte> bytes = new byte[4];
            GL.ReadPixels(x, (int)Height - y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
            var id = bytes[3] + (bytes[2] << 8) + (bytes[1] << 16) + (bytes[0] << 24);

            return id;
        });
    }

    public async Task LoadTexture(string name, byte[] data, ResourceType resourceType)
    {
        await RunOnGLThread(() =>
        {
            if (AssetManager.HasTexture(name))
                AssetManager.RemoveTexture(name);

            using var stream = new MemoryStream(data);

            var texture = resourceType switch
            {
                _ when resourceType == ResourceType.TGA => new TGATextureFactory(GL).FromStream(stream),
                _ when resourceType == ResourceType.TPC => new TPCTextureFactory(GL).FromStream(stream),
                _ => throw new NotImplementedException()
            };

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

            if (Source is null)
                return;

            var check = new List<BaseNode>() { model.Root };
            while (check.Any())
            {
                var node = check.First();
                check.RemoveAt(0);
                check.AddRange(node.Nodes);

                if (node is MeshNode mesh)
                {
                    var hasTexture1 = !string.IsNullOrEmpty(mesh.Texture1) && string.Equals(mesh.Texture1, "NULL", StringComparison.OrdinalIgnoreCase);
                    if (!hasTexture1 && !AssetManager.HasTexture(mesh.Texture1))
                    {
                        var textureName = mesh.Texture1;
                        var textureResource = Source.Find(mesh.Texture1, ResourceType.TPC);
                        var textureData = File.ReadAllBytes(textureResource.FilePath);
                        var resourceType = textureResource.Type;
                        await LoadTexture(textureName, textureData, resourceType);
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
                tcs.SetResult();
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

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

        return tcs.Task;
    }
}
