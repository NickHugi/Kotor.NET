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
using Avalonia.Controls.Utils;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Rendering;
using Avalonia.Threading;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.NET.Common.Data;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Cameras;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.Model.Nodes;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Factories;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;
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
        _camera.Target = new(0, 0, 2);

        await LoadTexture("LDA_flr09");
        await LoadTexture("LDA_flr05");
        await LoadTexture("lda_grate02");
        await LoadTexture("lda_trim01");
        await LoadTexture("lda_trim02");
        await LoadTexture("lda_wall03");
        await LoadTexture("lda_wall04");
        await LoadTexture("lda_wall05");
        await LoadTexture("lda_light03");
        await LoadModel("sandral_floor_0");
        await LoadModel("sandral_wall_0");
        await LoadModel("sandral_wall_0_door_0");
        await LoadModel("sandral_icorner_0");
        await LoadModel("sandral_ocorner_0");

        var floor = ViewModel.Engine.AssetManager.GetModel("sandral_floor_0");
        var magnet0 = floor.FindNode("magnet.wall.0");
        var magnet1 = floor.FindNode("magnet.wall.1");
        var magnet2 = floor.FindNode("magnet.wall.2");
        var magnet3 = floor.FindNode("magnet.wall.3");
        var magnetCorner0 = floor.FindNode("magnet.corner.0");
        var magnetCorner1 = floor.FindNode("magnet.corner.1");
        var magnetCorner2 = floor.FindNode("magnet.corner.2");
        var magnetCorner3 = floor.FindNode("magnet.corner.3");

        ViewModel.Engine.Scene.AddEntity(new RoomEntity(new Room(null)));
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
                throw;
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
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        if (_lastPointerPosition.HasValue)
        {
            var delta = pos - _lastPointerPosition.Value;

            double deltaX = delta.X;
            double deltaY = delta.Y;

            if (e.GetCurrentPoint(this).Properties.IsMiddleButtonPressed)
            {
                _camera.Pitch += (float)deltaY / 500;
                _camera.Yaw -= (float)deltaX / 500;
            }
        }

        _lastPointerPosition = pos;


        if (ViewModel.SceneMode == SceneMode.SwitchWall)
            RenderInterceptSwitchWall(mouseX, mouseY);
        if (ViewModel.SceneMode == SceneMode.AddTile)
            RenderInterceptExtendRoom(mouseX, mouseY);
    }

    private void PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        _camera.Distance -= (float)(e.Delta.Y / 1);
    }

    private void PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (!e.KeyModifiers.HasFlag(Avalonia.Input.KeyModifiers.Shift))
            return;

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        if (ViewModel.SceneMode == SceneMode.SwitchWall)
            PromptSwitchWall(mouseX, mouseY);
        if (ViewModel.SceneMode == SceneMode.AddTile)
            PromptExtendRoom(mouseX, mouseY);
    }
    #endregion

    private void PromptSwitchWall(int x, int y)
    {
        var wall = NearestWallMagnest(x, y);

        var menu = new ContextMenu();
        menu.Items.Add(new MenuItem() { Header = "sandral_wall_0", Command = ReactiveCommand.Create(() => wall.Parent.SwitchWall(wall, "sandral_wall_0")) });
        menu.Items.Add(new MenuItem() { Header = "sandral_wall_0_door_0", Command = ReactiveCommand.Create(() => wall.Parent.SwitchWall(wall, "sandral_wall_0_door_0")) });
        menu.Open(this);
    }
    private void RenderInterceptSwitchWall(int x, int y)
    {
        var wall = NearestWallMagnest(x, y);

        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            descriptors.Where(x => x.Tag == wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        };
    }

    private void PromptExtendRoom(int x, int y)
    {
        var wall = NearestWallMagnest(x, y);

        wall.Parent.Extend(wall);
    }
    private void RenderInterceptExtendRoom(int x, int y)
    {
        var wall = NearestWallMagnest(x, y);

        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            descriptors.Where(x => x.Tag == wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        };
    }

    private Wall NearestWallMagnest(int x, int y)
    {
        var ray = _camera.ProjectRay(x, y, ViewModel.Engine.Width, ViewModel.Engine.Height);

        return ViewModel.Engine.Scene.Entities.OfType<RoomEntity>()
            .SelectMany(x => x.Room.GetAllTiles())
            .SelectMany(x => x.Walls)
            .Where(x => x.LinkedTile is null)
            .OrderBy(x => ray.ShortestDistanceTo(x.Position))
            .First();
    }
}
