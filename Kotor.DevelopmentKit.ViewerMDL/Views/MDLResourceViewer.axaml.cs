using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Joins;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Kotor.DevelopmentKit.ViewerMDL.DialogResults;
using Kotor.DevelopmentKit.ViewerMDL.ViewModels;
using Kotor.NET.Encapsulations;
using Kotor.NET.Graphics;
using Kotor.NET.Graphics.Entities;
using Kotor.NET.Graphics.OpenGL;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;
using Silk.NET.OpenGL;

namespace Kotor.DevelopmentKit.ViewerMDL.Views;

public partial class MDLResourceViewer : ReactiveWindow<MDLResourceViewerViewModel>
{
    public MDLResourceViewer()
    {
        InitializeComponent();

        ViewModel = new();

        this.WhenActivated(d =>
        {
            ViewModel.SelectModel.RegisterHandler(async context =>
            {
                context.SetOutput(await SelectModel());
            }).DisposeWith(d);

            ViewModel.SelectTexture.RegisterHandler(async context =>
            {
                context.SetOutput(await SelectTexture());
            }).DisposeWith(d);
        });
    }

    public async Task<SelectModelResult?> SelectModel()
    {
        var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Open model",
            FileTypeFilter =
            [
                new FilePickerFileType("MDL File") { Patterns = ["*.mdl"] },
            ]
        });

        var file = files.FirstOrDefault();

        if (file is not null)
        {
            var mdlPath = file.Path.LocalPath;
            var mdl = new ResourceInfo(mdlPath);

            var mdxPath = Path.ChangeExtension(mdlPath, ".mdx");
            var mdx = new ResourceInfo(mdxPath);

            return new(mdl, mdx);
        }
        else
        {
            return null;
        }
    }

    public async Task<SelectTextureResult?> SelectTexture()
    {
        var files = await GetTopLevel(this)!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Swap texture",
            FileTypeFilter =
            [
                new FilePickerFileType("Texture File") { Patterns = ["*.tpc"] },
            ]
        });

        var file = files.FirstOrDefault();

        if (file is not null)
        {
            var mdlPath = file.Path.LocalPath;
            var texture = new ResourceInfo(mdlPath);

            return new(texture);
        }
        else
        {
            return null;
        }
    }
}
