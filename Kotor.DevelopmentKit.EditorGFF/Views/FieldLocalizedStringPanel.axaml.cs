using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.Settings.Values;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Localization;
using Kotor.NET.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldLocalizedStringPanel : ReactiveUserControl<LocalizedStringPanelViewModel>
{
    public static readonly RoutedEvent<LocalizedStringEditedEventArgs> FinishedEditingEvent =
        RoutedEvent.Register<BaseEditFieldPanel, LocalizedStringEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    public static StyledProperty<InstallationSettings?> InstallationProperty =
        AvaloniaProperty.Register<FieldLocalizedStringPanel, InstallationSettings?>(nameof(Installation));

    public event EventHandler<UInt16EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }

    public InstallationSettings? Installation
    {
        get => GetValue(InstallationProperty);
        set => SetValue(InstallationProperty, value);
    }

    public FieldLocalizedStringPanel() : base()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.WhenAnyValue(x => x.Installation)
                .Where(x => ViewModel is not null)
                .BindTo(ViewModel, vm => vm.Installation)
                .DisposeWith(disposables);
        });
    }

    public void AddSubString()
    {
        ViewModel.Value.AddSubString();
        RaiseFinishedEditing();
    }

    public void RemoveSelectedSubString()
    {
        ViewModel.Value.RemoveSelectedSubString();
        RaiseFinishedEditing();
    }

    private void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new LocalizedStringEditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
