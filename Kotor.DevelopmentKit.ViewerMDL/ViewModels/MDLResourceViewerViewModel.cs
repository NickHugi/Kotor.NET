using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Kotor.DevelopmentKit.ViewerMDL.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.GPU;
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

    //public KModel Model
    //{
    //    get => field;
    //    set => this.RaiseAndSetIfChanged(ref field, value);
    //}

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
}
