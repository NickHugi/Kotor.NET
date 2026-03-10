using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive.Joins;
using System.Reactive.Linq;
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
            if (ViewModel.Engine.AssetManager.HasModel("model"))
                ViewModel.Engine.AssetManager.RemoveModel("model");

            var directory = Path.GetDirectoryName(file.Path.LocalPath);
            ViewModel.Engine.Source = new FolderEncapsulation(directory);

            var mdlFilepath = file.Path.LocalPath;
            var mdl = File.ReadAllBytes(mdlFilepath);

            var mdxFilepath = mdlFilepath.Replace(".mdl", ".mdx");
            var mdx = File.ReadAllBytes(mdxFilepath);

            await ViewModel.Engine.LoadModel("model", mdl, mdx);

            ViewModel.ModelEntity = ViewModel.Engine.Scene.AddEntity(new AnimatedModel
            {
                Model = "model",
                Animations = [],
                Transformation = Matrix4x4.Identity,
            });
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

    private async void Swap_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var FilePickerOpenOptions = new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Open texture",
            FileTypeFilter =
            [
                new FilePickerFileType("TPC File") { Patterns = ["*.tpc"] },
            ]
        };

        var files = await GetTopLevel(this).StorageProvider.OpenFilePickerAsync(FilePickerOpenOptions);
        var file = files.FirstOrDefault();

        if (file is not null)
        {
            var name = ViewModel.SelectedTexture.Name;
            var data = File.ReadAllBytes(file.Path.LocalPath);
            await ViewModel.Engine.LoadTexture(name, data);
        }
    }
}
