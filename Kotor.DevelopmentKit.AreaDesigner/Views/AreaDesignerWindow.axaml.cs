using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.Views;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using ReactiveUI;
using System.Reactive;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;
using System.Linq;
using Kotor.NET.Resources.KotorMDL;
using Kotor.NET.Common;

namespace Kotor.DevelopmentKit.AreaDesigner.Views;

public partial class AreaDesignerWindow : ReactiveWindow<AreaDesignerViewModel>
{
    public AreaDesignerWindow()
    {
        InitializeComponent();
    }

    public async Task OpenKitEditor()
    {
        var kit = ViewModel.Kits.FirstOrDefault(x => x.Active)?.Kit;

        if (kit is null)
            return;

        var dialog = new KitEditorWindow()
        {
            DataContext = new KitEditorViewModel(kit)
        };

        // todo - refresh after save changes
        await dialog.ShowDialog<Kit>(this);
    }

    private void ListBox_Initialized(object? sender, System.EventArgs e)
    {
    }

    private void SceneControl_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Delete)
        {
            ViewModel.DeleteSelected();
        }
    }
}
