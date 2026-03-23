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
using Avalonia.Input;
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
using Kotor.NET.Graphics.Model;
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
    private float _scrollAngle;
    private DateTime _lastRender = DateTime.Now;
    private AreaEntity _area => ViewModel.Engine.Scene.Entities.OfType<AreaEntity>().First();

    public SceneControl()
    {
        InitializeComponent();
    }

    private async Task LoadDefaultResources()
    {
        _camera.Distance = 5;
        _camera.Pitch = 1;
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
        await LoadTexture("LDA_window01");
        await LoadTexture("LTS_unwal07");
        await LoadModel("sandral_floor_0");
        await LoadModel("sandral_wall_0");
        await LoadModel("sandral_wall_0_door_0");
        await LoadModel("sandral_wall_0_door_1");
        await LoadModel("sandral_icorner_0");
        await LoadModel("sandral_ocorner_0");
        await LoadModel("sandral_doorframe_0");

        ViewModel.Engine.Scene.AddEntity(new AreaEntity());
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
        var keyModifiers = e.KeyModifiers;
        var buttonProperties = e.GetCurrentPoint(this).Properties;

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        if (_lastPointerPosition.HasValue)
        {
            var delta = pos - _lastPointerPosition.Value;

            float deltaX = (float)delta.X;
            float deltaY = (float)delta.Y;

            if (buttonProperties.IsMiddleButtonPressed && keyModifiers == KeyModifiers.None)
            {
                _camera.Pitch += (float)deltaY / 500;
                _camera.Yaw -= (float)deltaX / 500;
            }
            if (buttonProperties.IsMiddleButtonPressed && keyModifiers == KeyModifiers.Shift)
            {
                Vector3 forward = new Vector3(
                    MathF.Cos(_camera.Pitch) * MathF.Cos(_camera.Yaw),
                    MathF.Cos(_camera.Pitch) * MathF.Sin(_camera.Yaw),
                    MathF.Sin(_camera.Pitch));

                forward = Vector3.Normalize(forward);

                Vector3 worldUp = Vector3.UnitZ;
                Vector3 right = Vector3.Normalize(Vector3.Cross(forward, worldUp));
                Vector3 flatForward = new Vector3(forward.X, forward.Y, 0f);

                if (flatForward.LengthSquared() > 0)
                    flatForward = Vector3.Normalize(flatForward);

                Vector3 movement = (-right * deltaX + flatForward * deltaY) * 0.01f;
                _camera.Target -= movement;
            }
        }

        _lastPointerPosition = pos;

        if (ViewModel.SceneMode == SceneMode.AddRoom)
            RenderInterceptAddRoom(mouseX, mouseY);
        if (ViewModel.SceneMode == SceneMode.SwitchWall)
            RenderInterceptSwitchWall(mouseX, mouseY);
        if (ViewModel.SceneMode == SceneMode.AddTile)
            RenderInterceptExtendRoom(mouseX, mouseY);
        if (ViewModel.SceneMode == SceneMode.AddRoom)
            RenderInterceptAddRoom(mouseX, mouseY);
    }

    private void PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        var keyModifiers = e.KeyModifiers;

        var scrollX = (float)e.Delta.X;
        var scrollY = (float)e.Delta.Y;

        if (keyModifiers == KeyModifiers.None)
        {
            _camera.Distance -= scrollY / 1;
        }
        if (keyModifiers == KeyModifiers.Control)
        {
            _scrollAngle -= scrollY * 5;
        }
    }

    private void PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var keyModifiers = e.KeyModifiers;
        var buttonProperties = e.GetCurrentPoint(this).Properties;

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        if (keyModifiers == KeyModifiers.None && buttonProperties.IsLeftButtonPressed)
        {
            if (ViewModel.SceneMode == SceneMode.AddRoom)
                PromptAddRoom(mouseX, mouseY);
            if (ViewModel.SceneMode == SceneMode.SwitchWall)
                PromptSwitchWall(mouseX, mouseY);
            if (ViewModel.SceneMode == SceneMode.AddTile)
                PromptExtendRoom(mouseX, mouseY);
        }
    }
    #endregion

    private void PromptSwitchWall(int x, int y)
    {
        var wall = NearestWallMagnest(x, y).Result;

        var menu = new ContextMenu();
        menu.Items.Add(new MenuItem() { Header = "Wall", Command = ReactiveCommand.Create(() => wall.Parent.SwitchWall(wall, WallTemplate.SandralWall0a)) });
        menu.Items.Add(new MenuItem() { Header = "Large Door", Command = ReactiveCommand.Create(() => wall.Parent.SwitchWall(wall, WallTemplate.SandralWall0b)) });
        menu.Items.Add(new MenuItem() { Header = "Small Door", Command = ReactiveCommand.Create(() => wall.Parent.SwitchWall(wall, WallTemplate.SandralWall0c)) });
        menu.Open(this);
    }
    private void RenderInterceptSwitchWall(int x, int y)
    {
        var wall = NearestWallMagnest(x, y).Result;

        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            descriptors.Where(x => x.Tag == wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        };
    }

    private void PromptExtendRoom(int x, int y)
    {
        var wall = NearestWallMagnest(x, y).Result;

        wall.Parent.Extend(wall);
    }
    private void RenderInterceptExtendRoom(int x, int y)
    {
        var wall = NearestWallMagnest(x, y).Result;

        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            descriptors.Where(x => x.Tag == wall).ToList().ForEach(x => x.AmbientColor = new(1.5f, 1.5f, 1.5f));
        };
    }

    private Room _addRoomRoom = new Room(null);
    private void PromptAddRoom(int x, int y)
    {
        var ray = _camera.ProjectRay(x, y, ViewModel.Engine.Width, ViewModel.Engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);

        _area.Area.AddRoom(_addRoomRoom);
    }
    private void RenderInterceptAddRoom(int x, int y)
    {
        var ray = _camera.ProjectRay(x, y, ViewModel.Engine.Width, ViewModel.Engine.Height);
        var point = ray.FindPointOnPlane(Axis.Z, 0);

        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            _addRoomRoom = new Room(null); 
            _addRoomRoom.Position = point;
            _addRoomRoom.Orientation = Quaternion.CreateFromYawPitchRoll(0, 0, _scrollAngle * (float)Math.PI / 180);

            (var newWall, var oldWall, var distance) = NearestAdjacentWall(_addRoomRoom);
            if (oldWall is not null)
            {
                newWall.Template = WallTemplate.SandralWall0b;

                _addRoomRoom.Orientation = oldWall.Parent.Orientation;

                if (oldWall.DoorFrame is not null)
                {
                    _addRoomRoom.Position = oldWall.Position;

                    var pos = newWall.DoorFrame.Position - newWall.Parent.Position;
                    pos += Vector3.Transform(newWall.Template.DoorFrame.Hooks.First().Position, oldWall.Orientation);
                    _addRoomRoom.Position -= pos;
                }
                else
                {
                    _addRoomRoom.Position = new(-1000, 0, 0);
                }
            }

            var roomMeshDescriptors = new List<MeshDescriptor>();
            _area.RenderRoom(ViewModel.Engine.AssetManager, _addRoomRoom, ref roomMeshDescriptors);
            roomMeshDescriptors.ForEach(x => x.AmbientColor = new Vector3(1.5f, 1.5f, 1.5f));
            descriptors.AddRange(roomMeshDescriptors);
        };
    }

    private RaycastResult<Wall>? NearestWallMagnest(int x, int y)
    {
        var ray = _camera.ProjectRay(x, y, ViewModel.Engine.Width, ViewModel.Engine.Height);

        return _area.Area.Rooms
            .SelectMany(x => x.Walls)
            .Where(x => x.LinkedTile is null)
            .OrderBy(x => ray.ShortestDistanceTo(x.Position))
            .Select(x => new RaycastResult<Wall>(x, ray.ShortestDistanceTo(x.Position)))
            .FirstOrDefault();
    }
    private (Wall ThisHook, Wall OtherHook, float distance) NearestAdjacentWall(Room room)
    {
        var near = new List<(Wall NewHook, Wall OldHook, float distance)>();
        var otherWalls = _area.Area.Rooms.SelectMany(x => x.Walls).ToList();

        foreach (var wall in room.Walls)
        {
            var match = otherWalls
                .Where(x => x.DoorFrame is not null)
                .Where(x => Vector3.Distance(wall.Position, x.Position) < 3)
                .OrderBy(x => Vector3.Distance(wall.Position, x.Position))
                .Select(x => (wall, x, Vector3.Distance(wall.Position, x.Position)))
                .ToList();

            if (match.Count > 0)
                near.AddRange(match);
        }

        return near.OrderBy(x => x.distance).FirstOrDefault();
    }
}
