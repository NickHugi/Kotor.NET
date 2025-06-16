using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;

namespace Kotor.DevelopmentKit.Base;

public partial class SettingsDialog : Window
{
    public SettingsDialog()
    {
        DataContext = new SettingsDialogViewModel();
        InitializeComponent();
    }
}
