using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Kotor.DevelopmentKit.AreaDesigner.ViewModels;

namespace Kotor.DevelopmentKit.AreaDesigner;

public partial class KitEditorWindow : ReactiveWindow<KitEditorViewModel>
{
    public KitEditorWindow()
    {
        InitializeComponent();
    }
}
