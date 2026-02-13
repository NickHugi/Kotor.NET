using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kotor.DevelopmentKit.ViewerMDL.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL.Model;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.ViewModels;

public class MDLResourceViewerViewModel : ReactiveObject
{
    public AvaloniaSilkNativeContext Context
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public GL GL
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public IAssetManager AssetManager
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public KModel? Model
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            this.RaisePropertyChanged(nameof(Animations));
        }
    }

    public AnimatedModel ModelEntity
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public Scene Scene
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public IEncapsulation Source
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }

    public ConcurrentDictionary<string, Func<(byte[] MDL, byte[] MDX)>> ModelBuffer
    {
        get; set;
    } = new();
    public ConcurrentQueue<string> TextureRequests { get; } = new();

    public string? SelectedAnimation
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public List<string> Animations
    {
        get => (Model is null) ? [] : Model.Animations.Select(x => x.Name).ToList();
    }
}
