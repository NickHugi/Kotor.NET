using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.Base.Settings.Views;

public partial class SelectInstallationMenuItem : MenuItem
{
    public static readonly StyledProperty<InstallationSettings?> SelectedInstallationProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, InstallationSettings?>(nameof(SelectedInstallation), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ObservableCollection<InstallationSettings>> InstallationsProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, ObservableCollection<InstallationSettings>>(nameof(Installations));

    public InstallationSettings? SelectedInstallation
    {
        get => GetValue(SelectedInstallationProperty);
        set => SetValue(SelectedInstallationProperty, value);
    }

    public ObservableCollection<InstallationSettings> Installations
    {
        get => GetValue(InstallationsProperty);
    }

    public ObservableCollection<Control> MenuItems =>
    [
        new MenuItem()
        {
            Header = "None",
            [!MenuItem.IsCheckedProperty] = new Binding() { Source = this, Path = nameof(SelectedInstallation) },
            ToggleType = MenuItemToggleType.Radio,
            Command = ReactiveCommand.Create(() => SelectInstallation(null)),
        },
        new Separator(),
        ..Installations.Select(x => new MenuItem()
            {
                [!MenuItem.HeaderProperty] = new Binding() { Source = x, Path = nameof(x.Name) },
                [!MenuItem.IsCheckedProperty] = new Binding() { Source = this, Path = nameof(SelectedInstallation) },
                ToggleType = MenuItemToggleType.Radio,
                Command = ReactiveCommand.Create(() => SelectInstallation(x)),
            })
    ];

    protected override Type StyleKeyOverride => typeof(MenuItem);


    public SelectInstallationMenuItem()
    {
        InitializeComponent();

        this.WhenAnyValue(x => x.Installations)
            .WhereNotNull()
            .Select(x => x.ToObservableChangeSet())
            .Switch()
            .Subscribe(x =>
            {
                MenuItem.ItemsSource = MenuItems;
            });
    }

    public void SelectInstallation(InstallationSettings? installation)
    {
        SelectedInstallation = installation;
    }
}
