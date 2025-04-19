using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Kotor.DevelopmentKit.Base.ViewModels;

namespace Kotor.DevelopmentKit.Base.Views;

public partial class EncapsulatedResourceList : UserControl
{
    public EncapsulatedResourceListViewModel Context => (EncapsulatedResourceListViewModel)DataContext!;

    public EncapsulatedResourceList()
    {
        InitializeComponent();
    }
}
