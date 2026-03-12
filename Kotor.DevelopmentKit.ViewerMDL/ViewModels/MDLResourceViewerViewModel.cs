using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using System.Resources;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DynamicData;
using Kotor.DevelopmentKit.ViewerMDL.DialogResults;
using Kotor.NET.Encapsulations;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.GPU;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Graphics.OpenGL.Model;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.ViewerMDL.ViewModels;

public class MDLResourceViewerViewModel : ReactiveObject
{
    public Interaction<Unit, SelectModelResult?> SelectModel { get; } = new();
    public Interaction<Unit, SelectTextureResult?> SelectTexture { get; } = new();

    public MDLResourceViewerViewModel()
    {
        TextureSource.Connect().Subscribe(x =>
        {
            this.RaisePropertyChanged(nameof(Textures));
        });
    }

    public Engine Engine { get; set => this.RaiseAndSetIfChanged(ref field, value); }

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

    public string? SelectedAnimation
    {
        get => field;
        set => this.RaiseAndSetIfChanged(ref field, value);
    }
    public List<string> Animations
    {
        get => (Model is null) ? [] : Model.Animations.Select(x => x.Name).ToList();
    }

    public TextureListItem? SelectedTexture
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

    public async Task LoadModel()
    {
        var resources = await SelectModel.Handle(Unit.Default);

        if (resources is not null)
        {
            if (Engine.AssetManager.HasModel("model"))
                Engine.AssetManager.RemoveModel("model");

            var directory = Path.GetDirectoryName(resources.MDL.FilePath);
            Engine.Source = new FolderEncapsulation(directory);

            var mdl = resources.MDL.ReadData();
            var mdx = resources.MDX.ReadData();
            await Engine.LoadModel("model", mdl, mdx);

            Model = Engine.AssetManager.GetModel("model");
            ModelEntity = Engine.Scene.AddEntity(new AnimatedModel
            {
                Model = "model",
                Animations = [],
            });
        }
    }

    public async Task SwapTexture()
    {
        var resource = await SelectTexture.Handle(Unit.Default);

        if (resource is not null)
        {
            var name = SelectedTexture.Name;
            var data = resource.Texture.ReadData();
            await Engine.LoadTexture(name, data);
        }
    }

    public void PlayAnimation()
    {
        ModelEntity.Animations.ForEach(x =>
        {
            x.BlendFactor = 1f;
            x.FadeFactor = 5f;
        });
        ModelEntity.Animations.Add(new(SelectedAnimation)
        {
            BlendFactor = 1,
            CurrentTime = ModelEntity.Animations.FirstOrDefault()?.CurrentTime ?? 0
        });
    }

    public void PauseAnimation()
    {
        ModelEntity.Animations.ForEach(x => x.Paused = true);
    }
}
