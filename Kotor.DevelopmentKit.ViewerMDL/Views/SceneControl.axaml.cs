using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Silk.NET.Core.Contexts;
using Silk.NET.OpenGL;
using Vector3 = System.Numerics.Vector3;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class SceneControl : OpenGlControlBase
{
    public MDLResourceViewerViewModel ViewModel => (MDLResourceViewerViewModel)DataContext;

    public SceneControl()
    {
        InitializeComponent();
    }

    protected unsafe override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        ViewModel.Context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        ViewModel.GL = new GL(ViewModel.Context);
        ViewModel.GL.Enable(EnableCap.DepthTest);
        ViewModel.AssetManager = new AssetManager();

        var shader = new ShaderFactory(ViewModel.GL).FromFile("Assets/vertex.glsl", "Assets/fragment.glsl");
        ViewModel.AssetManager.AddShader("basic", shader);

        var placeholderTexture = new TPCTextureFactory(ViewModel.GL).FromPlaceholder();
        ViewModel.AssetManager.AddTexture("placeholder", placeholderTexture);

        ViewModel.Scene = new();
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);
    }

    private DateTime _lastRender = DateTime.Now;
    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var width = (uint)(Bounds.Width * scale);
        var height = (uint)(Bounds.Height * scale);
        ViewModel.GL.Viewport(0, 0, width, height);

        ViewModel.GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        ViewModel.GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI/3f, width / (float)height, 0.001f, 1000);
        var view = Matrix4x4.CreateLookAt(new(2, 2, 1), new(0, 0, 1f), new(0, 0, 1));
        ViewModel.AssetManager.GetShader("basic").Activate();
        ViewModel.AssetManager.GetShader("basic").SetMatrix4x4("projection", projection);
        ViewModel.AssetManager.GetShader("basic").SetMatrix4x4("view", view);
        ViewModel.AssetManager.GetShader("basic").SetMatrix4x4("mesh", Matrix4x4.Identity);
        ViewModel.AssetManager.GetShader("basic").SetUniform1("texture1", 0);

        while (ViewModel.ModelBuffer.Count > 0)
        {
            var name = ViewModel.ModelBuffer.Keys.First();
            var data = ViewModel.ModelBuffer.TryRemove(name, out var value) ? value() : (null, null);

            var model = new ModelLoader().LoadModel(ViewModel.GL, data.MDL, data.MDX);
            ViewModel.Model = model;
            ViewModel.AssetManager.AddModel(name, model);

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
                        ViewModel.TextureRequests.Enqueue(mesh.Texture1);
                    }
                }
            }
        }

        while (ViewModel.TextureRequests.Count > 0)
        {
            var name = ViewModel.TextureRequests.TryDequeue(out var value) ? value : null;
            if (!ViewModel.AssetManager.HasTexture(name))
            {
                var data = ViewModel.Source.Find(name, ResourceType.TPC)
                    ?? ViewModel.Source.Find(name, ResourceType.TGA);
                using var stream = data.OpenStream();
                var texture = new TPCTextureFactory(ViewModel.GL).FromStream(stream);
                ViewModel.AssetManager.AddTexture(name, texture);
                ViewModel.TextureSource.Edit(updater =>
                {
                    updater.AddOrUpdate(data.FilePath, name);
                });
            }
        }

        var delta = (float)(DateTime.Now - _lastRender).Milliseconds / 1000;

        ViewModel.Scene.Update(ViewModel.AssetManager, delta);
        ViewModel.Scene.Render(ViewModel.AssetManager);

        _lastRender = DateTime.Now;

        RequestNextFrameRendering();
    }
}

public class AvaloniaSilkNativeContext : INativeContext
{
    private readonly Func<string, IntPtr> _getProcAddress;

    public AvaloniaSilkNativeContext(Func<string, IntPtr> getProcAddress)
    {
        _getProcAddress = getProcAddress;
    }

    public void Dispose()
    {

    }

    public nint GetProcAddress(string proc, int? slot = null)
    {
        return _getProcAddress(proc);
    }

    public bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
    {
        addr = _getProcAddress(proc);
        return true;
    }

    public static Matrix4x4 CreateLookAtZUp(
        Vector3 eye,
        Vector3 target)
    {
        // Forward (camera looks toward target)
        Vector3 forward = Vector3.Normalize(eye - target);

        // Z is up
        Vector3 up = Vector3.UnitZ;

        // Right = Up × Forward
        Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));

        // Recompute orthogonal up
        Vector3 trueUp = Vector3.Cross(forward, right);

        return new Matrix4x4(
            right.X, trueUp.X, forward.X, 0f,
            right.Y, trueUp.Y, forward.Y, 0f,
            right.Z, trueUp.Z, forward.Z, 0f,
            -Vector3.Dot(right, eye),
            -Vector3.Dot(trueUp, eye),
            -Vector3.Dot(forward, eye),
            1f
        );
    }

    //public IntPtr GetProcAddress(string procName)
    //    => _getProcAddress(procName);

    //public bool TryGetProcAddress(string proc, out nint address)
    //{
    //    address = _getProcAddress(proc);
    //    return address != IntPtr.Zero;
    //}
}
