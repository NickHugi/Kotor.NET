using System;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DynamicData.Binding;
using Kotor.DevelopmentKit.EditorGFF.EventArgs;
using Kotor.DevelopmentKit.EditorGFF.ReactiveObjects;
using Kotor.DevelopmentKit.EditorGFF.ViewModels.FieldPanel;
using ReactiveUI;

namespace Kotor.DevelopmentKit.EditorGFF.Views;

public partial class FieldUInt8Panel : EditFieldPanel<UInt8GFFNode, Byte, UInt8EditedEventArgs>
{
    public required UInt8PanelViewModel ViewModel
    {
        get => (DataContext as UInt8PanelViewModel)!;
        set => DataContext = value;
    }


    public FieldUInt8Panel() : base()
    {
        InitializeComponent();
    }

    protected override void RaiseFinishedEditing()
    {
        RoutedEventArgs args = new UInt8EditedEventArgs(FinishedEditingEvent, this, ViewModel.Value);
        RaiseEvent(args);
    }

    protected override Byte GetCurrentValue()
    {
        return SourceNode?.FieldValue ?? 0;
    }

    private void NumericUpDown_LostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RaiseFinishedEditing();
    }
}
