using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Rendering;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class SceneControl : OpenGlControlBase, ICustomHitTest
{
    public MDLResourceViewerViewModel ViewModel => (MDLResourceViewerViewModel)DataContext;

    private Point? _lastPointerPosition;
    private DateTime _lastRender = DateTime.Now;
    private OrbitCamera _camera = new();

    public SceneControl()
    {
        InitializeComponent();
    }

    private Entity? Pick(int x, int y)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var width = (uint)(Bounds.Width * scale);
        var height = (uint)(Bounds.Height * scale);
        ViewModel.GL.Viewport(0, 0, width, height);

        ViewModel.GL.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);
        ViewModel.GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, width / (float)height, 0.001f, 1000);
        var view = _camera.GetViewTransform();
        ViewModel.AssetManager.GetShader("picker").Activate();
        ViewModel.AssetManager.GetShader("picker").SetMatrix4x4("projection", projection);
        ViewModel.AssetManager.GetShader("picker").SetMatrix4x4("view", view);
        ViewModel.AssetManager.GetShader("picker").SetMatrix4x4("mesh", Matrix4x4.Identity);

        ViewModel.Scene.PickRender(ViewModel.AssetManager);

        Span<byte> bytes = new byte[4];
        ViewModel.GL.ReadPixels(x, (int)height-y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
        var id = bytes[3] + (bytes[2] << 8) + (bytes[1] << 16) + (bytes[0] << 24);

        return ViewModel.Scene.Entities.FirstOrDefault(x => x.ID == id);
    }

    bool ICustomHitTest.HitTest(Point point)
    {
        return this.Bounds.Contains(point);
    }

    private readonly Queue<Action> _glQueue = new();
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

        RequestNextFrameRendering();
        return tcs.Task;
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

        RequestNextFrameRendering();
        return tcs.Task;
    }

    # region OpenGlControlBase
    protected override void OnOpenGlInit(GlInterface gl)
    {
        ViewModel.LoadTexture.RegisterHandler(async interaction =>
        {
            await RunOnGLThread(() =>
            {
                var name = interaction.Input.Name;
                var data = interaction.Input.Data;

                if (ViewModel.AssetManager.HasTexture(name))
                    ViewModel.AssetManager.RemoveTexture(name);

                using var stream = new MemoryStream(data);
                var texture = new TPCTextureFactory(ViewModel.GL).FromStream(stream);
                ViewModel.AssetManager.AddTexture(name, texture);
            });

            interaction.SetOutput(Unit.Default);
        });

        base.OnOpenGlInit(gl);

        ViewModel.Context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        ViewModel.GL = new GL(ViewModel.Context);
        ViewModel.GL.Enable(EnableCap.DepthTest);
        ViewModel.AssetManager = new AssetManager();

        var shader1 = new ShaderFactory(ViewModel.GL).FromFile("Assets/standard/vertex.glsl", "Assets/standard/fragment.glsl");
        ViewModel.AssetManager.AddShader("basic", shader1);

        var shader2 = new ShaderFactory(ViewModel.GL).FromFile("Assets/picker/vertex.glsl", "Assets/picker/fragment.glsl");
        ViewModel.AssetManager.AddShader("picker", shader2);

        var placeholderTexture = new TPCTextureFactory(ViewModel.GL).FromPlaceholder();
        ViewModel.AssetManager.AddTexture("placeholder", placeholderTexture);

        ViewModel.Scene = new();
    }

    protected async override void OnOpenGlRender(GlInterface gl, int fb)
    {
        while (_glQueue.Count > 0)
        {
            var action = _glQueue.Dequeue();
            action();
        }

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var width = (uint)(Bounds.Width * scale);
        var height = (uint)(Bounds.Height * scale);
        ViewModel.GL.Viewport(0, 0, width, height);

        ViewModel.GL.ClearColor(0.1f, 0.0f, 0.0f, 1.0f);
        ViewModel.GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);

        var projection = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 3f, width / (float)height, 0.001f, 1000);
        var view = _camera.GetViewTransform();
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
                        var textureName = mesh.Texture1;
                        var textureResource = ViewModel.Source.Find(mesh.Texture1, ResourceType.TPC);
                        var textureData = File.ReadAllBytes(textureResource.FilePath);
                        await ViewModel.LoadTexture.Handle((textureName, textureData));
                    }
                }
            }
        }

        var delta = (float)(DateTime.Now - _lastRender).Milliseconds / 1000;

        ViewModel.Scene.Update(ViewModel.AssetManager, delta);
        ViewModel.Scene.Render(ViewModel.AssetManager);

        _lastRender = DateTime.Now;

        RequestNextFrameRendering();
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);
    }
    #endregion

    #region Events
    private void PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        var currentPosition = e.GetPosition(this);

        if (_lastPointerPosition.HasValue)
        {
            var delta = currentPosition - _lastPointerPosition.Value;

            double deltaX = delta.X;
            double deltaY = delta.Y;

            if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
            {
                _camera.Pitch += (float)deltaY / 500;
                _camera.Yaw -= (float)deltaX / 500;
            }
        }

        _lastPointerPosition = currentPosition;
    }

    private void PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        _camera.Distance -= (float)(e.Delta.Y / 1);
    }

    private async void PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;
        var entity = await RunOnGLThread(() => Pick((int)pos.X, (int)pos.Y));
    }
    #endregion
}
