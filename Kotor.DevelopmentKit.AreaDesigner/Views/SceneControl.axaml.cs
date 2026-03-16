using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Rendering;
using Avalonia.Threading;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Tests.Encapsulation;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.Views;

public partial class SceneControl : OpenGlControlBase, ICustomHitTest
{
    public AreaDesignerViewModel ViewModel => (AreaDesignerViewModel)DataContext;
    
    public OrbitCamera _camera { get; } = new();
    private Point? _lastPointerPosition;
    private DateTime _lastRender = DateTime.Now;

    public SceneControl()
    {
        InitializeComponent();

    }

    private async Task LoadDefaultResources()
    {
        _camera.Distance = 3;
        _camera.Pitch = 0;
        _camera.Target = new(0, 0, 1);

        //await LoadTexture("LDA_flr09");
        //await LoadTexture("LDA_flr05");
        //await LoadModel("sandral_floor_0");

        await LoadTexture("n_selkath01");
        await LoadModel("c_selkath");

        ViewModel.Engine.Scene.AddEntity(new PropEntity
        {
            Model = "c_selkath",
            Animations = [],
        });
    }
    private async Task LoadModel(string name)
    {
        var mdl = File.ReadAllBytes($@"C:\Users\hugin\Desktop\KotOR Modding Stuff\Area Designer\Sandral Estate\{name}.mdl");
        var mdx = File.ReadAllBytes($@"C:\Users\hugin\Desktop\KotOR Modding Stuff\Area Designer\Sandral Estate\{name}.mdx");
        await ViewModel.Engine.LoadModel(name, mdl, mdx);
    }
    private async Task LoadTexture(string name)
    {
        var texture = File.ReadAllBytes($@"C:\Users\hugin\Desktop\KotOR Modding Stuff\Area Designer\Sandral Estate\{name}.tpc");
        await ViewModel.Engine.LoadTexture(name, texture);
    }

    #region ICustomHitTest
    public bool HitTest(Point point)
    {
        return Bounds.Contains(point);
    }
    #endregion

    #region OpenGlControlBase
    protected override void OnOpenGlInit(GlInterface gl)
    {
        base.OnOpenGlInit(gl);

        var context = new AvaloniaSilkNativeContext(gl.GetProcAddress);
        ViewModel.Engine = new()
        {
            AssetManager = new AssetManager(),
            GL = new GL(context),
            Scene = new Scene(),
        };
        ViewModel.Engine.Init();

        Dispatcher.UIThread.Post(async () =>
        {
            try
            {
                await LoadDefaultResources();
            }
            catch (Exception ex)
            {

            }
        });
    }

    protected async override void OnOpenGlRender(GlInterface gl, int fb)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        ViewModel.Engine.Width = (uint)(Bounds.Width * scale);
        ViewModel.Engine.Height = (uint)(Bounds.Height * scale);

        var delta = (float)(DateTime.Now - _lastRender).Milliseconds / 1000;
        ViewModel.Engine.Update(delta);
        ViewModel.Engine.Render(_camera);

        _lastRender = DateTime.Now;
        RequestNextFrameRendering();
    }

    protected override void OnOpenGlDeinit(GlInterface gl)
    {
        base.OnOpenGlDeinit(gl);

        ViewModel.Engine.Deinit();
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
        var entity = await ViewModel.Engine.Pick((int)pos.X, (int)pos.Y, _camera);
    }
    #endregion
}
