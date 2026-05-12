using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
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
using Kotor.NET.Tools;
using ReactiveUI;
using Silk.NET.Core.Native;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.Views;

public partial class SceneControl : OpenGlControlBase, ICustomHitTest, IActivatableView
{
    public AreaDesignerViewModel ViewModel => (AreaDesignerViewModel)DataContext;
    
    public OrbitCamera _camera { get; } = new();
    private Point? _lastPointerPosition;
    private DateTime _lastRender = DateTime.Now;
    private DesignerResourceManager _resourceManager = new DesignerResourceManager(@"C:\Kits");

    public SceneControl()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel.SelectWallTemplate.RegisterHandler(SelectWallTemplate).DisposeWith(d);
            ViewModel.SelectTileTemplate.RegisterHandler(SelectTileTemplate).DisposeWith(d);
            ViewModel.SelectSaveFilepathForArea.RegisterHandler(SelectSaveFilepathForArea).DisposeWith(d);
            ViewModel.SelectLoadFilepathForArea.RegisterHandler(SelectLoadFilepathForArea).DisposeWith(d);
        });
    }

    private async Task LoadDefaultResources()
    {
        _camera.Distance = 5;
        _camera.Pitch = 1;
        _camera.Target = new(0, 0, 2);

        await LoadRequiredDataForKits();

        ViewModel.Engine.Scene.AddEntity(new AreaEntity());
        ViewModel.Engine.RenderInterceptor = descriptors =>
        {
            ViewModel.Mode?.RenderIntercept(_camera, _lastPointerPosition.GetValueOrDefault(), descriptors);
        };
    }

    private async Task LoadRequiredDataForKits()
    {
        var loadModels = Kit.Manager.Kits
            .SelectMany(kit => Directory.GetFiles($@"{Kit.Manager.ActiveDirectory}/{kit.ID}")
                .Where(x => string.Equals(Path.GetExtension(x), ".mdl", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => LoadModel(kit.ID, Path.GetFileNameWithoutExtension(x))))
            .ToArray();
        await Task.WhenAll(loadModels);

        var loadTextures = ViewModel.Engine.AssetManager.Models.SelectMany(x => x.Value.GetAllTextures()).Select(x => LoadTexture(x));
        await Task.WhenAll(loadTextures);
    }

    private async Task LoadModel(string kitID, string name)
    {
        var mdl = File.ReadAllBytes($@"{Kit.Manager.ActiveDirectory}/{kitID}/{name}.mdl");
        var mdx = File.ReadAllBytes($@"{Kit.Manager.ActiveDirectory}/{kitID}/{name}.mdx");
        await ViewModel.Engine.LoadModel(name, mdl, mdx);
    }
    private async Task LoadTexture(string name)
    {
        var filepath = $@"C:\Kits\sandral\{name}.tpc";
        if (!File.Exists(filepath))
            return;

        var texture = File.ReadAllBytes(filepath);
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

            if (buttonProperties.IsRightButtonPressed && keyModifiers == KeyModifiers.None)
            {
                _camera.Pitch += (float)deltaY / 500;
                _camera.Yaw -= (float)deltaX / 500;
            }
            if (buttonProperties.IsLeftButtonPressed && keyModifiers == KeyModifiers.Shift)
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
            ViewModel.Mode?.Trigger();
        }
        if (keyModifiers == KeyModifiers.None && buttonProperties.IsRightButtonPressed)
        {
            ViewModel.Mode?.AlternativeTrigger();
        }
    }
    #endregion

    public async Task SelectWallTemplate(IInteractionContext<Unit, WallTemplate?> context)
    {
        var tcs = new TaskCompletionSource<WallTemplate?>(TaskCreationOptions.RunContinuationsAsynchronously);

        var menu = new ContextMenu();
        Kit.Manager.Get("sandral").Walls.ToList().ForEach(template =>
        {
            menu.Items.Add(new MenuItem
            {
                Header = template.Name,
                Command = ReactiveCommand.Create(() => tcs.TrySetResult(template))
            });
        });
        menu.Closed += (_, __) => tcs.TrySetCanceled();
        menu.Open(this);

        try
        {
            var result = await tcs.Task;
            context.SetOutput(result);
        }
        catch (TaskCanceledException)
        {
            context.SetOutput(null);
        }
    }

    public async Task SelectTileTemplate(IInteractionContext<Unit, TileTemplate?> context)
    {
        var tcs = new TaskCompletionSource<TileTemplate?>(TaskCreationOptions.RunContinuationsAsynchronously);

        var menu = new ContextMenu();
        Kit.Manager.Get("sandral").Tiles.ToList().ForEach(template =>
        {
            menu.Items.Add(new MenuItem
            {
                Header = template.Name,
                Command = ReactiveCommand.Create(() => tcs.TrySetResult(template))
            });
        });
        menu.Closed += (_, __) => tcs.TrySetCanceled();
        menu.Open(this);

        try
        {
            var result = await tcs.Task;
            context.SetOutput(result);
        }
        catch (TaskCanceledException)
        {
            context.SetOutput(null);
        }
    }

    public async Task SelectSaveFilepathForArea(IInteractionContext<Unit, string?> context)
    {
        var file = await TopLevel.GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save Area As",
            FileTypeChoices =
            [
                new FilePickerFileType("Area File") { Patterns = ["*.area"] },
            ]
        });

        if (file is not null)
        {
            context.SetOutput(file.Path.LocalPath);
            return;
        }
        else
        {
            context.SetOutput(null);
            return;
        }
    }

    public async Task SelectLoadFilepathForArea(IInteractionContext<Unit, string?> context)
    {
        var files = await TopLevel.GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Load Area",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Area File") { Patterns = ["*.area"] },
            ]
        });

        var file = files.FirstOrDefault();

        if (file is not null)
        {
            context.SetOutput(file.Path.LocalPath);
            return;
        }
        else
        {
            context.SetOutput(null);
            return;
        }
    }
}
