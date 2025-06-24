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

namespace Kotor.DevelopmentKit.Base.Settings;

public partial class SelectInstallationMenuItem : MenuItem
{
    public static readonly StyledProperty<Installation?> SelectedInstallationProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, Installation?>(nameof(SelectedInstallation), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<ObservableCollection<Installation>> InstallationsProperty
        = AvaloniaProperty.Register<SelectInstallationMenuItem, ObservableCollection<Installation>>(nameof(Installations));

    public Installation? SelectedInstallation
    {
        get => GetValue(SelectedInstallationProperty);
        set => SetValue(SelectedInstallationProperty, value);
    }

    public ObservableCollection<Installation> Installations
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

    public void SelectInstallation(Installation? installation)
    {
        SelectedInstallation = installation;
    }
}
