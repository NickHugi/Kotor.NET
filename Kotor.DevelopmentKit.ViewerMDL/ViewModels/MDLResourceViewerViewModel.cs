using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Kotor.DevelopmentKit.ViewerMDL.Views;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.GPU;
using ReactiveUI;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.ViewModels;

public class MDLResourceViewerViewModel : ReactiveObject
{
    private AvaloniaSilkNativeContext _context;
    public AvaloniaSilkNativeContext Context
    {
        get => _context;
        set => this.RaiseAndSetIfChanged(ref _context, value);
    }

    private GL _gl;
    public GL GL
    {
        get => _gl;
        set => this.RaiseAndSetIfChanged(ref _gl, value);
    }

    private IAssetManager _assetManager;
    public IAssetManager AssetManager
    {
        get => _assetManager;
        set => this.RaiseAndSetIfChanged(ref _assetManager, value);
    }

    private IModel _model;
    public IModel Model
    {
        get => _model;
        set => this.RaiseAndSetIfChanged(ref _model, value);
    }

    public ConcurrentDictionary<string, Func<IModel>> ModelBuffer
    {
        get; set;
    } = new();
}
