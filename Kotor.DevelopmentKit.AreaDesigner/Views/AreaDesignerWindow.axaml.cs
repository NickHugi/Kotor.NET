using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.Views;
using Kotor.DevelopmentKit.AreaDesigner.relocate;
using Kotor.DevelopmentKit.AreaDesigner.relocate.AreaExportation;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;
using Kotor.DevelopmentKit.Base;
using Kotor.DevelopmentKit.Base.Settings.ViewModels;
using Kotor.NET.Common;
using Kotor.NET.Resources.KotorMDL;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

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

    public async Task OpenAreaSettings()
    {
        if (ViewModel?.Area is null)
            return;

        var viewModel = new AreaSettingsViewModel(ViewModel.Area);
        await new AreaSettingsDialog()
        {
            DataContext = viewModel
        }.ShowDialog(this);

        viewModel.Apply(ViewModel.Area);
    }

    public async Task OpenSettings()
    {
        var viewModel = App.ServiceProvider.GetService<SettingsDialogViewModel>();
        await new SettingsDialog()
        {
            DataContext = viewModel
        }.ShowDialog(this);
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

    private void StackPanel_PointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.Source is not Control control)
            return;
        if (control.DataContext is not KitItem kitItem)
            return;

        kitItem.Active = !kitItem.Active;
    }
}
