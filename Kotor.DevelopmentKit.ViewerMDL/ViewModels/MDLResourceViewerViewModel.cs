using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using DynamicData;
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
    public MDLResourceViewerViewModel()
    {
        TextureSource.Connect().Subscribe(x =>
        {
            this.RaisePropertyChanged(nameof(Textures));
        });
    }
    

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
            this.RaisePropertyChanged(nameof(Textures));
            //Model.GetAllTextures().Select(x => new TextureListItem()
            //{
            //    Name = x,
            //    Source = 
            //});
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

    public string? SelectedTexture
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public ICollection<TextureListItem> Textures
    {
        get => (Model is null) ? [] : Model.GetAllTextures().Select(x => new TextureListItem()
        {
            Name = x,
            Source = (TextureSource is not null && TextureSource.Lookup(x).HasValue) ? TextureSource?.Lookup(x).Value : null
        }).ToList();
    }
    public SourceCache<string, string> TextureSource
    {
        get => field;
    } = new(s => s);
}
