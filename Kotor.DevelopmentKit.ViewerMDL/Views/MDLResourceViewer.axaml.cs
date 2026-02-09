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
            var directory = Path.GetDirectoryName(file.Path.AbsolutePath);
            ViewModel.Source = new FolderEncapsulation(directory);

            ViewModel.ModelBuffer.TryAdd("model", () =>
            {
                var mdlFilepath = file.Path.AbsolutePath;
                var mdl = File.ReadAllBytes(mdlFilepath);

                var mdxFilepath = mdlFilepath.Replace(".mdl", ".mdx");
                var mdx = File.ReadAllBytes(mdxFilepath);

                return (mdl, mdx);
            });

            ViewModel.Scene.Entities.Add(new AnimatedModel()
            {
                Model = "model",
                Animation = "cpause2",
                Transformation = Matrix4x4.Identity,
            });
        }
    }
}
