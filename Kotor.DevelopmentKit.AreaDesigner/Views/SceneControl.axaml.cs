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
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;
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
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using Silk.NET.Core.Native;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.AreaDesigner.Views;

public partial class SceneControl : OpenGlControlBase, ICustomHitTest, IActivatableView
{
    public AreaDesignerViewModel ViewModel => (AreaDesignerViewModel)DataContext;

    private Point? _lastPointerPosition;
    private DateTime _lastRender = DateTime.Now;
    private DesignerResourceManager _resourceManager = new(@"C:\Kits");
    private Inputs _inputs = new();

    public SceneControl()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel.SelectSaveFilepathForArea.RegisterHandler(SelectSaveFilepathForArea).DisposeWith(d);
            ViewModel.SelectLoadFilepathForArea.RegisterHandler(SelectLoadFilepathForArea).DisposeWith(d);
            ViewModel.PromptEditSettings.RegisterHandler(EditSettings).DisposeWith(d);
        });
    }

    private async Task LoadDefaultResources()
    {
        await LoadRequiredDataForKits();
    }

    private async Task LoadRequiredDataForKits()
    {
        var loadModels = Kit.Manager.Kits
            .SelectMany(kit => Directory.GetFiles($@"{Kit.Manager.ActiveDirectory}/{kit.KitID}")
                .Where(x => string.Equals(Path.GetExtension(x), ".mdl", StringComparison.InvariantCultureIgnoreCase))
                .Select(x => LoadModel(kit.KitID, Path.GetFileNameWithoutExtension(x))))
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
        var filepath = Directory.GetFiles(@"C:\Kits", "*.*", SearchOption.AllDirectories).FirstOrDefault(x =>
        {
            return Path.GetFileNameWithoutExtension(x).ToLower() == name.ToLower();
        });

        if (filepath is null)
            return;

        var texture = File.ReadAllBytes(filepath);
        var resourceType = ResourceType.FromFilepath(filepath);
        await ViewModel.Engine.LoadTexture(name, texture, resourceType);
    }

    #region ICustomHitTest
    public bool HitTest(Point point)
    {
        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        return Bounds.Contains(point + new Vector2((float)Bounds.X, (float)Bounds.Y));
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
            Scene = new AreaScene(),
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
        ViewModel.Engine.Render();

        _lastRender = DateTime.Now;
        Dispatcher.UIThread.Post(RequestNextFrameRendering);
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

        ViewModel.Scene.Mouse = new Vector2((int)pos.X, (int)pos.Y);

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        ViewModel.Scene.Mode?.MouseMove(_inputs, ViewModel.Scene);
    }

    private void PointerWheelChanged(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        var keyModifiers = e.KeyModifiers;

        var scrollX = (float)e.Delta.X;
        var scrollY = (float)e.Delta.Y;

        ViewModel.Scene.Mode?.MouseScroll(_inputs, ViewModel.Scene, new(scrollX, scrollY));
    }

    private void PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var keyModifiers = e.KeyModifiers;
        var buttonProperties = e.GetCurrentPoint(this).Properties;

        var scale = TopLevel.GetTopLevel(this).RenderScaling;
        var pos = e.GetCurrentPoint(this).Position * scale;

        var mouseX = (int)pos.X;
        var mouseY = (int)pos.Y;

        _inputs.SetMouseButtonDown(0, buttonProperties.IsLeftButtonPressed);
        _inputs.SetMouseButtonDown(1, buttonProperties.IsMiddleButtonPressed);
        _inputs.SetMouseButtonDown(2, buttonProperties.IsRightButtonPressed);

        ViewModel.Scene?.Mode?.MousePress(_inputs);
    }

    private void PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        var buttonProperties = e.GetCurrentPoint(this).Properties;

        _inputs.SetMouseButtonDown(0, buttonProperties.IsLeftButtonPressed);
        _inputs.SetMouseButtonDown(1, buttonProperties.IsMiddleButtonPressed);
        _inputs.SetMouseButtonDown(2, buttonProperties.IsRightButtonPressed);
    }

    private void UserControl_KeyDown(object? sender, KeyEventArgs e)
    {
        if (!_inputs.IsKeyDown((int)e.Key))
            ViewModel.Mode?.KeyPress(_inputs, (int)e.Key);

        _inputs.SetKeyDown((int)e.Key, true);
    }

    private void UserControl_KeyUp(object? sender, KeyEventArgs e)
    {
        _inputs.SetKeyDown((int)e.Key, false);
    }
    #endregion


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

    public async Task EditSettings(IInteractionContext<Unit, Unit> context)
    {
        var dialog = new SettingsDialog()
        {
            DataContext = App.ServiceProvider.GetService<SettingsDialogViewModel>()
        };
        await dialog.ShowDialog((Window)TopLevel.GetTopLevel(this)!);
        context.SetOutput(Unit.Default);
    }
}

public class Inputs
{
    private readonly HashSet<int> _keysDown = new();
    private readonly HashSet<int> _buttonsDown = new();

    public bool IsKeyDown(int key)
    {
        return _keysDown.Contains(key);
    }

    public bool AreKeysDown(HashSet<int> keys)
    {
        return _keysDown.SetEquals(keys);
    }

    public bool IsMouseButtonDown(int button)
    {
        return _buttonsDown.Contains(button);
    }

    public bool AreMouseButtonsDown(HashSet<int> buttons)
    {
        return _buttonsDown.SetEquals(buttons);
    }

    public void SetKeyDown(int key, bool down)
    {
        if (down)
            _keysDown.Add(key);
        else
            _keysDown.Remove(key);
    }

    public void SetMouseButtonDown(int button, bool down)
    {
        if (down)
            _buttonsDown.Add(button);
        else
            _buttonsDown.Remove(button);
    }
}
