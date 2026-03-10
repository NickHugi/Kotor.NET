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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class SceneControl : OpenGlControlBase, ICustomHitTest
{
    public MDLResourceViewerViewModel ViewModel => (MDLResourceViewerViewModel)DataContext;

    private Point? _lastPointerPosition;
    private DateTime _lastRender = DateTime.Now;

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
        var view = ViewModel.SceneWrapper.Camera.GetViewTransform();
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
    // make these interactives, move loadtexture loadmodel
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

    public async Task LoadTexture(string name, byte[] data)
    {
        await RunOnGLThread(() =>
        {
            if (ViewModel.AssetManager.HasTexture(name))
                ViewModel.AssetManager.RemoveTexture(name);

            using var stream = new MemoryStream(data);
            var texture = new TPCTextureFactory(ViewModel.GL).FromStream(stream);
            ViewModel.AssetManager.AddTexture(name, texture);
        });
    }
    public async Task LoadModel(string name, byte[] mdlData, byte[] mdxData)
    {
        await RunOnGLThread(async () =>
        {
            if (ViewModel.AssetManager.HasModel(name))
                ViewModel.AssetManager.RemoveModel(name);

            ViewModel.Model = new ModelLoader().LoadModel(ViewModel.GL, mdlData, mdxData);
            ViewModel.AssetManager.AddModel(name, ViewModel.Model);

            var check = new List<BaseNode>() { ViewModel.Model.Root };
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
        });
    }
    
    # region OpenGlControlBase
    protected override void OnOpenGlInit(GlInterface gl)
    {
        ViewModel.LoadTexture.RegisterHandler(async interaction =>
        {
            var name = interaction.Input.Name;
            var data = interaction.Input.Data;
            await LoadTexture(name, data);

            interaction.SetOutput(Unit.Default);
        });
        ViewModel.LoadModel.RegisterHandler(async interaction =>
        {
            var name = interaction.Input.Name;
            var mdlData = interaction.Input.MDLData;
            var mdxData = interaction.Input.MDXData;
            await LoadModel(name, mdlData, mdxData);

            interaction.SetOutput(Unit.Default);
        });

        base.OnOpenGlInit(gl);

        var context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        ViewModel.GL = new GL(context);

        ViewModel.AssetManager = new AssetManager();

        ViewModel.Scene = new();

        ViewModel.SceneWrapper = new()
        {
            AssetManager = (AssetManager)ViewModel.AssetManager,
            GL = ViewModel.GL,
            Scene= ViewModel.Scene,
        };
        ViewModel.SceneWrapper.Init();
    }

    protected async override void OnOpenGlRender(GlInterface gl, int fb)
    {
        while (_glQueue.Count > 0)
        {
            var action = _glQueue.Dequeue();
            action();
        }

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        ViewModel.SceneWrapper.Width = (uint)(Bounds.Width * scale);
        ViewModel.SceneWrapper.Height = (uint)(Bounds.Height * scale);

        var delta = (float)(DateTime.Now - _lastRender).Milliseconds / 1000;
        ViewModel.SceneWrapper.Update(delta);
        ViewModel.SceneWrapper.Render(delta);

        _lastRender = DateTime.Now;
        RequestNextFrameRendering();
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);

        ViewModel.SceneWrapper.Deinit();
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
                ViewModel.SceneWrapper.Camera.Pitch += (float)deltaY / 500;
                ViewModel.SceneWrapper.Camera.Yaw -= (float)deltaX / 500;
            }
        }

        _lastPointerPosition = currentPosition;
    }

    private void PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        ViewModel.SceneWrapper.Camera.Distance -= (float)(e.Delta.Y / 1);
    }

    private async void PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;
        var entity = await RunOnGLThread(() => Pick((int)pos.X, (int)pos.Y));
    }
    #endregion
}
