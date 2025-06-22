using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings;

public partial class SelectInstallationMenuItem : MenuItem
{
    public static readonly StyledProperty<Installation> SelectedInstallationProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, Installation>(nameof(SelectedInstallation));

    public static readonly StyledProperty<ObservableCollection<Installation>> InstallationsProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, ObservableCollection<Installation>>(nameof(Installations));

    public Installation SelectedInstallation
    {
        get => GetValue(SelectedInstallationProperty);
        set => SetValue(SelectedInstallationProperty, value);
    }

    public ObservableCollection<Installation> Installations
    {
        get => GetValue(InstallationsProperty);
    }

    protected override Type StyleKeyOverride => typeof(MenuItem);

    public ObservableCollection<Control> Itemz =>
    [
        new MenuItem() { Header = "None" },
        new Separator(),
        ..Installations?.Select(x => new MenuItem() { Header = x.Name }) ?? []
    ];


    public SelectInstallationMenuItem()
    {
        InitializeComponent();

        this.WhenAnyValue(x => x.Installations).Subscribe(x =>
        {
            MenuItemX.ItemsSource = Itemz;
        });
    }
}
