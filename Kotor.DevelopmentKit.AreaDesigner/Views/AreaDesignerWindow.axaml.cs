using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;

namespace Kotor.DevelopmentKit.AreaDesigner.Views;

public partial class AreaDesignerWindow : ReactiveWindow<AreaDesignerViewModel>
{
    public AreaDesignerWindow()
    {
        InitializeComponent();
    }

    public async Task OpenKitEditor()
    {
        if (ViewModel.SelectedKit is null)
            return;

        var dialog = new KitEditorWindow();
        dialog.ViewModel = new(ViewModel.SelectedKit);
        // todo - save changes
        ViewModel.SelectedKit = await dialog.ShowDialog<Kit>(this);
    }
}
