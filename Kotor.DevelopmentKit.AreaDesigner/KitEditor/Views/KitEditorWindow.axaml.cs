using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.KitEditor.ViewModels;

namespace Kotor.DevelopmentKit.AreaDesigner.KitEditor.Views;

public partial class KitEditorWindow : ReactiveWindow<KitEditorViewModel>
{
    public KitEditorWindow()
    {
        InitializeComponent();
    }
}
