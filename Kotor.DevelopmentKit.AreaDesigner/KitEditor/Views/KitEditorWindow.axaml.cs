using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.NET.Tests.Encapsulation;
using ReactiveUI;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.Views;

public partial class KitEditorWindow : ReactiveWindow<KitEditorViewModel>
{
    public KitEditorWindow()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            ViewModel.SelectKitSaveFile.RegisterHandler(SelectKitSaveFile).DisposeWith(d);
        });
    }


    public async Task SelectKitSaveFile(IInteractionContext<Unit, string?> context)
    {
        var file = await GetTopLevel(this)!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save Kit As",
            FileTypeChoices =
            [
                new FilePickerFileType("Kit File") { Patterns = ["*.kit"] },
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
}
