using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using Kotor.DevelopmentKit.Base.ViewModels;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.GFFTreeNodes;
using Kotor.NET.Common.Data;
using Kotor.NET.Common.Localization;
using Kotor.NET.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldLocalizedStringPanel : UserControl
{
    public event EventHandler<UInt16EditedEventArgs>? FinishedEditing
    {
        add => AddHandler(FinishedEditingEvent, value);
        remove => RemoveHandler(FinishedEditingEvent, value);
    }
    public static readonly RoutedEvent<LocalizedStringEditedEventArgs> FinishedEditingEvent =
            RoutedEvent.Register<EditFieldPanel, LocalizedStringEditedEventArgs>(nameof(FinishedEditing), RoutingStrategies.Bubble);

    public required LocalizedStringPanelViewModel ViewModel
    {
        get => (DataContext as LocalizedStringPanelViewModel)!;
        set => DataContext = value;
    }


    public FieldLocalizedStringPanel() : base()
    {
        InitializeComponent();

        //this.WhenAnyValue(x => x.DataContext)
        //    .WhereNotNull()
        //    .Subscribe(x =>
        //    {
        //        ViewModel.Value.WhenAnyPropertyChanged().Subscribe(y =>
        //        {

        //        });
        //    });
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
        RoutedEventArgs args = new LocalizedStringEditedEventArgs(FinishedEditingEvent, this, ViewModel.SourcePath, ViewModel.Value);
        RaiseEvent(args);
    }

    private void TextBox_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
