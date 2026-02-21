using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive.Joins;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
using Kotor.NET.Encapsulations;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.OpenGL;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class MDLResourceViewer : ReactiveWindow<MDLResourceViewerViewModel>
{
    public MDLResourceViewer()
    {
        InitializeComponent();

        ViewModel = new();
    }

    public async Task Open()
    {
        var FilePickerOpenOptions = new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Open model",
            FileTypeFilter =
            [
                new FilePickerFileType("MDL File") { Patterns = ["*.mdl"] },
            ]
        };
        var files = await GetTopLevel(this).StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);
        var file = files.FirstOrDefault();

        if (file is not null)
        {
            if (ViewModel.AssetManager.HasModel("model"))
                ViewModel.AssetManager.RemoveModel("model");

            var directory = Path.GetDirectoryName(file.Path.LocalPath);
            ViewModel.Source = new FolderEncapsulation(directory);

            ViewModel.ModelBuffer.TryAdd("model", () =>
            {
                var mdlFilepath = file.Path.LocalPath;
                var mdl = File.ReadAllBytes(mdlFilepath);

                var mdxFilepath = mdlFilepath.Replace(".mdl", ".mdx");
                var mdx = File.ReadAllBytes(mdxFilepath);

                return (mdl, mdx);
            });

            ViewModel.ModelEntity = new AnimatedModel()
            {
                Model = "model",
                Animations = [],
                Transformation = Matrix4x4.Identity,
            };
            ViewModel.Scene.Entities.Add(ViewModel.ModelEntity);
        }
    }

    private void Play_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ViewModel.ModelEntity.Animations.ForEach(x =>
        {
            x.BlendFactor = 1f;
            x.FadeFactor = 5f;
        });
        ViewModel.ModelEntity.Animations.Add(new(ViewModel.SelectedAnimation)
        {
            BlendFactor = 1,
            CurrentTime = ViewModel.ModelEntity.Animations.FirstOrDefault()?.CurrentTime ?? 0
        });
    }

    private void Pause_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ViewModel.ModelEntity.Animations.ForEach(x => x.Paused = true);
    }

    private void Swap_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ViewModel.ModelEntity.Animations.ForEach(x => x.Paused = true);
    }
}
